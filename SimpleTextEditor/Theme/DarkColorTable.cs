using System.Drawing;
using System.Windows.Forms;

namespace SimpleTextEditor.Theme
{
    public class DarkColorTable : ProfessionalColorTable
    {
        // Slate 800 for backgrounds
        public override Color ToolStripDropDownBackground => AppTheme.Surface;
        public override Color ImageMarginGradientBegin => AppTheme.Surface;
        public override Color ImageMarginGradientMiddle => AppTheme.Surface;
        public override Color ImageMarginGradientEnd => AppTheme.Surface;
        
        // Slate 900 for Borders
        public override Color MenuBorder => AppTheme.Background;
        public override Color MenuItemBorder => AppTheme.Background;
        
        // Blue 500 for selected/hover item
        public override Color MenuItemSelected => AppTheme.Accent;
        public override Color MenuStripGradientBegin => AppTheme.Surface;
        public override Color MenuStripGradientEnd => AppTheme.Surface;
        public override Color MenuItemSelectedGradientBegin => AppTheme.Accent;
        public override Color MenuItemSelectedGradientEnd => AppTheme.Accent;
        public override Color MenuItemPressedGradientBegin => AppTheme.Accent;
        public override Color MenuItemPressedGradientEnd => AppTheme.Accent;
        
        // Remove ToolStrip white lines
        // Slate 800 / Slate 900
        public override Color ToolStripBorder => AppTheme.Background;
        public override Color ToolStripGradientBegin => AppTheme.Surface;
        public override Color ToolStripGradientMiddle => AppTheme.Surface;
        public override Color ToolStripGradientEnd => AppTheme.Surface;
        public override Color ToolStripPanelGradientBegin => AppTheme.Surface;
        public override Color ToolStripPanelGradientEnd => AppTheme.Surface;
        
        // Button Hover and Pressed (ToolStrip) - Blue Accent
        public override Color ButtonSelectedHighlight => AppTheme.Accent;
        public override Color ButtonSelectedHighlightBorder => AppTheme.Accent;
        public override Color ButtonSelectedGradientBegin => AppTheme.Accent;
        public override Color ButtonSelectedGradientEnd => AppTheme.Accent;
        public override Color ButtonSelectedBorder => AppTheme.Accent;
        
        // Button Pressed - Darker Blue
        public override Color ButtonPressedGradientBegin => AppTheme.AccentDark;
        public override Color ButtonPressedGradientMiddle => AppTheme.AccentDark;
        public override Color ButtonPressedGradientEnd => AppTheme.AccentDark;
        public override Color ButtonPressedHighlight => AppTheme.AccentDark;
        public override Color ButtonPressedHighlightBorder => AppTheme.AccentDark;
        public override Color ButtonPressedBorder => AppTheme.AccentDark;
    }
}
