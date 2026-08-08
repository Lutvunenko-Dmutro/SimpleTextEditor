using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public partial class SettingsForm : Form
    {
        private RichTextBox _editor;
        private Label lblPreview;
        private SimpleTextEditor.Controls.DarkComboBox cbFamily, cbStyle, cbSize;

        public SettingsForm(RichTextBox editor)
        {
            _editor = editor;
            InitializeComponent();
            
            PopulateFonts();
            
            cbFamily.SelectedIndexChanged += UpdateFont;
            cbStyle.SelectedIndexChanged  += UpdateFont;
            cbSize.SelectedIndexChanged   += UpdateFont;
        }

        private void PopulateFonts()
        {
            foreach (FontFamily ff in FontFamily.Families)
            {
                if (ff.IsStyleAvailable(FontStyle.Regular) || ff.IsStyleAvailable(FontStyle.Bold) || ff.IsStyleAvailable(FontStyle.Italic))
                {
                    cbFamily.Items.Add(ff.Name);
                }
            }

            cbStyle.Items.AddRange(new[] { "Regular", "Bold", "Italic", "Bold Italic" });
            cbSize.Items.AddRange(new[] { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" });

            if (_editor != null && _editor.SelectionFont != null)
            {
                Font f = _editor.SelectionFont;
                cbFamily.SelectedItem = f.FontFamily.Name;
                cbSize.SelectedItem = ((int)f.Size).ToString();
                
                string st = "Regular";
                if (f.Bold && f.Italic) st = "Bold Italic";
                else if (f.Bold) st = "Bold";
                else if (f.Italic) st = "Italic";
                cbStyle.SelectedItem = st;
            }
            else if (_editor != null && _editor.Font != null)
            {
                Font f = _editor.Font;
                cbFamily.SelectedItem = f.FontFamily.Name;
                cbSize.SelectedItem = ((int)f.Size).ToString();
                
                string st = "Regular";
                if (f.Bold && f.Italic) st = "Bold Italic";
                else if (f.Bold) st = "Bold";
                else if (f.Italic) st = "Italic";
                cbStyle.SelectedItem = st;
            }
            else
            {
                cbFamily.SelectedItem = "Consolas";
                cbStyle.SelectedItem = "Regular";
                cbSize.SelectedItem = "11";
            }
            
            if (cbFamily.SelectedIndex == -1) cbFamily.SelectedIndex = 0;
            if (cbSize.SelectedIndex == -1) cbSize.SelectedItem = "11";
        }

        private void UpdateFont(object sender, EventArgs e)
        {
            if (cbFamily.SelectedItem == null || cbStyle.SelectedItem == null || cbSize.SelectedItem == null) return;

            string fam = cbFamily.SelectedItem.ToString();
            string st = cbStyle.SelectedItem.ToString();
            float sz = float.Parse(cbSize.SelectedItem.ToString());

            FontStyle fs = FontStyle.Regular;
            if (st.Contains("Bold")) fs |= FontStyle.Bold;
            if (st.Contains("Italic")) fs |= FontStyle.Italic;

            try
            {
                Font newFont = new Font(fam, sz, fs);
                lblPreview.Font = newFont;

                if (_editor != null)
                {
                    if (_editor.SelectionLength > 0)
                        _editor.SelectionFont = newFont;
                    else
                        _editor.Font = newFont;
                }
            }
            catch { }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsTheme.ApplyDarkThemeToAllControls(this);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            WindowsTheme.EnableDarkMode(this.Handle);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED (fixes flickering)
                return cp;
            }
        }
    }
}
