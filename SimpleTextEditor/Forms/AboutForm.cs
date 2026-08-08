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
            DoubleBuffered = true;
            Text           = "Про програму";
            ClientSize     = new Size(460, 300);
            FormBorderStyle = FormBorderStyle.None;   // borderless – we draw custom frame
            StartPosition  = FormStartPosition.CenterParent;
            BackColor      = AppTheme.Background;
            ForeColor      = AppTheme.TextPrimary;

            Theme.WindowsTheme.EnableDarkMode(Handle);

            BuildUI();

            // Close on Escape or click outside
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };
            this.MouseClick += (s, e) => { /* click on backdrop = close */ };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.WindowsTheme.EnableDarkMode(Handle);
        }
    }
}
