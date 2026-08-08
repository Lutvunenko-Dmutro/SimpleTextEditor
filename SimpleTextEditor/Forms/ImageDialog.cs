using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public class ImageDialog : Form
    {
        public string ImageUrl { get; private set; }
        public string AltText { get; private set; }
        public bool IsLocalFile { get; private set; }

        private TextBox _txtAlt;
        private TextBox _txtUrl;
        private Button _btnBrowse;
        private Button _btnOk;
        private Button _btnCancel;

        public ImageDialog()
        {
            this.Text = "Вставити зображення";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = AppTheme.Surface;
            this.ForeColor = AppTheme.TextPrimary;

            var lblAlt = new Label { Text = "Текст заміни (alt text)", AutoSize = true, Location = new Point(20, 20), ForeColor = AppTheme.TextPrimary };
            _txtAlt = new TextBox { Location = new Point(20, 45), Width = 340, BackColor = AppTheme.SurfaceHigh, ForeColor = AppTheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };

            var lblUrl = new Label { Text = "Адреса або шлях до файлу", AutoSize = true, Location = new Point(20, 80), ForeColor = AppTheme.TextPrimary };
            _txtUrl = new TextBox { Location = new Point(20, 105), Width = 250, BackColor = AppTheme.SurfaceHigh, ForeColor = AppTheme.TextPrimary, BorderStyle = BorderStyle.FixedSingle };

            _btnBrowse = new Button
            {
                Text = "Огляд...",
                Location = new Point(280, 104),
                Width = 80,
                Height = 24,
                FlatStyle = FlatStyle.Flat,
                BackColor = AppTheme.SurfaceHigh,
                ForeColor = AppTheme.TextPrimary
            };
            _btnBrowse.FlatAppearance.BorderSize = 1;
            _btnBrowse.FlatAppearance.BorderColor = AppTheme.Border;
            _btnBrowse.Click += BtnBrowse_Click;

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
                AltText = _txtAlt.Text;
                ImageUrl = _txtUrl.Text;
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

            this.Controls.Add(lblAlt);
            this.Controls.Add(_txtAlt);
            this.Controls.Add(lblUrl);
            this.Controls.Add(_txtUrl);
            this.Controls.Add(_btnBrowse);
            this.Controls.Add(_btnOk);
            this.Controls.Add(_btnCancel);
            
            this.AcceptButton = _btnOk;
            this.CancelButton = _btnCancel;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Зображення|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Всі файли|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _txtUrl.Text = ofd.FileName;
                    IsLocalFile = true;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WindowsTheme.EnableDarkMode(this.Handle);
        }
    }
}
