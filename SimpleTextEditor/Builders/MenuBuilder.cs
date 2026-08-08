using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static partial class MenuBuilder
    {
        private static readonly Font MenuFont = new Font("Segoe UI", 10f);

        public static MenuStrip BuildCombinedMenu(Form1 mainForm)
        {
            var mainMenu = new MenuStrip
            {
                BackColor = AppTheme.SurfaceHigh,
                ForeColor = AppTheme.TextPrimary,
                Renderer  = new DarkToolStripRenderer(),
                Padding   = new Padding(8, 4, 8, 4),
                Font      = MenuFont
            };

            var fileMenu = BuildFileMenu(mainForm);
            var editMenu = BuildEditMenu(mainForm);
            var viewMenu = BuildViewMenu(mainForm);

            foreach (ToolStripMenuItem m in new[] { fileMenu, editMenu, viewMenu })
                StyleMenuRecursive(m);

            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, viewMenu });
            return mainMenu;
        }

        private static ToolStripMenuItem Menu(string text, Image image = null)
        {
            return new ToolStripMenuItem(text, image)
            {
                Font         = new Font("Segoe UI", 10f),
                ForeColor    = AppTheme.TextPrimary,
                ImageScaling = ToolStripItemImageScaling.None
            };
        }

        private static ToolStripMenuItem Item(string text, string shortcutText,
            System.EventHandler onClick, Keys keys = Keys.None, Image image = null)
        {
            var item = new ToolStripMenuItem(text, image, onClick)
            {
                ShortcutKeyDisplayString = shortcutText,
                Font         = new Font("Segoe UI", 9.5f),
                BackColor    = AppTheme.Surface,
                ForeColor    = AppTheme.TextPrimary,
                ImageScaling = ToolStripItemImageScaling.None
            };
            if (keys != Keys.None) item.ShortcutKeys = keys;
            return item;
        }

        private static ToolStripMenuItem CheckItem(string text, bool isChecked,
            System.EventHandler onClick)
        {
            var item = new ToolStripMenuItem(text, null, onClick)
            {
                Checked   = isChecked,
                Font      = new Font("Segoe UI", 9.5f),
                BackColor = AppTheme.Surface,
                ForeColor = AppTheme.TextPrimary
            };
            item.Click += (s, e) => item.Checked = !item.Checked;
            return item;
        }

        private static ToolStripSeparator Sep() => new ToolStripSeparator();

        private static void StyleMenuRecursive(ToolStripDropDownItem menu)
        {
            foreach (ToolStripItem item in menu.DropDownItems)
            {
                item.BackColor = AppTheme.Surface;
                item.ForeColor = AppTheme.TextPrimary;
                if (item is ToolStripMenuItem sub) StyleMenuRecursive(sub);
            }
        }
    }
}
