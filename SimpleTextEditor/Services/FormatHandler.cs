using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace SimpleTextEditor.Services
{
    public class FormatHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // ── Notification service (injected after Form is shown) ───────────────────
        public NotificationService Notifier { get; set; }

        private void Notify(string message, NotifyKind kind)
        {
            Log.Info("[{0}] {1}", kind, message);
            Notifier?.Show(message, kind);
        }

        // ── Selection save/restore ─────────────────────────────────────────────────
        // Toolbar buttons steal focus. We save the selection BEFORE focus leaves the
        // editor (via EditorFocusLost) and restore it before applying any format.
        private RichTextBox _lastEditor;
        private int _savedStart;
        private int _savedLength;


        /// <summary>
        /// Call this from the editor's LostFocus event to remember the selection.
        /// </summary>
        public void SaveSelection(RichTextBox editor)
        {
            if (editor == null) return;
            _lastEditor   = editor;
            _savedStart   = editor.SelectionStart;
            _savedLength  = editor.SelectionLength;
        }

        /// <summary>
        /// Returns the editor with its selection restored. Returns null if no saved state.
        /// </summary>
        private RichTextBox RestoreSelection(RichTextBox editor)
        {
            if (editor == null) return null;
            editor.Focus();
            if (_savedLength > 0)
                editor.Select(_savedStart, _savedLength);
            return editor;
        }

        // ── Public format methods (all restore selection first) ────────────────────

        public void ToggleBold(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font cur = editor.SelectionFont ?? editor.Font;
            bool willBeBold = !cur.Bold;
            ToggleStyle(editor, FontStyle.Bold);
            Notify(willBeBold ? "Жирний увімкнено" : "Жирний вимкнено",
                   willBeBold ? NotifyKind.FormatOn : NotifyKind.FormatOff);
        }

        public void ToggleItalic(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font cur = editor.SelectionFont ?? editor.Font;
            bool willBeItalic = !cur.Italic;
            ToggleStyle(editor, FontStyle.Italic);
            Notify(willBeItalic ? "Курсив увімкнено" : "Курсив вимкнено",
                   willBeItalic ? NotifyKind.FormatOn : NotifyKind.FormatOff);
        }

        public void ToggleUnderline(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font cur = editor.SelectionFont ?? editor.Font;
            bool willBeUnderline = !cur.Underline;
            ToggleStyle(editor, FontStyle.Underline);
            Notify(willBeUnderline ? "Підкреслення увімкнено" : "Підкреслення вимкнено",
                   willBeUnderline ? NotifyKind.FormatOn : NotifyKind.FormatOff);
        }

        public void ToggleStrike(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font cur = editor.SelectionFont ?? editor.Font;
            bool willBeStrike = !cur.Strikeout;
            ToggleStyle(editor, FontStyle.Strikeout);
            Notify(willBeStrike ? "Закреслений увімкнено" : "Закреслений вимкнено",
                   willBeStrike ? NotifyKind.FormatOn : NotifyKind.FormatOff);
        }

        private void ToggleStyle(RichTextBox editor, FontStyle style)
        {
            if (editor == null) return;
            Font currentFont = editor.SelectionFont ?? editor.Font;
            FontStyle newStyle = currentFont.Style ^ style;
            editor.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
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
                    RestoreSelection(editor);
                    editor.SelectionColor = colorDialog.Color;
                }
            }
        }

        public void InsertHeading(RichTextBox editor, int level)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;

            float[] sizes = { 26f, 22f, 18f, 16f, 14f, 12f };
            float targetSize = sizes[Math.Min(level - 1, 5)];

            // If nothing is selected, select the current line (like MS Word)
            if (editor.SelectionLength == 0)
            {
                int lineIdx   = editor.GetLineFromCharIndex(editor.SelectionStart);
                int lineStart = editor.GetFirstCharIndexFromLine(lineIdx);
                string lineText = lineIdx < editor.Lines.Length ? editor.Lines[lineIdx] : "";
                editor.Select(lineStart, lineText.Length);
            }

            Font currentFont = editor.SelectionFont ?? editor.Font;
            editor.SelectionFont = new Font(currentFont.FontFamily, targetSize, FontStyle.Bold);

            Notify($"Заголовок H{level} ({targetSize:0}pt)", NotifyKind.Heading);

            // Move cursor to end of heading
            int end = editor.SelectionStart + editor.SelectionLength;
            editor.Select(end, 0);
            editor.SelectionFont = editor.Font;
        }

        public void ToggleBulletList(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;

            bool currentBullet = editor.SelectionBullet;
            bool willEnable    = !currentBullet;
            editor.SelectionBullet = willEnable;

            Notify(willEnable ? "Маркований список додано" : "Маркований список прибрано",
                   willEnable ? NotifyKind.FormatOn : NotifyKind.FormatOff);
        }

        public void InsertQuote(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            editor.SelectionIndent = 20;
            Font currentFont = editor.SelectionFont ?? editor.Font;
            editor.SelectionFont = new Font(currentFont, FontStyle.Italic);
            editor.SelectionColor = Color.Gray;
            Notify("Цитату додано", NotifyKind.Insert);
        }

        public void InsertCodeBlock(RichTextBox editor)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font currentFont = editor.SelectionFont ?? editor.Font;
            editor.SelectionFont = new Font("Consolas", currentFont.Size, FontStyle.Regular);
            editor.SelectionBackColor = Color.FromArgb(40, 40, 40);
            Notify("Блок коду додано", NotifyKind.Insert);
        }

        public void InsertLink(RichTextBox editor, string text, string url)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;

            if (string.IsNullOrEmpty(text)) text = url;

            editor.SelectionColor = Color.FromArgb(167, 139, 250);
            Font currentFont = editor.SelectionFont ?? editor.Font;
            editor.SelectionFont = new Font(currentFont, FontStyle.Underline);
            editor.SelectedText = text;

            // reset
            editor.SelectionColor = editor.ForeColor;
            editor.SelectionFont = editor.Font;
            editor.SelectedText = " ";
        }

        public void InsertImage(RichTextBox editor, string imagePath, string altText)
        {
            editor = RestoreSelection(editor);
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
                    editor.SelectedText = $"[Зображення: {altText} - {imagePath}]";
                }
            }
            catch
            {
                editor.SelectedText = $"[Зображення: {altText}]";
            }
        }

        public void ApplyFont(RichTextBox editor, string fontFamily, float size)
        {
            editor = RestoreSelection(editor);
            if (editor == null) return;
            Font currentFont = editor.SelectionFont ?? editor.Font;
            float targetSize = size > 0 ? size : currentFont.Size;
            string targetFamily = !string.IsNullOrEmpty(fontFamily) ? fontFamily : currentFont.FontFamily.Name;
            editor.SelectionFont = new Font(targetFamily, targetSize, currentFont.Style);
        }
    }
}
