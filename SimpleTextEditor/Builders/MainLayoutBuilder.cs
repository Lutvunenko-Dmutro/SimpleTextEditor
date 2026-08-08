using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class MainLayoutBuilder
    {
        public static (FlowLayoutPanel tabStrip, Panel contentPanel) BuildLayout(Form1 mainForm)
        {
            // ── Tab strip ──────────────────────────────────────────────────────────
            var tabStrip = new FlowLayoutPanel
            {
                Dock         = DockStyle.Fill,
                BackColor    = AppTheme.TabBackground,
                WrapContents = false,
                AutoScroll   = true,
                Margin       = new Padding(0),
                Padding      = new Padding(4, 0, 4, 0)
            };
            // thin bottom border on tab strip
            tabStrip.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(AppTheme.Border, 1),
                    0, tabStrip.Height - 1, tabStrip.Width, tabStrip.Height - 1);
            };

            // ── Content panel ──────────────────────────────────────────────────────
            var contentPanel = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = AppTheme.Background,
                Margin    = new Padding(0),
                Padding   = new Padding(0)
            };

            // ── Status bar ─────────────────────────────────────────────────────────
            StatusBarBuilder.BuildStatusBar(mainForm);

            // ── Menu + format toolbar ──────────────────────────────────────────────
            var mainMenu = MenuBuilder.BuildCombinedMenu(mainForm);
            mainMenu.Dock   = DockStyle.Fill;
            mainMenu.Margin = new Padding(0);
            
            var formatToolbar = FormatToolbarBuilder.BuildFormatToolbar(mainForm);
            formatToolbar.AutoSize = true;

            formatToolbar.Dock = DockStyle.Fill;

            // Thin separator line below toolbar
            var toolbarBorder = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = AppTheme.Border,
                Height    = 1
            };

            // ── Main layout ────────────────────────────────────────────────────────
            var layout = new TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                RowCount    = 5,
                ColumnCount = 1,
                Margin      = new Padding(0),
                BackColor   = AppTheme.Background
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));   // tab strip
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         // menu
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));         // format toolbar
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 1f));     // thin border
            layout.RowStyles.Add(new RowStyle(SizeType.Percent,  100f));   // content
            
            layout.Controls.Add(tabStrip,         0, 0);
            layout.Controls.Add(mainMenu,         0, 1);
            layout.Controls.Add(formatToolbar,    0, 2);
            layout.Controls.Add(toolbarBorder,    0, 3);
            layout.Controls.Add(contentPanel,     0, 4);
            
            mainForm.Controls.Add(layout);

            // statusStrip1 is already in designer at Dock.Bottom equivalent position
            // Bring it to front so it shows at the bottom
            mainForm.Controls.Add(mainForm.statusStrip1);
            mainForm.statusStrip1.BringToFront();

            return (tabStrip, contentPanel);
        }
    }
}
