using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public class ToggleSwitch : CheckBox
    {
        public ToggleSwitch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            AutoSize = false;
            Width = 40;
            Height = 20;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent.BackColor);

            Color onColor = Color.FromArgb(76, 194, 255); // Win 11 cyan
            Color offColor = Color.FromArgb(45, 45, 45); // Dark gray
            Color borderColor = Color.FromArgb(100, 100, 100);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath path = RoundedRect(rect, Height);

            using (SolidBrush brush = new SolidBrush(Checked ? onColor : offColor))
            {
                g.FillPath(brush, path);
            }

            if (!Checked)
            {
                using (Pen pen = new Pen(borderColor, 1))
                {
                    g.DrawPath(pen, path);
                }
            }

            int circleMargin = 3;
            int circleSize = Height - (circleMargin * 2) - 1;
            int circleX = Checked ? Width - circleSize - circleMargin - 1 : circleMargin + 1;
            
            Color circleColor = Checked ? Color.Black : Color.FromArgb(200, 200, 200);

            using (SolidBrush brush = new SolidBrush(circleColor))
            {
                g.FillEllipse(brush, circleX, circleMargin, circleSize, circleSize);
            }
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int diameter)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
