using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.Controls
{
    public class DarkComboBox : ComboBox
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [StructLayout(LayoutKind.Sequential)]
        struct COMBOBOXINFO
        {
            public int cbSize;
            public Rectangle rcItem;
            public Rectangle rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [DllImport("user32.dll")]
        private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref COMBOBOXINFO pcbi);

        public DarkComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = AppTheme.SurfaceHigh;
            this.ForeColor = AppTheme.TextPrimary;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetWindowTheme(this.Handle, "DarkMode_CFD", null);
            this.Region = new Region(new Rectangle(1, 1, this.Width - 2, this.Height - 2));
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            COMBOBOXINFO info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf(info);
            if (GetComboBoxInfo(this.Handle, ref info))
            {
                SetWindowTheme(info.hwndList, "DarkMode_Explorer", null);
            }
        }

        private const int WM_PAINT = 0x000F;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                base.WndProc(ref m);
                using (var g = Graphics.FromHwnd(this.Handle))
                {
                    // Paint over the white button part
                    int btnWidth = SystemInformation.VerticalScrollBarWidth;
                    Rectangle btnRect = new Rectangle(this.Width - btnWidth, 0, btnWidth, this.Height);
                    g.FillRectangle(new SolidBrush(AppTheme.SurfaceHigh), btnRect);
                    
                    int arrowX = btnRect.X + (btnRect.Width / 2) - 4;
                    int arrowY = btnRect.Y + (btnRect.Height / 2) - 1;
                    Point[] arrow = new Point[] 
                    { 
                        new Point(arrowX, arrowY), 
                        new Point(arrowX + 7, arrowY), 
                        new Point(arrowX + 3, arrowY + 4) 
                    };
                    g.FillPolygon(new SolidBrush(AppTheme.TextSecondary), arrow);
                }
                return;
            }
            base.WndProc(ref m);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var g = e.Graphics;
            var itemText = this.Items[e.Index].ToString();

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                g.FillRectangle(new SolidBrush(AppTheme.Accent), e.Bounds);
            }
            else
            {
                g.FillRectangle(new SolidBrush(AppTheme.SurfaceHigh), e.Bounds);
            }

            TextRenderer.DrawText(g, itemText, this.Font, e.Bounds, AppTheme.TextPrimary, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }
    }
}
