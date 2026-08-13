using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NLog;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.Services
{
    /// <summary>
    /// Shows short, styled action notifications in the app's status bar.
    /// Each notification fades away automatically after a short delay.
    /// Simultaneously logs every action via NLog.
    /// </summary>
    public class NotificationService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ToolStripStatusLabel _label;
        private System.Windows.Forms.Timer    _hideTimer;
        private Color                          _defaultColor;

        // Icons for different action categories
        private static readonly Dictionary<NotifyKind, (string icon, Color color)> Styles = new()
        {
            [NotifyKind.FormatOn]    = ("✅", Color.FromArgb(134, 239, 172)),   // soft green
            [NotifyKind.FormatOff]   = ("🚫", Color.FromArgb(252, 165, 165)),   // soft red
            [NotifyKind.Insert]      = ("➕", Color.FromArgb(147, 197, 253)),   // soft blue
            [NotifyKind.Heading]     = ("📝", Color.FromArgb(216, 180, 254)),   // soft purple
            [NotifyKind.File]        = ("💾", Color.FromArgb(253, 224, 71)),    // soft yellow
            [NotifyKind.Info]        = ("ℹ️",  Color.FromArgb(203, 213, 225)),   // soft grey
        };

        public NotificationService(ToolStripStatusLabel label)
        {
            _label        = label;
            _defaultColor = label.ForeColor;

            _hideTimer = new System.Windows.Forms.Timer { Interval = 2800 };
            _hideTimer.Tick += (s, e) =>
            {
                _hideTimer.Stop();
                _label.Text      = string.Empty;
                _label.ForeColor = _defaultColor;
            };
        }

        // ── Public API ─────────────────────────────────────────────────────────────

        public void Show(string message, NotifyKind kind = NotifyKind.Info, string logMessage = null)
        {
            var (icon, color) = Styles[kind];

            _label.Text      = $"  {icon}  {message}";
            _label.ForeColor = color;

            // Restart timer
            _hideTimer.Stop();
            _hideTimer.Start();

            // Also write to NLog file
            string logText = logMessage ?? message;
            if (kind == NotifyKind.FormatOff)
                Log.Info("[{0}] {1}", kind, logText);
            else
                Log.Info("[{0}] {1}", kind, logText);
        }

        // ── Convenience shortcuts ──────────────────────────────────────────────────

        public void FormatOn(string what)  => Show(what + " увімкнено",  NotifyKind.FormatOn,  what + " ON");
        public void FormatOff(string what) => Show(what + " вимкнено",   NotifyKind.FormatOff, what + " OFF");
        public void Inserted(string what)  => Show(what + " додано",     NotifyKind.Insert,    what + " inserted");
        public void HeadingApplied(int level, float size) =>
            Show($"Заголовок H{level} ({size:0}pt)", NotifyKind.Heading, $"Heading H{level}");
        public void FileSaved(string name) => Show($"Збережено: {name}", NotifyKind.File, $"File saved: {name}");
    }

    public enum NotifyKind { FormatOn, FormatOff, Insert, Heading, File, Info }
}
