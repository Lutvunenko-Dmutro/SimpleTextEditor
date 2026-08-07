using System.Drawing;

namespace SimpleTextEditor.Theme
{
    public static class AppTheme
    {
        // Slate 900
        public static Color Background { get; set; } = Color.FromArgb(15, 23, 42);
        
        // Slate 800
        public static Color Surface { get; set; } = Color.FromArgb(30, 41, 59);
        
        // Slate 700 / Hover
        public static Color SurfaceHover { get; set; } = Color.FromArgb(51, 65, 85);

        // Blue 500 (Primary Accent)
        public static Color Accent { get; set; } = Color.FromArgb(59, 130, 246);
        
        // Blue 400 (Secondary Accent, Icons)
        public static Color AccentLight { get; set; } = Color.FromArgb(96, 165, 250);
        
        // Blue 600 (Pressed state)
        public static Color AccentDark { get; set; } = Color.FromArgb(37, 99, 235);

        // White
        public static Color TextPrimary { get; set; } = Color.White;
        
        // Slate 100
        public static Color TextEditor { get; set; } = Color.FromArgb(241, 245, 249);
        
        // Slate 300
        public static Color TextSecondary { get; set; } = Color.FromArgb(203, 213, 225);
        
        // Slate 400
        public static Color TextMuted { get; set; } = Color.FromArgb(148, 163, 184);
    }
}
