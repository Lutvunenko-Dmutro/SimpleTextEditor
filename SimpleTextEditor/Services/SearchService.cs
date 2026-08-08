using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.Services
{
    public static class SearchService
    {
        public static void DoFind(Form1 mainForm, string query, bool reverse)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Contains("Пошук")) return;
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb == null) return;

            int start = rtb.SelectionStart + rtb.SelectionLength;
            var flags = reverse ? RichTextBoxFinds.Reverse : RichTextBoxFinds.None;
            int idx = rtb.Find(query, start, flags);
            if (idx == -1) idx = rtb.Find(query, 0, flags); // wrap
            if (idx != -1)
            {
                rtb.Select(idx, query.Length);
                rtb.ScrollToCaret();
                rtb.Focus();
            }
            else
            {
                MessageBox.Show("Текст не знайдено.", "Пошук",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void DoReplace(Form1 mainForm, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            // Simple replace dialog using Form
            using var dlg = new Form
            {
                Text          = "Замінити",
                Size          = new Size(340, 140),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox   = false,
                MinimizeBox   = false,
                BackColor     = AppTheme.Surface,
                ForeColor     = AppTheme.TextPrimary
            };

            var lbl = new Label { Text = $"Замінити «{query}» на:", Location = new Point(12, 14), AutoSize = true, ForeColor = AppTheme.TextSecondary };
            var txt = new TextBox { Location = new Point(12, 36), Size = new Size(300, 24), BackColor = AppTheme.Background, ForeColor = AppTheme.TextEditor, BorderStyle = BorderStyle.FixedSingle };
            var btn = new Button  { Text = "Замінити", Location = new Point(12, 68), Size = new Size(100, 30), BackColor = AppTheme.Accent, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK };
            btn.FlatAppearance.BorderSize = 0;
            dlg.Controls.AddRange(new Control[] { lbl, txt, btn });
            dlg.AcceptButton = btn;

            if (dlg.ShowDialog(mainForm) == DialogResult.OK)
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                string newText = rtb.Text.Replace(query, txt.Text);
                if (newText != rtb.Text) rtb.Text = newText;
            }
        }
    }
}
