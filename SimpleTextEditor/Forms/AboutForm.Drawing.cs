using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor
{
    public partial class AboutForm
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyRoundedCorners(16);
        }

        private void ApplyRoundedCorners(int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(Width - d, 0, d, d, 270, 90);
            path.AddArc(Width - d, Height - d, d, d, 0, 90);
            path.AddArc(0, Height - d, d, d, 90, 90);
            path.CloseFigure();
            Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Gradient fill
            using var bg = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(20, 18, 36),
                Color.FromArgb(13, 13, 18),
                LinearGradientMode.Vertical);
            g.FillRectangle(bg, ClientRectangle);

            // Violet glow orb at top-right
            using var glow = new PathGradientBrush(new PointF[]
            {
                new PointF(Width, 0), new PointF(Width - 200, 0),
                new PointF(Width, 200)
            })
            {
                CenterColor    = Color.FromArgb(80, 124, 77, 255),
                SurroundColors = new[] { Color.Transparent }
            };
            g.FillRectangle(glow, Width - 200, 0, 200, 200);

            // Top accent line
            using var accentPen = new Pen(AppTheme.Accent, 2f);
            g.DrawLine(accentPen, 24, 1, Width - 24, 1);

            // Border
            using var borderPen = new Pen(AppTheme.Border, 1f);
            RoundedRect(g, borderPen, 0, 0, Width - 1, Height - 1, 16);
        }

        private static void RoundedRect(Graphics g, Pen pen, int x, int y, int w, int h, int r)
        {
            var path = new GraphicsPath();
            int d = r * 2;
            path.AddArc(x, y, d, d, 180, 90);
            path.AddArc(x + w - d, y, d, d, 270, 90);
            path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
            path.AddArc(x, y + h - d, d, d, 90, 90);
            path.CloseFigure();
            g.DrawPath(pen, path);
        }
    }
}
