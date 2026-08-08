using System.Drawing;

namespace SimpleTextEditor.Theme
{
    public static class AppTheme
    {
        // ── Obsidian Dark Palette ──────────────────────────────────────────────
        
        // Deep black-blue base
        public static Color Background    { get; set; } = Color.FromArgb(13, 13, 18);
        
        // Card / surface layer
        public static Color Surface       { get; set; } = Color.FromArgb(22, 22, 32);
        
        // Elevated surface (e.g., toolbar, menu)
        public static Color SurfaceHigh   { get; set; } = Color.FromArgb(32, 32, 48);
        
        // Hover state
        public static Color SurfaceHover  { get; set; } = Color.FromArgb(45, 45, 65);

        // ── Violet / Purple Accent ──────────────────────────────────────────────
        public static Color Accent        { get; set; } = Color.FromArgb(124, 77, 255);   // vivid violet
        public static Color AccentLight   { get; set; } = Color.FromArgb(167, 139, 250);  // soft lavender
        public static Color AccentDark    { get; set; } = Color.FromArgb(91, 33, 182);    // deep purple

        // ── Text Hierarchy ──────────────────────────────────────────────────────
        public static Color TextPrimary   { get; set; } = Color.FromArgb(240, 238, 255);  // near-white with violet tint
        public static Color TextEditor    { get; set; } = Color.FromArgb(230, 228, 250);  // editor text
        public static Color TextSecondary { get; set; } = Color.FromArgb(172, 169, 210);  // muted labels
        public static Color TextMuted     { get; set; } = Color.FromArgb(100, 97, 145);   // very muted

        // ── Borders & Separators ────────────────────────────────────────────────
        public static Color Border        { get; set; } = Color.FromArgb(42, 40, 70);
        public static Color BorderActive  { get; set; } = Color.FromArgb(124, 77, 255);

        // ── Semantic Colours ────────────────────────────────────────────────────
        public static Color Success       { get; set; } = Color.FromArgb(72, 199, 142);
        public static Color Warning       { get; set; } = Color.FromArgb(255, 183, 77);
        public static Color Error         { get; set; } = Color.FromArgb(255, 82, 82);
        
        // ── Tab Strip ───────────────────────────────────────────────────────────
        public static Color TabBackground { get; set; } = Color.FromArgb(13, 13, 18);
        public static Color TabActive     { get; set; } = Color.FromArgb(22, 22, 32);
        public static Color TabHover      { get; set; } = Color.FromArgb(28, 28, 42);
        public static Color TabInactive   { get; set; } = Color.FromArgb(13, 13, 18);
    }
}
