using System.Drawing;
using System.Windows.Forms;

namespace SimpleTextEditor.Theme
{
    public class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer() : base(new DarkColorTable())
        {
            this.RoundedEdges = false;
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;
            if (e.ToolStrip is StatusStrip)
            {
                g.DrawLine(new Pen(AppTheme.Border), 0, 0, e.ToolStrip.Width, 0);
            }
            else
            {
                g.DrawLine(new Pen(AppTheme.Border), 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
            }
        }
    }
}
