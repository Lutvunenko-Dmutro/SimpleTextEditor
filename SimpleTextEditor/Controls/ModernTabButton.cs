using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI.Controls
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  ModernTabButton  –  a single tab pill in the tab strip
    // ─────────────────────────────────────────────────────────────────────────────
    internal class ModernTabButton : Control
    {
        private bool _isActive;
        private bool _isDirty;       // unsaved changes indicator
        private bool _closeHovered;
        private string _title;
        private readonly Rectangle _closeRect = new Rectangle(0, 0, 18, 18);

        public event EventHandler CloseClicked;
        public event EventHandler TabClicked;

        public ModernTabButton(string title)
        {
            _title = title;
            DoubleBuffered = true;
            Size = new Size(160, 36);
            Cursor = Cursors.Hand;
            Margin = new Padding(2, 4, 0, 0);
        }

        public string Title
        {
            get => _title;
            set { _title = value; Invalidate(); }
        }

        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; Invalidate(); }
        }

        public bool IsDirty
        {
            get => _isDirty;
            set { _isDirty = value; Invalidate(); }
        }

        private Rectangle CloseHitArea =>
            new Rectangle(Width - 24, (Height - 18) / 2, 18, 18);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool wasHovered = _closeHovered;
            _closeHovered = CloseHitArea.Contains(e.Location);
            if (wasHovered != _closeHovered) Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _closeHovered = false;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (CloseHitArea.Contains(e.Location))
                CloseClicked?.Invoke(this, EventArgs.Empty);
            else
                TabClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var bg = _isActive ? AppTheme.TabActive : AppTheme.TabInactive;
            g.Clear(bg);

            // ── Active tab: top accent bar + slightly brighter fill ──────────────
            if (_isActive)
            {
                using var fill = new SolidBrush(AppTheme.SurfaceHigh);
                g.FillRectangle(fill, 0, 0, Width, Height);

                using var accentPen = new Pen(AppTheme.Accent, 2f);
                g.DrawLine(accentPen, 2, 0, Width - 2, 0);
            }

            // ── Dirty dot (unsaved) ──────────────────────────────────────────────
            Color textColor;
            if (_isDirty)
            {
                textColor = AppTheme.Warning;
                using var dot = new SolidBrush(AppTheme.Warning);
                g.FillEllipse(dot, 8, Height / 2 - 4, 8, 8);
            }
            else
            {
                textColor = _isActive ? AppTheme.TextPrimary : AppTheme.TextMuted;
            }

            // ── Tab title ────────────────────────────────────────────────────────
            int dotOffset = _isDirty ? 12 : 0;
            using var titleFont = new Font("Segoe UI", 9f, _isActive ? FontStyle.Regular : FontStyle.Regular);
            var textRect = new Rectangle(12 + dotOffset, 0, Width - 38 - dotOffset, Height);
            var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
            using var brush = new SolidBrush(textColor);
            g.DrawString(_title, titleFont, brush, textRect, sf);

            // ── Close button ─────────────────────────────────────────────────────
            var cr = CloseHitArea;
            if (_closeHovered)
            {
                using var circleBrush = new SolidBrush(AppTheme.Error);
                g.FillEllipse(circleBrush, cr);
                using var xPen = new Pen(Color.White, 1.5f);
                int pad = 4;
                g.DrawLine(xPen, cr.Left + pad, cr.Top + pad, cr.Right - pad, cr.Bottom - pad);
                g.DrawLine(xPen, cr.Right - pad, cr.Top + pad, cr.Left + pad, cr.Bottom - pad);
            }
            else
            {
                using var xPen = new Pen(_isActive ? AppTheme.TextMuted : Color.FromArgb(60, 100, 97, 145), 1.5f);
                int pad = 5;
                g.DrawLine(xPen, cr.Left + pad, cr.Top + pad, cr.Right - pad, cr.Bottom - pad);
                g.DrawLine(xPen, cr.Right - pad, cr.Top + pad, cr.Left + pad, cr.Bottom - pad);
            }
        }
    }
}
