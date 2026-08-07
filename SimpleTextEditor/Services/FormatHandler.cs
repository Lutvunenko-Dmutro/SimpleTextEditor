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

            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = editor.SelectionFont ?? editor.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    editor.SelectionFont = fontDialog.Font;
                }
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
    }
}
