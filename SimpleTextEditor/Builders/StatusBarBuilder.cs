using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class StatusBarBuilder
    {
        public static void BuildStatusBar(Form1 mainForm)
        {
            var statusStrip = mainForm.statusStrip1;
            statusStrip.BackColor   = AppTheme.Background;
            statusStrip.ForeColor   = AppTheme.TextMuted;
            statusStrip.SizingGrip  = false;
            statusStrip.Font        = new Font("Segoe UI", 9f);
            statusStrip.Renderer    = new DarkToolStripRenderer();
            statusStrip.Padding     = new Padding(4, 0, 4, 0);

            // ── Cursor position ───────────────────────────────────────────────────
            mainForm.statusLabel = new ToolStripStatusLabel("Рядок 1, стовп. 1")
            {
                ForeColor = AppTheme.TextSecondary,
                Padding   = new Padding(6, 0, 6, 0)
            };

            // ── Syntax / mode selector ────────────────────────────────────────────
            mainForm.syntaxLabel = new ToolStripDropDownButton("Звичайний текст")
            {
                ForeColor    = AppTheme.AccentLight,
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font         = new Font("Segoe UI", 9f, FontStyle.Bold),
                Padding      = new Padding(6, 0, 6, 0)
            };

            var normalItem = new ToolStripMenuItem("Звичайний текст", null, (s, e) =>
            {
                mainForm.syntaxLabel.Text = "Звичайний текст";
                if (mainForm.tabManager.CurrentEditor != null)
                    mainForm.tabManager.PreviewManager.SetPreviewMode(
                        mainForm.tabManager.CurrentEditor, false, mainForm.tabManager.CurrentEditor);
            });
            var mdSyntaxItem = new ToolStripMenuItem("Синтаксис Markdown", null, (s, e) =>
            {
                mainForm.syntaxLabel.Text = "Markdown";
                if (mainForm.tabManager.CurrentEditor != null)
                    mainForm.tabManager.PreviewManager.SetPreviewMode(
                        mainForm.tabManager.CurrentEditor, false, mainForm.tabManager.CurrentEditor);
            });
            var mdPreviewItem = new ToolStripMenuItem("Форматований", null, (s, e) =>
            {
                mainForm.syntaxLabel.Text = "Форматований";
                if (mainForm.tabManager.CurrentEditor != null)
                    mainForm.tabManager.PreviewManager.SetPreviewMode(
                        mainForm.tabManager.CurrentEditor, true, mainForm.tabManager.CurrentEditor);
            });

            StyleDropDownItems(mainForm.syntaxLabel, normalItem, mdSyntaxItem, mdPreviewItem);
            mainForm.syntaxLabel.DropDownItems.AddRange(
                new ToolStripItem[] { normalItem, mdSyntaxItem, mdPreviewItem });

            // ── Zoom ──────────────────────────────────────────────────────────────
            mainForm.zoomLabel = new ToolStripStatusLabel("100%")
            {
                ForeColor = AppTheme.TextMuted,
                Padding   = new Padding(6, 0, 6, 0)
            };

            // ── Encoding & line-ending ────────────────────────────────────────────
            mainForm.encodingLabel = new ToolStripStatusLabel("UTF-8")
            {
                ForeColor = AppTheme.TextMuted,
                Padding   = new Padding(6, 0, 6, 0)
            };
            mainForm.formatLabel = new ToolStripStatusLabel("CRLF")
            {
                ForeColor = AppTheme.TextMuted,
                Padding   = new Padding(6, 0, 6, 0)
            };

            // ── Character count (right-aligned spring) ────────────────────────────
            var springLabel = new ToolStripStatusLabel { Spring = true };
            var charCountLabel = new ToolStripStatusLabel("0 симв.")
            {
                ForeColor = AppTheme.TextMuted,
                Padding   = new Padding(6, 0, 6, 0)
            };
            mainForm.Tag = charCountLabel; // store ref so Form1 can update it

            // ── Separator helper ──────────────────────────────────────────────────
            ToolStripSeparator Sep() => new ToolStripSeparator();

            statusStrip.Items.AddRange(new ToolStripItem[]
            {
                mainForm.statusLabel,
                Sep(),
                mainForm.syntaxLabel,
                Sep(),
                mainForm.zoomLabel,
                Sep(),
                mainForm.formatLabel,
                Sep(),
                mainForm.encodingLabel,
                springLabel,
                charCountLabel
            });
        }

        private static void StyleDropDownItems(ToolStripDropDownButton parent,
                                               params ToolStripMenuItem[] items)
        {
            parent.DropDownOpening += (s, e) =>
            {
                foreach (var item in items)
                {
                    item.BackColor = AppTheme.Surface;
                    item.ForeColor = AppTheme.TextPrimary;
                }
            };
        }
    }
}
