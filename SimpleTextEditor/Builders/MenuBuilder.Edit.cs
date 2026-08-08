using System.Windows.Forms;

namespace SimpleTextEditor.UI
{
    public static partial class MenuBuilder
    {
        private static ToolStripMenuItem BuildEditMenu(Form1 mainForm)
        {
            var editMenu = Menu("Редагувати", Icons.MenuEdit);

            var itemUndo    = Item("Скасувати",   "Ctrl+Z",  (s,e) => { mainForm.tabManager.CurrentEditor?.Focus(); mainForm.tabManager.CurrentEditor?.Undo(); });
            var itemRedo    = Item("Повторити",   "Ctrl+Y",  (s,e) => { mainForm.tabManager.CurrentEditor?.Focus(); mainForm.tabManager.CurrentEditor?.Redo(); });
            var itemCut     = Item("Вирізати",    "Ctrl+X",  (s,e) => mainForm.tabManager.CurrentEditor?.Cut(),    Keys.Control | Keys.X);
            var itemCopy    = Item("Копіювати",   "Ctrl+C",  (s,e) => mainForm.tabManager.CurrentEditor?.Copy(),   Keys.Control | Keys.C);
            var itemPaste   = Item("Вставити",    "Ctrl+V",  (s,e) => mainForm.tabManager.CurrentEditor?.Paste(),  Keys.Control | Keys.V);
            var itemDelete  = Item("Видалити",    "Del",     (s,e) => { if (mainForm.tabManager.CurrentEditor != null) mainForm.tabManager.CurrentEditor.SelectedText = ""; });
            var itemSelAll  = Item("Вибрати все", "Ctrl+A",  (s,e) => { mainForm.tabManager.CurrentEditor?.SelectAll(); mainForm.tabManager.CurrentEditor?.Focus(); }, Keys.Control | Keys.A);
            var itemDate    = Item("Дата й час",  "F5",
                 (s,e) => { if (mainForm.tabManager.CurrentEditor != null) mainForm.tabManager.CurrentEditor.SelectedText = System.DateTime.Now.ToString("HH:mm dd.MM.yyyy"); },
                 Keys.F5);
            var itemBing    = Item("Пошук у Bing","Ctrl+E",
                 (s,e) => {
                     var rtb = mainForm.tabManager.CurrentEditor;
                     if (rtb != null && !string.IsNullOrEmpty(rtb.SelectedText))
                         System.Diagnostics.Process.Start($"https://www.bing.com/search?q={System.Uri.EscapeDataString(rtb.SelectedText)}");
                 }, Keys.Control | Keys.E);
            var itemFont    = Item("Налаштування…", "", (s,e) => mainForm.formatHandler.ChangeFont(mainForm.tabManager.CurrentEditor));
            var itemColor   = Item("Колір тексту…", "", (s,e) => mainForm.formatHandler.ChangeColor(mainForm.tabManager.CurrentEditor));

            editMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                itemUndo, itemRedo,
                Sep(),
                itemCut, itemCopy, itemPaste, itemDelete,
                Sep(),
                itemSelAll,
                Sep(),
                itemDate, itemBing,
                Sep(),
                itemFont, itemColor
            });

            editMenu.DropDownOpening += (s, e) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                bool hasText      = rtb != null && rtb.TextLength > 0;
                bool hasSelection = rtb != null && rtb.SelectionLength > 0;
                bool canUndo      = rtb != null && rtb.CanUndo;
                bool canRedo      = rtb != null && rtb.CanRedo;
                bool hasClip      = Clipboard.ContainsText();

                itemUndo.Enabled   = canUndo;
                itemRedo.Enabled   = canRedo;
                itemCut.Enabled    = hasSelection;
                itemCopy.Enabled   = hasSelection;
                itemDelete.Enabled = hasSelection;
                itemPaste.Enabled  = hasClip;
                itemSelAll.Enabled = hasText;
                itemBing.Enabled   = hasSelection;
            };

            return editMenu;
        }
    }
}
