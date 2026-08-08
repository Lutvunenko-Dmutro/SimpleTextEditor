using System.Drawing;
using System.Windows.Forms;

namespace SimpleTextEditor.Theme
{
    public class DarkColorTable : ProfessionalColorTable
    {
        // ─── Dropdown / ToolStripDropDown Backgrounds ────────────────────────
        public override Color ToolStripDropDownBackground   => AppTheme.Surface;
        public override Color ImageMarginGradientBegin      => AppTheme.Surface;
        public override Color ImageMarginGradientMiddle     => AppTheme.Surface;
        public override Color ImageMarginGradientEnd        => AppTheme.Surface;

        // ─── Borders ─────────────────────────────────────────────────────────
        public override Color MenuBorder                    => AppTheme.Border;
        public override Color MenuItemBorder                => AppTheme.Border;
        public override Color SeparatorDark                 => AppTheme.Border;
        public override Color SeparatorLight                => AppTheme.Border;

        // ─── Menu Strip ───────────────────────────────────────────────────────
        public override Color MenuStripGradientBegin        => AppTheme.SurfaceHigh;
        public override Color MenuStripGradientEnd          => AppTheme.SurfaceHigh;

        // ─── Menu Item Selected / Hover ───────────────────────────────────────
        public override Color MenuItemSelected              => AppTheme.Accent;
        public override Color MenuItemSelectedGradientBegin => AppTheme.Accent;
        public override Color MenuItemSelectedGradientEnd   => AppTheme.Accent;
        public override Color MenuItemPressedGradientBegin  => AppTheme.AccentDark;
        public override Color MenuItemPressedGradientMiddle => AppTheme.AccentDark;
        public override Color MenuItemPressedGradientEnd    => AppTheme.AccentDark;

        // ─── ToolStrip ────────────────────────────────────────────────────────
        public override Color ToolStripBorder               => AppTheme.Border;
        public override Color ToolStripGradientBegin        => AppTheme.SurfaceHigh;
        public override Color ToolStripGradientMiddle       => AppTheme.SurfaceHigh;
        public override Color ToolStripGradientEnd          => AppTheme.SurfaceHigh;
        public override Color ToolStripPanelGradientBegin   => AppTheme.SurfaceHigh;
        public override Color ToolStripPanelGradientEnd     => AppTheme.SurfaceHigh;
        public override Color ToolStripContentPanelGradientBegin => AppTheme.Surface;
        public override Color ToolStripContentPanelGradientEnd   => AppTheme.Surface;

        // ─── Button Hover ─────────────────────────────────────────────────────
        public override Color ButtonSelectedHighlight       => AppTheme.Accent;
        public override Color ButtonSelectedHighlightBorder => AppTheme.AccentLight;
        public override Color ButtonSelectedGradientBegin   => AppTheme.Accent;
        public override Color ButtonSelectedGradientMiddle  => AppTheme.Accent;
        public override Color ButtonSelectedGradientEnd     => AppTheme.Accent;
        public override Color ButtonSelectedBorder          => AppTheme.AccentLight;

        // ─── Button Pressed ───────────────────────────────────────────────────
        public override Color ButtonPressedGradientBegin   => AppTheme.AccentDark;
        public override Color ButtonPressedGradientMiddle  => AppTheme.AccentDark;
        public override Color ButtonPressedGradientEnd     => AppTheme.AccentDark;
        public override Color ButtonPressedHighlight       => AppTheme.AccentDark;
        public override Color ButtonPressedHighlightBorder => AppTheme.Accent;
        public override Color ButtonPressedBorder          => AppTheme.Accent;

        // ─── Status Strip ─────────────────────────────────────────────────────
        public override Color StatusStripGradientBegin     => AppTheme.Background;
        public override Color StatusStripGradientEnd       => AppTheme.Background;

        // ─── Check Mark ───────────────────────────────────────────────────────
        public override Color CheckBackground              => AppTheme.Accent;
        public override Color CheckSelectedBackground      => AppTheme.AccentDark;
        public override Color CheckPressedBackground       => AppTheme.AccentDark;
    }
}
