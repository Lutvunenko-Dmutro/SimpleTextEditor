using System.Windows.Forms;

namespace SimpleTextEditor.UI
{
    public static partial class MenuBuilder
    {
        private static ToolStripMenuItem BuildFileMenu(Form1 mainForm)
        {
            var fileMenu = Menu("Файл", Icons.MenuFile);
            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                Item("Нова вкладка",          "Ctrl+N",  (s,e) => mainForm.tabManager.AddNewTab("Untitled", ""), Keys.Control | Keys.N),
                Item("Нове вікно",            "Ctrl+Shift+N", (s,e) => System.Diagnostics.Process.Start(Application.ExecutablePath), Keys.Control | Keys.Shift | Keys.N),
                Item("Нова вкладка Markdown", "", (s,e) => mainForm.tabManager.AddNewTab("Untitled.md", "", null, "Markdown")),
                Sep(),
                Item("Відкрити…",    "Ctrl+O",  mainForm.OpenFile,    Keys.Control | Keys.O),
                Item("Останні файли", "",        (s,e) => { /* TODO */ }),
                Sep(),
                Item("Зберегти",     "Ctrl+S",  mainForm.SaveFile,    Keys.Control | Keys.S),
                Item("Зберегти як…","Ctrl+Shift+S", mainForm.SaveAsFile, Keys.Control | Keys.Shift | Keys.S),
                Item("Зберегти все","Ctrl+Alt+S", mainForm.SaveAllFiles, Keys.Control | Keys.Alt | Keys.S),
                Sep(),
                Item("Параметри сторінки…", "", mainForm.PageSetup),
                Item("Друк…",       "Ctrl+P",  mainForm.PrintFile, Keys.Control | Keys.P),
                Sep(),
                Item("Закрити вкладку", "Ctrl+W", (s,e) => mainForm.tabManager.CloseCurrentTab(), Keys.Control | Keys.W),
                Item("Закрити вікно",   "Ctrl+Shift+W", (s,e) => mainForm.Close(), Keys.Control | Keys.Shift | Keys.W),
                Sep(),
                Item("Вийти", "", (s,e) => mainForm.Close())
            });
            return fileMenu;
        }
    }
}
