using System.Windows.Forms;
using SimpleTextEditor.Services;

namespace SimpleTextEditor.UI
{
    public static partial class MenuBuilder
    {
        private static ToolStripMenuItem BuildViewMenu(Form1 mainForm)
        {
            var viewMenu = Menu("Переглянути", Icons.MenuView);

            var zoomMenu = Menu("Масштаб");
            zoomMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                Item("Збільшити",   "Ctrl++", (s,e) => ZoomService.AdjustZoom(mainForm, +0.1f), Keys.Control | Keys.Oemplus),
                Item("Зменшити",    "Ctrl+-", (s,e) => ZoomService.AdjustZoom(mainForm, -0.1f), Keys.Control | Keys.OemMinus),
                Item("100%",        "Ctrl+0", (s,e) => ZoomService.ResetZoom(mainForm),          Keys.Control | Keys.D0)
            });

            var markdownMenu = Menu("Режим редактора");
            
            var modeFormattedItem = Item("Форматований (WYSIWYG)", "", (s, e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) mainForm.tabManager.SetFormattedMode(rtb, true);
            });
            var modeMarkdownItem = Item("Звичайний текст (Markdown)", "", (s, e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) mainForm.tabManager.SetFormattedMode(rtb, false);
            });
            var previewMarkdownItem = Item("Попередній перегляд Markdown", "", (s, e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null)
                {
                    mainForm.tabManager.SetFormattedMode(rtb, false); // force markdown mode
                    mainForm.tabManager.PreviewManager.SetPreviewMode(rtb, true, rtb);
                }
            });

            markdownMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                modeFormattedItem,
                modeMarkdownItem,
                Sep(),
                previewMarkdownItem
            });

            markdownMenu.DropDownOpening += (s, e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null)
                {
                    bool isFormatted = mainForm.tabManager.IsFormattedMode(rtb);
                    ((ToolStripMenuItem)modeFormattedItem).Checked = isFormatted;
                    ((ToolStripMenuItem)modeMarkdownItem).Checked = !isFormatted;
                    ((ToolStripMenuItem)previewMarkdownItem).Checked = mainForm.tabManager.PreviewManager.IsPreviewMode(rtb);
                    previewMarkdownItem.Enabled = !isFormatted; // Can only preview in markdown mode
                }
            };

            var wordWrapItem = CheckItem("Перенос по словах", true, (s,e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) rtb.WordWrap = !rtb.WordWrap;
            });

            var statusBarItem = CheckItem("Рядок стану", true, (s,e) =>
            {
                mainForm.statusStrip1.Visible = !mainForm.statusStrip1.Visible;
            });

            viewMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                zoomMenu, wordWrapItem, statusBarItem,
                Sep(),
                markdownMenu,
                Sep(),
                Item("Про програму", "", mainForm.ShowAboutForm)
            });

            return viewMenu;
        }
    }
}
