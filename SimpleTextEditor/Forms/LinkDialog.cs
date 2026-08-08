using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public class LinkDialog : Form
    {
        public string LinkText { get; private set; }
        public string LinkUrl { get; private set; }

        private TextBox _txtText;
        private TextBox _txtUrl;
        private Button _btnOk;
        private Button _btnCancel;

        public LinkDialog(string defaultText = "")
        {
            this.Text = "Вставити посилання";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = AppTheme.Surface;
            this.ForeColor = AppTheme.TextPrimary;

            var lblText = new Label { Text = "Текст для відображення", AutoSize = true, Location = new Point(20, 20), ForeColor = AppTheme.TextPrimary };
            _txtText = new TextBox { Text = defaultText, Location = new Point(20, 45), Width = 340, BackColor = AppTheme.SurfaceHigh, ForeColor = AppTheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };

            var lblUrl = new Label { Text = "Адреса (URL)", AutoSize = true, Location = new Point(20, 80), ForeColor = AppTheme.TextPrimary };
            _txtUrl = new TextBox { Location = new Point(20, 105), Width = 340, BackColor = AppTheme.SurfaceHigh, ForeColor = AppTheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };

            _btnOk = new Button
            {
                Text = "Вставити",
                DialogResult = DialogResult.OK,
                Location = new Point(180, 160),
                Width = 85,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppTheme.Accent,
                ForeColor = AppTheme.TextPrimary
            };
            _btnOk.FlatAppearance.BorderSize = 0;
            _btnOk.Click += (s, e) =>
            {
                LinkText = _txtText.Text;
                LinkUrl = _txtUrl.Text;
            };

            _btnCancel = new Button
            {
                Text = "Скасувати",
                DialogResult = DialogResult.Cancel,
                Location = new Point(275, 160),
                Width = 85,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppTheme.SurfaceHigh,
                ForeColor = AppTheme.TextPrimary
            };
            _btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.Add(lblText);
            this.Controls.Add(_txtText);
            this.Controls.Add(lblUrl);
            this.Controls.Add(_txtUrl);
            this.Controls.Add(_btnOk);
            this.Controls.Add(_btnCancel);
            
            this.AcceptButton = _btnOk;
            this.CancelButton = _btnCancel;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WindowsTheme.EnableDarkMode(this.Handle);
        }
    }
}
