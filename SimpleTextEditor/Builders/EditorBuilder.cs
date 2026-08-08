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
                SelectionColor = AppTheme.TextEditor,
                ScrollBars  = RichTextBoxScrollBars.None
            };
            rtb.HandleCreated += (s, e) => WindowsTheme.ApplyDarkThemeToScrollbars(rtb.Handle);

            // Fallback font if Cascadia Code not installed
            try { var _ = new Font("Cascadia Code", 12f); }
            catch { rtb.Font = new Font("Consolas", 12f); }

            // ── Line number rendering ────────────────────────────────────────────
            linePanel.Paint += (s, e) =>
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int firstIdx  = rtb.GetCharIndexFromPosition(new Point(0, 0));
                int firstLine = rtb.GetLineFromCharIndex(firstIdx);

                Point pt      = new Point(0, linePanel.Height);
                int lastIdx   = rtb.GetCharIndexFromPosition(pt);
                int lastLine  = rtb.GetLineFromCharIndex(lastIdx);

                using var font  = new Font(rtb.Font.FontFamily, rtb.Font.Size - 2f);
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
    }
}
