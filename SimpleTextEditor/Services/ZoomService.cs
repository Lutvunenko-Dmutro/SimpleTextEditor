using System;

namespace SimpleTextEditor.Services
{
    public static class ZoomService
    {
        public static void AdjustZoom(Form1 mainForm, float delta)
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb == null) return;
            rtb.ZoomFactor = Math.Clamp(rtb.ZoomFactor + delta, 0.1f, 5.0f);
            if (mainForm.zoomLabel != null)
                mainForm.zoomLabel.Text = $"{(int)(rtb.ZoomFactor * 100)}%";
        }

        public static void ResetZoom(Form1 mainForm)
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb == null) return;
            rtb.ZoomFactor = 1.0f;
            if (mainForm.zoomLabel != null)
                mainForm.zoomLabel.Text = "100%";
        }
    }
}
