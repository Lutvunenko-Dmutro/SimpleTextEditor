using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            DoubleBuffered  = true;
            Text            = "Про програму";
            ClientSize      = new Size(460, 300);
            FormBorderStyle = FormBorderStyle.None;   // borderless – we draw custom frame
            StartPosition   = FormStartPosition.Manual; // we position it ourselves
            BackColor       = AppTheme.Background;
            ForeColor       = AppTheme.TextPrimary;

            Theme.WindowsTheme.EnableDarkMode(Handle);

            BuildUI();

            // Center on parent when shown
            this.Shown += (s, e) =>
            {
                if (Owner != null)
                {
                    Left = Owner.Left + (Owner.Width  - Width)  / 2;
                    Top  = Owner.Top  + (Owner.Height - Height) / 2;
                }
                else
                {
                    // fallback: center on screen
                    var scr = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position).WorkingArea;
                    Left = scr.Left + (scr.Width  - Width)  / 2;
                    Top  = scr.Top  + (scr.Height - Height) / 2;
                }
            };

            // Close on Escape
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.WindowsTheme.EnableDarkMode(Handle);
        }
    }
}
