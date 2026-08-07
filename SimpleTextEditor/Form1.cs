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
        
        internal ToolStripStatusLabel statusLabel;
        internal ToolStripDropDownButton syntaxLabel;
        internal ToolStripStatusLabel zoomLabel;
        internal ToolStripStatusLabel encodingLabel;
        internal ToolStripStatusLabel formatLabel;
        
        internal PrintHandler printHandler = new PrintHandler();
        internal FormatHandler formatHandler = new FormatHandler();
        internal TabManager tabManager;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.WindowsTheme.EnableDarkMode(this.Handle);
        }

        public Form1()
        {
            InitializeComponent();
            
            this.Text = "Simple Text Editor";
            this.BackColor = AppTheme.Background;
            this.ForeColor = AppTheme.TextPrimary;
            
            // Hide old controls
            richTextBox1.Visible = false;
            toolStrip1.Visible = false;

            // Custom Tab System
            tabStrip = new FlowLayoutPanel();
            tabStrip.Dock = DockStyle.Fill;
            tabStrip.BackColor = AppTheme.Background;
            tabStrip.WrapContents = false;
            tabStrip.AutoScroll = true;
            tabStrip.Margin = new Padding(0);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = AppTheme.Background;
            contentPanel.Margin = new Padding(0);
            contentPanel.Padding = new Padding(10, 5, 10, 5);

            tabManager = new TabManager(tabStrip, contentPanel);
            tabManager.EditorChanged += TabManager_EditorChanged;
            tabManager.NoTabsLeft += (s, e) => this.Close();

            // StatusStrip Setup
            StatusBarBuilder.BuildStatusBar(this);

            // Menu & Toolbar Setup (Combined)
            var mainMenu = MenuBuilder.BuildCombinedMenu(this);
            mainMenu.Dock = DockStyle.Fill;
            mainMenu.Margin = new Padding(0);
            
            var formatToolbar = FormatToolbarBuilder.BuildFormatToolbar(this);
            formatToolbar.AutoSize = true;

            TableLayoutPanel toolbarContainer = new TableLayoutPanel();
            toolbarContainer.Dock = DockStyle.Fill;
            toolbarContainer.RowCount = 1;
            toolbarContainer.ColumnCount = 3;
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            toolbarContainer.BackColor = AppTheme.Background;
            toolbarContainer.Margin = new Padding(0);
            
            toolbarContainer.Controls.Add(formatToolbar, 1, 0);
            
            // Build absolute layout structure
            TableLayoutPanel layoutPanel = new TableLayoutPanel();
            layoutPanel.Dock = DockStyle.Fill;
            layoutPanel.RowCount = 5;
            layoutPanel.ColumnCount = 1;
            layoutPanel.Margin = new Padding(0);
            layoutPanel.BackColor = AppTheme.Background;
            
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); // tabStrip
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // mainMenu
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // formatToolbar
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // contentPanel
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // statusStrip1
            
            layoutPanel.Controls.Add(tabStrip, 0, 0);
            layoutPanel.Controls.Add(mainMenu, 0, 1);
            layoutPanel.Controls.Add(toolbarContainer, 0, 2);
            layoutPanel.Controls.Add(contentPanel, 0, 3);
            layoutPanel.Controls.Add(statusStrip1, 0, 4);
            
            this.Controls.Add(layoutPanel);
            
            tabManager.AddNewTab("Untitled", "");
        }

        private void TabManager_EditorChanged(object sender, EventArgs e)
        {
            var rtb = sender as RichTextBox;
            if (rtb == null) return;

            int index = rtb.SelectionStart;
            int line = rtb.GetLineFromCharIndex(index);
            int firstChar = rtb.GetFirstCharIndexFromLine(line);
            int column = index - firstChar;
            int totalChars = rtb.Text.Length;
            
            statusLabel.Text = $"Рядок {line + 1}, стовпець {column + 1} | {totalChars} символів";
            
            bool isPreview = tabManager.PreviewManager.IsPreviewMode(rtb);
            if (isPreview) syntaxLabel.Text = "Форматований";
            else syntaxLabel.Text = tabManager.GetSyntax(rtb) == "Markdown" ? "Синтаксис Markdown" : tabManager.GetSyntax(rtb);
        }

        internal void OpenFile(object sender, EventArgs e)
        {
            var result = FileHandler.OpenFile();
            if (result.fileName != null)
            {
                string title = System.IO.Path.GetFileName(result.fileName);
                var currentEditor = tabManager.CurrentEditor;
                
                if (tabManager.EditorCount == 1 && string.IsNullOrEmpty(tabManager.GetFilePath(currentEditor)) && string.IsNullOrEmpty(currentEditor.Text))
                {
                    currentEditor.Text = result.content;
                    tabManager.SetFilePath(currentEditor, result.fileName);
                }
                else
                {
                    tabManager.AddNewTab(title, result.content, result.fileName);
                }
            }
        }

        internal void SaveFile(object sender, EventArgs e) => PerformSave(tabManager.CurrentEditor, false);
        internal void SaveAsFile(object sender, EventArgs e) => PerformSave(tabManager.CurrentEditor, true);
        
        internal void SaveAllFiles(object sender, EventArgs e)
        {
            foreach (var editor in tabManager.GetAllEditors())
            {
                PerformSave(editor, false);
            }
        }

        private void PerformSave(RichTextBox editor, bool forceSaveAs)
        {
            if (editor == null) return;
            tabManager.PreviewManager.SyncPreviewToEditor(editor);
            
            string currentPath = forceSaveAs ? null : tabManager.GetFilePath(editor);
            string savedPath = FileHandler.SaveFile(editor.Text, currentPath);
            
            if (savedPath != null)
            {
                tabManager.SetFilePath(editor, savedPath);
            }
        }

        internal void ShowAboutForm(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        internal void PageSetup(object sender, EventArgs e)
        {
            using (PageSetupDialog psd = new PageSetupDialog())
            {
                psd.Document = printDocument1;
                psd.ShowDialog();
            }
        }

        internal void PrintFile(object sender, EventArgs e)
        {
            if (tabManager.CurrentEditor != null)
            {
                printHandler.PrintFile(tabManager.CurrentEditor.Text, tabManager.CurrentEditor.Font);
            }
        }

        // Keep stubs for designer events
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e) { }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) { }
    }
}
