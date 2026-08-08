using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleTextEditor.Services
{
    public class FormatHandler
    {
        public void ToggleBold(RichTextBox editor) => ToggleStyle(editor, FontStyle.Bold);
        public void ToggleItalic(RichTextBox editor) => ToggleStyle(editor, FontStyle.Italic);
        public void ToggleUnderline(RichTextBox editor) => ToggleStyle(editor, FontStyle.Underline);
        public void ToggleStrike(RichTextBox editor) => ToggleStyle(editor, FontStyle.Strikeout);

        private void ToggleStyle(RichTextBox editor, FontStyle style)
        {
            if (editor == null) return;

            Font currentFont = editor.SelectionFont;
            if (currentFont != null)
            {
                FontStyle newStyle = currentFont.Style ^ style;
                editor.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
        }

        public void ChangeFont(RichTextBox editor)
        {
            if (editor == null) return;

            using (var settingsForm = new UI.SettingsForm(editor))
            {
                settingsForm.ShowDialog();
            }
        }

        public void ChangeColor(RichTextBox editor)
        {
            if (editor == null) return;

            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    editor.SelectionColor = colorDialog.Color;
                }
            }
        }

        public void InsertHeading(RichTextBox editor, int level)
        {
            if (editor == null) return;
            float[] sizes = { 26f, 22f, 18f, 16f, 14f, 12f };
            float targetSize = sizes[Math.Min(level - 1, 5)];
            editor.SelectionFont = new Font(editor.Font.FontFamily, targetSize, FontStyle.Bold);
        }

        public void ToggleBulletList(RichTextBox editor)
        {
            if (editor == null) return;
            editor.SelectionBullet = !editor.SelectionBullet;
        }

        public void InsertQuote(RichTextBox editor)
        {
            if (editor == null) return;
            editor.SelectionIndent = 20;
            editor.SelectionFont = new Font(editor.Font, FontStyle.Italic);
            editor.SelectionColor = Color.Gray; 
        }

        public void InsertCodeBlock(RichTextBox editor)
        {
            if (editor == null) return;
            editor.SelectionFont = new Font("Consolas", editor.Font.Size, FontStyle.Regular);
            editor.SelectionBackColor = Color.FromArgb(40, 40, 40);
        }

        public void InsertLink(RichTextBox editor, string text, string url)
        {
            if (editor == null) return;
            
            if (string.IsNullOrEmpty(text)) text = url;

            editor.SelectionColor = Color.FromArgb(167, 139, 250); // Accent color
            editor.SelectionFont = new Font(editor.SelectionFont, FontStyle.Underline);
            editor.SelectedText = text;
            
            // reset
            editor.SelectionColor = editor.ForeColor;
            editor.SelectionFont = new Font(editor.SelectionFont, FontStyle.Regular);
            editor.SelectedText = " ";
        }

        public void InsertImage(RichTextBox editor, string imagePath, string altText)
        {
            if (editor == null) return;
            
            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    using (Image img = Image.FromFile(imagePath))
                    {
                        var orgData = Clipboard.GetDataObject();
                        Clipboard.SetImage(img);
                        editor.Paste();
                        if (orgData != null) Clipboard.SetDataObject(orgData);
                    }
                }
                else
                {
                    // Fallback to text if URL or file not found
                    editor.SelectedText = $"[Зображення: {altText} - {imagePath}]";
                }
            }
            catch 
            {
                editor.SelectedText = $"[Зображення: {altText}]";
            }
        }
    }
}
