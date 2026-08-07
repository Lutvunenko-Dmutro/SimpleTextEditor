using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;
namespace SimpleTextEditor.UI
{
    public class TabManager
    {
        private FlowLayoutPanel tabStrip;
        private Panel contentPanel;

        private List<RichTextBox> openEditors = new List<RichTextBox>();
        private Dictionary<RichTextBox, string> editorFiles = new Dictionary<RichTextBox, string>();
        private Dictionary<RichTextBox, Button> editorTabs = new Dictionary<RichTextBox, Button>();
        private Dictionary<RichTextBox, string> editorSyntax = new Dictionary<RichTextBox, string>();

        public MarkdownPreviewManager PreviewManager { get; private set; }

        public RichTextBox CurrentEditor { get; private set; }

        public event EventHandler EditorChanged;
        public event EventHandler NoTabsLeft;

        public TabManager(FlowLayoutPanel tabStrip, Panel contentPanel)
        {
            this.tabStrip = tabStrip;
            this.contentPanel = contentPanel;

            PreviewManager = new MarkdownPreviewManager(contentPanel);
        }

        public void AddNewTab(string title, string content, string filePath = null, string syntax = "Звичайний текст")
        {
            Panel editorContainer = new Panel();
            editorContainer.Dock = DockStyle.Fill;
            
            Panel linePanel = new Panel();
            linePanel.Dock = DockStyle.Left;
            linePanel.Width = 40;
            linePanel.BackColor = AppTheme.Background;
            linePanel.ForeColor = AppTheme.TextMuted;

            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.BackColor = AppTheme.Background;
            rtb.ForeColor = AppTheme.TextEditor;
            rtb.Font = new Font("Consolas", 12F, FontStyle.Regular);
            rtb.BorderStyle = BorderStyle.None;
            rtb.Text = content;

            linePanel.Paint += (s, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                
                int firstIndex = rtb.GetCharIndexFromPosition(new Point(0, 0));
                int firstLine = rtb.GetLineFromCharIndex(firstIndex);
                
                Point pt = new Point(0, linePanel.Height);
                int lastIndex = rtb.GetCharIndexFromPosition(pt);
                int lastLine = rtb.GetLineFromCharIndex(lastIndex);

                using (Font font = new Font(rtb.Font.FontFamily, rtb.Font.Size - 1))
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    for (int i = firstLine; i <= lastLine + 1; i++)
                    {
                        if (i >= rtb.Lines.Length) break;
                        int charIndex = rtb.GetFirstCharIndexFromLine(i);
                        if (charIndex == -1) break;
                        
                        Point pos = rtb.GetPositionFromCharIndex(charIndex);
                        e.Graphics.DrawString((i + 1).ToString(), font, brush, 5, pos.Y);
                    }
                }
            };
            rtb.VScroll += (s, e) => linePanel.Invalidate();
            rtb.TextChanged += (s, e) => linePanel.Invalidate();
            rtb.FontChanged += (s, e) => linePanel.Invalidate();
            rtb.Resize += (s, e) => linePanel.Invalidate();

            editorContainer.Controls.Add(linePanel);
            editorContainer.Controls.Add(rtb);
            rtb.BringToFront(); // Ensures rtb takes the remaining space after linePanel

            rtb.SelectionChanged += (s, e) => EditorChanged?.Invoke(s, e);
            rtb.TextChanged += (s, e) => EditorChanged?.Invoke(s, e);

            openEditors.Add(rtb);
            contentPanel.Controls.Add(editorContainer);
            editorSyntax[rtb] = syntax;
            PreviewManager.SetPreviewMode(rtb, false, null);
            if (filePath != null) editorFiles[rtb] = filePath;

            Button tabBtn = new Button();
            tabBtn.Text = title;
            tabBtn.FlatStyle = FlatStyle.Flat;
            tabBtn.FlatAppearance.BorderSize = 0;
            tabBtn.Size = new Size(150, 35);
            tabBtn.Margin = new Padding(0);
            tabBtn.Font = new Font("Segoe UI", 9F);
            tabBtn.Click += (s, e) => SelectTab(rtb);
            
            tabBtn.MouseEnter += (s, e) => {
                if (CurrentEditor != rtb) {
                    tabBtn.BackColor = AppTheme.Surface;
                    tabBtn.ForeColor = AppTheme.TextSecondary;
                }
            };
            tabBtn.MouseLeave += (s, e) => {
                if (CurrentEditor != rtb) {
                    tabBtn.BackColor = AppTheme.Background;
                    tabBtn.ForeColor = AppTheme.TextMuted;
                }
            };

            editorTabs[rtb] = tabBtn;
            tabStrip.Controls.Add(tabBtn);

            SelectTab(rtb);
        }

        public void SelectTab(RichTextBox rtb)
        {
            if (rtb == null || !openEditors.Contains(rtb)) return;

            CurrentEditor = rtb;
            bool isPreview = PreviewManager.IsPreviewMode(rtb);

            foreach (var editor in openEditors)
            {
                if (editor == rtb)
                {
                    editor.Parent.Visible = !isPreview;
                    if (!isPreview) editor.Parent.BringToFront();
                    Button btn = editorTabs[editor];
                    btn.BackColor = AppTheme.Accent;
                    btn.ForeColor = AppTheme.TextPrimary;
                }
                else
                {
                    editor.Parent.Visible = false;
                    Button btn = editorTabs[editor];
                    btn.BackColor = AppTheme.Background;
                    btn.ForeColor = AppTheme.TextMuted;
                }
            }

            PreviewManager.ShowPreviewIfActive(rtb);

            EditorChanged?.Invoke(rtb, EventArgs.Empty);
        }



        public void CloseTab(RichTextBox rtb)
        {
            if (rtb == null || !openEditors.Contains(rtb)) return;

            if (openEditors.Count > 1)
            {
                Button btn = editorTabs[rtb];
                tabStrip.Controls.Remove(btn);
                contentPanel.Controls.Remove(rtb.Parent);
                
                openEditors.Remove(rtb);
                editorTabs.Remove(rtb);
                editorFiles.Remove(rtb);
                PreviewManager.RemoveEditor(rtb);
                
                if (CurrentEditor == rtb)
                {
                    SelectTab(openEditors[openEditors.Count - 1]);
                }
            }
            else
            {
                NoTabsLeft?.Invoke(this, EventArgs.Empty);
            }
        }

        public void CloseCurrentTab()
        {
            CloseTab(CurrentEditor);
        }

        public IEnumerable<RichTextBox> GetAllEditors() => openEditors;
        public int EditorCount => openEditors.Count;
        
        public string GetFilePath(RichTextBox rtb) => editorFiles.GetValueOrDefault(rtb);
        public string GetSyntax(RichTextBox rtb) => editorSyntax.GetValueOrDefault(rtb, "Звичайний текст");

        public void SetFilePath(RichTextBox rtb, string path)
        {
            editorFiles[rtb] = path;
            if (path != null && editorTabs.TryGetValue(rtb, out Button btn))
            {
                btn.Text = System.IO.Path.GetFileName(path);
            }
        }
    }
}
