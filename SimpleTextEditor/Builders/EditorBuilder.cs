using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class EditorBuilder
    {
        public static (Panel container, RichTextBox rtb) BuildEditor(string content)
        {
            var container = new Panel { Dock = DockStyle.Fill };

            var linePanel = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 44,
                BackColor = AppTheme.Background,
                ForeColor = AppTheme.TextMuted
            };

            var rtb = new RichTextBox
            {
                Dock        = DockStyle.Fill,
                BackColor   = AppTheme.Background,
                ForeColor   = AppTheme.TextEditor,
                Font        = new Font("Cascadia Code", 12f, FontStyle.Regular),
                BorderStyle = BorderStyle.None,
                Text        = content,
                WordWrap    = true,
                HideSelection = false,
                SelectionColor = AppTheme.TextEditor,
                ScrollBars  = RichTextBoxScrollBars.None
            };
            rtb.HandleCreated += (s, e) => WindowsTheme.ApplyDarkThemeToScrollbars(rtb.Handle);

            // Fallback font if Cascadia Code not installed
            try { var _ = new Font("Cascadia Code", 12f); }
            catch { rtb.Font = new Font("Consolas", 12f); }

            // ── Bullet list continuation on Enter ────────────────────────────────
            rtb.KeyDown += (s, e) => HandleEditorKeyDown(rtb, e);

            // ── Line number rendering ────────────────────────────────────────────
            linePanel.Paint += (s, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int firstIdx  = rtb.GetCharIndexFromPosition(new Point(0, 0));
                int firstLine = rtb.GetLineFromCharIndex(firstIdx);

                Point pt      = new Point(0, linePanel.Height);
                int lastIdx   = rtb.GetCharIndexFromPosition(pt);
                int lastLine  = rtb.GetLineFromCharIndex(lastIdx);

                using var font  = new Font(rtb.Font.FontFamily, Math.Max(1f, rtb.Font.Size - 2f));
                using var brush = new SolidBrush(AppTheme.TextMuted);
                using var activeBrush = new SolidBrush(AppTheme.AccentLight);

                int currentLine = rtb.GetLineFromCharIndex(rtb.SelectionStart);

                for (int i = firstLine; i <= lastLine + 1; i++)
                {
                    if (i >= rtb.Lines.Length) break;
                    int charIndex = rtb.GetFirstCharIndexFromLine(i);
                    if (charIndex == -1) break;

                    Point pos = rtb.GetPositionFromCharIndex(charIndex);
                    bool isCurrent = (i == currentLine);

                    var b = isCurrent ? activeBrush : brush;
                    var sf = new StringFormat { Alignment = StringAlignment.Far };
                    e.Graphics.DrawString((i + 1).ToString(), font, b,
                        new RectangleF(0, pos.Y, linePanel.Width - 6, rtb.Font.Height), sf);
                }
            };

            rtb.VScroll          += (s, e) => linePanel.Invalidate();
            rtb.TextChanged      += (s, e) => linePanel.Invalidate();
            rtb.FontChanged      += (s, e) => linePanel.Invalidate();
            rtb.Resize           += (s, e) => linePanel.Invalidate();
            rtb.SelectionChanged += (s, e) => linePanel.Invalidate();

            container.Controls.Add(linePanel);
            container.Controls.Add(rtb);
            rtb.BringToFront();

            return (container, rtb);
        }

        /// <summary>
        /// Handles special key behaviour inside the editor:
        /// - Enter in a bullet-list paragraph → continues the bullet on the next line.
        /// - Enter in an empty bullet line    → turns off bullet (exits the list).
        /// </summary>
        private static void HandleEditorKeyDown(RichTextBox rtb, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Shift || e.Control || e.Alt)
                return;

            // Only act when we're in a bulleted paragraph
            if (!rtb.SelectionBullet)
                return;

            // Check whether the CURRENT line is empty (user wants to exit the list)
            int lineIdx = rtb.GetLineFromCharIndex(rtb.SelectionStart);
            string currentLineText = lineIdx < rtb.Lines.Length ? rtb.Lines[lineIdx] : "";

            if (string.IsNullOrWhiteSpace(currentLineText))
            {
                // Exit bullet list: turn off bullet and let the normal Enter do its work
                rtb.SelectionBullet = false;
                rtb.SelectionIndent = 0;
                // Don't suppress — we still want the newline
                return;
            }

            // Continue the list: insert newline and keep bullet formatting
            e.SuppressKeyPress = true; // prevent default Enter

            int caretPos = rtb.SelectionStart;
            rtb.Select(caretPos, rtb.SelectionLength);
            rtb.SelectedText = "\n";

            // The new line inherits bullet automatically in RichTextBox,
            // but we make sure the font is reset to the base font (not heading size)
            Font baseFont = rtb.Font;
            // Only reset if the current selection font differs greatly (was a heading)
            Font curFont = rtb.SelectionFont;
            if (curFont != null && curFont.Size > baseFont.Size + 2f)
                rtb.SelectionFont = new Font(baseFont.FontFamily, baseFont.Size, FontStyle.Regular);
        }
    }
}
