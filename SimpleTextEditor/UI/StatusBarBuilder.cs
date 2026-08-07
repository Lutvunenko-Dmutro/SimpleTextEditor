using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class StatusBarBuilder
    {
        public static void BuildStatusBar(Form1 mainForm)
        {
            var statusStrip = mainForm.statusStrip1; // Assuming statusStrip1 is exposed or we create a new one. Wait, in Form1 it's internal.
            statusStrip.BackColor = AppTheme.Background;
            statusStrip.ForeColor = AppTheme.TextMuted;
            statusStrip.SizingGrip = false;
            
            mainForm.statusLabel = new ToolStripStatusLabel("Рядок 1, стовпець 1 | 0 символів");
            
            mainForm.syntaxLabel = new ToolStripDropDownButton("Звичайний текст");
            mainForm.syntaxLabel.ForeColor = AppTheme.AccentLight;
            mainForm.syntaxLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            var normalItem = new ToolStripMenuItem("Звичайний текст", null, (s, e) => {
                mainForm.syntaxLabel.Text = "Звичайний текст";
                if (mainForm.tabManager.CurrentEditor != null) 
                    mainForm.tabManager.PreviewManager.SetPreviewMode(mainForm.tabManager.CurrentEditor, false, mainForm.tabManager.CurrentEditor);
            });
            var mdSyntaxItem = new ToolStripMenuItem("Синтаксис Markdown", null, (s, e) => {
                mainForm.syntaxLabel.Text = "Синтаксис Markdown";
                if (mainForm.tabManager.CurrentEditor != null) 
                    mainForm.tabManager.PreviewManager.SetPreviewMode(mainForm.tabManager.CurrentEditor, false, mainForm.tabManager.CurrentEditor);
            });
            var mdPreviewItem = new ToolStripMenuItem("Форматований", null, (s, e) => {
                mainForm.syntaxLabel.Text = "Форматований";
                if (mainForm.tabManager.CurrentEditor != null) 
                    mainForm.tabManager.PreviewManager.SetPreviewMode(mainForm.tabManager.CurrentEditor, true, mainForm.tabManager.CurrentEditor);
            });
            mainForm.syntaxLabel.DropDownItems.AddRange(new ToolStripItem[] { normalItem, mdSyntaxItem, mdPreviewItem });

            mainForm.zoomLabel = new ToolStripStatusLabel("100%");
            mainForm.encodingLabel = new ToolStripStatusLabel("UTF-8");
            mainForm.formatLabel = new ToolStripStatusLabel("Windows (CRLF)");
            
            statusStrip.Items.Add(mainForm.statusLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(mainForm.syntaxLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(mainForm.zoomLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(mainForm.formatLabel);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(mainForm.encodingLabel);
        }
    }
}
