using System;
using System.Windows.Forms;
using System.Drawing;
using SimpleTextEditor.Services;
using SimpleTextEditor.UI;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel tabStrip;
        private Panel contentPanel;
        
        internal ToolStripStatusLabel    statusLabel;
        internal ToolStripDropDownButton syntaxLabel;
        internal ToolStripStatusLabel    zoomLabel;
        internal ToolStripStatusLabel    encodingLabel;
        internal ToolStripStatusLabel    formatLabel;
        
        internal PrintHandler         printHandler  = new PrintHandler();
        internal FormatHandler         formatHandler = new FormatHandler();
        internal TabManager            tabManager;
        internal NotificationService   notifier;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsTheme.ApplyDarkThemeToAllControls(this);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WindowsTheme.EnableDarkMode(this.Handle);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED (fixes flickering)
                return cp;
            }
        }

        public Form1()
        {
            InitializeComponent();
            
            this.Text      = "Simple Text Editor";
            this.BackColor = AppTheme.Background;
            this.ForeColor = AppTheme.TextPrimary;
            this.MinimumSize = new Size(800, 520);
            
            // Hide legacy designer controls
            richTextBox1.Visible = false;
            toolStrip1.Visible   = false;

            var (tabStripResult, contentPanelResult) = MainLayoutBuilder.BuildLayout(this);
            tabStrip = tabStripResult;
            contentPanel = contentPanelResult;

            tabManager = new TabManager(tabStrip, contentPanel);
            tabManager.EditorChanged += TabManager_EditorChanged;
            tabManager.NoTabsLeft    += (s, e) => this.Close();

            // ── Notification service (needs statusLabel, built in StatusBar) ────────
            // Will be wired after the first render; use a post-load hook
            this.Shown += (s, e) =>
            {
                notifier = new NotificationService(statusLabel);
                formatHandler.Notifier = notifier;
            };

            // ── Keyboard shortcuts ─────────────────────────────────────────────────
            this.KeyPreview = true;
            this.KeyDown   += Form1_KeyDown;
            
            tabManager.AddNewTab("Untitled", "");
        }

        // ── Keyboard handler ───────────────────────────────────────────────────────
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+Tab = next tab,  Ctrl+Shift+Tab = prev tab
            if (e.Control && !e.Alt && e.KeyCode == Keys.Tab)
            {
                if (e.Shift) tabManager.SelectPrevTab();
                else         tabManager.SelectNextTab();
                e.SuppressKeyPress = true;
                return;
            }

            var rtb = tabManager.CurrentEditor;
            if (rtb == null) return;
            bool formatted = tabManager.IsFormattedMode(rtb);

            // Ctrl+B — Bold
            if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.B)
            {
                e.SuppressKeyPress = true;
                if (formatted) formatHandler.ToggleBold(rtb);
                else { int s = rtb.SelectionStart; rtb.SelectedText = "**" + rtb.SelectedText + "**"; rtb.SelectionStart = s + 2; }
            }
            // Ctrl+I — Italic
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.I)
            {
                e.SuppressKeyPress = true;
                if (formatted) formatHandler.ToggleItalic(rtb);
                else { int s = rtb.SelectionStart; rtb.SelectedText = "_" + rtb.SelectedText + "_"; rtb.SelectionStart = s + 1; }
            }
            // Ctrl+U — Underline
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.U)
            {
                e.SuppressKeyPress = true;
                if (formatted) formatHandler.ToggleUnderline(rtb);
            }
        }

        // ── Editor changed (status bar update) ────────────────────────────────────
        private void TabManager_EditorChanged(object sender, EventArgs e)
        {
            var rtb = sender as RichTextBox;
            if (rtb == null) return;

            int idx     = rtb.SelectionStart;
            int line    = rtb.GetLineFromCharIndex(idx);
            int first   = rtb.GetFirstCharIndexFromLine(line);
            int column  = idx - first;
            int total   = rtb.Text.Length;
            int selected = rtb.SelectionLength;

            statusLabel.Text = $"Рядок {line + 1}, стовп. {column + 1}" +
                               (selected > 0 ? $"  ({selected} вибр.)" : "");

            // char count label (stored in Tag)
            if (this.Tag is ToolStripStatusLabel charLbl)
                charLbl.Text = $"{total:N0} симв.";

            bool isPreview = tabManager.PreviewManager.IsPreviewMode(rtb);
            if (isPreview)
                syntaxLabel.Text = "Форматований";
            else
                syntaxLabel.Text = tabManager.GetSyntax(rtb) == "Markdown"
                    ? "Markdown" : tabManager.GetSyntax(rtb);
        }

        // ── File operations ────────────────────────────────────────────────────────
        internal void OpenFile(object sender, EventArgs e)
        {
            var result = FileHandler.OpenFile();
            if (result.fileName == null) return;

            string title         = System.IO.Path.GetFileName(result.fileName);
            var    currentEditor = tabManager.CurrentEditor;
            
            bool isEmpty = tabManager.EditorCount == 1
                        && string.IsNullOrEmpty(tabManager.GetFilePath(currentEditor))
                        && string.IsNullOrEmpty(currentEditor?.Text);

            if (isEmpty)
            {
                currentEditor.Text = result.content;
                tabManager.SetFilePath(currentEditor, result.fileName);
            }
            else
            {
                tabManager.AddNewTab(title, result.content, result.fileName);
            }
        }

        internal void SaveFile(object sender, EventArgs e)    => PerformSave(tabManager.CurrentEditor, false);
        internal void SaveAsFile(object sender, EventArgs e)  => PerformSave(tabManager.CurrentEditor, true);

        internal void SaveAllFiles(object sender, EventArgs e)
        {
            foreach (var editor in tabManager.GetAllEditors())
                PerformSave(editor, false);
        }

        private void PerformSave(RichTextBox editor, bool forceSaveAs)
        {
            if (editor == null) return;
            tabManager.PreviewManager.SyncPreviewToEditor(editor);
            
            string currentPath = forceSaveAs ? null : tabManager.GetFilePath(editor);
            string savedPath   = FileHandler.SaveFile(editor.Text, currentPath);
            
            if (savedPath != null)
            {
                tabManager.SetFilePath(editor, savedPath);
                tabManager.MarkSaved(editor);
            }
        }

        internal void ShowAboutForm(object sender, EventArgs e)
        {
            using var f = new AboutForm();
            f.ShowDialog(this);
        }

        internal void PageSetup(object sender, EventArgs e)
        {
            using var psd = new PageSetupDialog { Document = printDocument1 };
            psd.ShowDialog();
        }

        internal void PrintFile(object sender, EventArgs e)
        {
            if (tabManager.CurrentEditor != null)
                printHandler.PrintFile(tabManager.CurrentEditor.Text, tabManager.CurrentEditor.Font);
        }

        // ── Designer stubs ─────────────────────────────────────────────────────────
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e) { }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) { }
    }
}
