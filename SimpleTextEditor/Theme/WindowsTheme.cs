using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleTextEditor.Theme
{
    public static class WindowsTheme
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        public static void EnableDarkMode(IntPtr handle)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = 1;
                DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int));
            }
        }
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public static void ApplyDarkThemeToScrollbars(IntPtr handle)
        {
            SetWindowTheme(handle, "DarkMode_Explorer", null);
        }

        public static void ApplyDarkThemeToComboBox(IntPtr handle)
        {
            // DarkMode_Explorer correctly themes the popup list border and scrollbar
            SetWindowTheme(handle, "DarkMode_Explorer", null);
        }

        public static void ApplyDarkThemeToAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is ComboBox)
                {
                    ApplyDarkThemeToComboBox(c.Handle);
                }
                else if (c is Panel || c is FlowLayoutPanel || c is TableLayoutPanel)
                {
                    ApplyDarkThemeToScrollbars(c.Handle);
                }
                ApplyDarkThemeToAllControls(c);
            }
        }
    }
}
