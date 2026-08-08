using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;
using SimpleTextEditor.Services;

namespace SimpleTextEditor.UI
{
    public static partial class FormatToolbarBuilder
    {
        public static ToolStrip BuildFormatToolbar(Form1 mainForm)
        {
            var toolbar = new ToolStrip
            {
                BackColor  = AppTheme.SurfaceHigh,
                ForeColor  = AppTheme.TextPrimary,
                GripStyle  = ToolStripGripStyle.Hidden,
                Padding    = new Padding(10, 4, 10, 4),
                Renderer   = new DarkToolStripRenderer()
            };

            var btnHeadings = BuildHeadingsDropdown(mainForm);
            var btnLists    = BuildListsDropdown(mainForm);
            var btnTable    = BuildTableDropdown(mainForm);
            var btnCodeBlock = BuildCodeBlockDropdown(mainForm);
            
            var inlineFmt = BuildInlineFormatting(mainForm);
            
            // Search box (right-aligned)
            var searchControls = BuildSearchControls(mainForm);

            // Assemble
            toolbar.Items.AddRange(new ToolStripItem[]
            {
                btnHeadings, new ToolStripSeparator(),
                btnLists,    new ToolStripSeparator()
            });
            toolbar.Items.AddRange(inlineFmt);
            toolbar.Items.AddRange(new ToolStripItem[]
            {
                btnTable, btnCodeBlock,
                new ToolStripSeparator(),
                BuildClearButton(mainForm),
                searchControls.btnFind, searchControls.txtSearch
            });

            return toolbar;
        }

        private static (ToolStripTextBox txtSearch, ToolStripButton btnFind) BuildSearchControls(Form1 mainForm)
        {
            var txtSearch = new ToolStripTextBox
            {
                Alignment   = ToolStripItemAlignment.Right,
                BackColor   = AppTheme.Surface,
                ForeColor   = AppTheme.TextMuted,
                BorderStyle = BorderStyle.FixedSingle,
                Size        = new Size(190, 24),
                Font        = new Font("Segoe UI", 9.5f),
                Margin      = new Padding(0, 0, 8, 0),
                Text        = "🔍  Пошук…"
            };
            txtSearch.Enter += (s, e) => { if (txtSearch.Text.Contains("Пошук")) { txtSearch.Text = ""; txtSearch.ForeColor = AppTheme.TextEditor; } };
            txtSearch.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "🔍  Пошук…"; txtSearch.ForeColor = AppTheme.TextMuted; } };

            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    SearchService.DoFind(mainForm, txtSearch.Text, false);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    mainForm.tabManager.CurrentEditor?.Focus();
                }
                else if (e.Control && e.KeyCode == Keys.H)
                {
                    e.SuppressKeyPress = true;
                    SearchService.DoReplace(mainForm, txtSearch.Text);
                }
            };

            var btnFind = new ToolStripButton(null, Icons.Find, (s, e) => SearchService.DoFind(mainForm, txtSearch.Text, false))
            {
                Alignment    = ToolStripItemAlignment.Right,
                Padding      = new Padding(4, 0, 4, 0),
                ToolTipText  = "Знайти далі  (Enter у полі пошуку)   |   Ctrl+H — Замінити",
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None
            };
            return (txtSearch, btnFind);
        }

        private static ToolStripButton BuildClearButton(Form1 mainForm)
        {
            return ToolbarItemFactory.MakeIconBtn(Icons.Clear, "Очистити Markdown-символи з виділеного",
                (s, e) =>
                {
                    var rtb = mainForm.tabManager.CurrentEditor;
                    if (rtb != null)
                        rtb.SelectedText = rtb.SelectedText
                            .Replace("**", "").Replace("__", "").Replace("_", "")
                            .Replace("~~", "").Replace("`", "");
                });
        }
    }
}
