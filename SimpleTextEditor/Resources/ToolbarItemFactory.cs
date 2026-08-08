using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class ToolbarItemFactory
    {
        public static ToolStripButton MakeBtn(string text, Font font, string tip, EventHandler click, Color? color = null)
        {
            return new ToolStripButton(text, null, click)
            {
                Font         = font,
                ForeColor    = color ?? AppTheme.TextPrimary,
                Padding      = new Padding(4, 1, 4, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                ToolTipText  = tip
            };
        }

        public static ToolStripButton MakeIconBtn(Image image, string tip, EventHandler click)
        {
            return new ToolStripButton(null, image, click)
            {
                Padding      = new Padding(4, 1, 4, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ToolTipText  = tip,
                ImageScaling = ToolStripItemImageScaling.None
            };
        }

        public static void StyleDropDown(ToolStripDropDownButton dd)
        {
            foreach (ToolStripItem item in dd.DropDownItems)
            {
                item.BackColor = AppTheme.Surface;
                item.ForeColor = AppTheme.TextPrimary;
                if (item is ToolStripMenuItem mi)
                {
                    mi.MouseEnter += (s, e) => { mi.BackColor = AppTheme.Accent; };
                    mi.MouseLeave += (s, e) => { mi.BackColor = AppTheme.Surface; };
                }
            }
        }

        public static ToolStripMenuItem DdItem(string text, Action action, Image image = null)
        {
            return new ToolStripMenuItem(text, image, (s, e) => action())
            {
                BackColor = AppTheme.Surface,
                ForeColor = AppTheme.TextPrimary,
                ImageScaling = ToolStripItemImageScaling.None
            };
        }

        public static void InsertText(Form1 mainForm, string prefix, string suffix = "")
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb != null)
                rtb.SelectedText = prefix + rtb.SelectedText + suffix;
        }
    }
}
