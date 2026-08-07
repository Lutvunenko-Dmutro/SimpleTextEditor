using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class MenuBuilder
    {
        public static MenuStrip BuildCombinedMenu(Form1 mainForm)
        {
            MenuStrip mainMenu = new MenuStrip();
            mainMenu.BackColor = AppTheme.Surface;
            mainMenu.ForeColor = AppTheme.TextPrimary;
            mainMenu.Renderer = new ToolStripProfessionalRenderer(new DarkColorTable());
            mainMenu.Padding = new Padding(8, 6, 8, 6);
            mainMenu.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);

            // ================== FILE MENU ==================
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            
            ToolStripMenuItem newTabItem = new ToolStripMenuItem("Створити вкладку", null, (s, e) => mainForm.tabManager.AddNewTab("Untitled", ""), Keys.Control | Keys.N);
            ToolStripMenuItem newWindowItem = new ToolStripMenuItem("Створити вікно", null, (s, e) => System.Diagnostics.Process.Start(Application.ExecutablePath), Keys.Control | Keys.Shift | Keys.N);
            ToolStripMenuItem newMarkdownItem = new ToolStripMenuItem("Нова вкладка Markdown", null, (s, e) => mainForm.tabManager.AddNewTab("Untitled", "", null, "Синтаксис Markdown"));
            ToolStripMenuItem openItem = new ToolStripMenuItem("Відкрити", null, mainForm.OpenFile, Keys.Control | Keys.O);
            ToolStripMenuItem recentItem = new ToolStripMenuItem("Останні");
            
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Зберегти", null, mainForm.SaveFile, Keys.Control | Keys.S);
            ToolStripMenuItem saveAsItem = new ToolStripMenuItem("Зберегти як", null, mainForm.SaveAsFile, Keys.Control | Keys.Shift | Keys.S);
            ToolStripMenuItem saveAllItem = new ToolStripMenuItem("Зберегти все", null, mainForm.SaveAllFiles, Keys.Control | Keys.Alt | Keys.S);
            
            ToolStripMenuItem pageSetupItem = new ToolStripMenuItem("Параметри сторінки", null, mainForm.PageSetup);
            ToolStripMenuItem printItem = new ToolStripMenuItem("Друк", null, mainForm.PrintFile, Keys.Control | Keys.P);
            
            ToolStripMenuItem closeTabItem = new ToolStripMenuItem("Закрити вкладку", null, (s, e) => mainForm.tabManager.CloseCurrentTab(), Keys.Control | Keys.W);
            ToolStripMenuItem closeWindowItem = new ToolStripMenuItem("Закрити вікно", null, (s, e) => mainForm.Close(), Keys.Control | Keys.Shift | Keys.W);
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Вийти", null, (s, e) => mainForm.Close());

            fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
                newTabItem, newWindowItem, newMarkdownItem, openItem, recentItem,
                new ToolStripSeparator(),
                saveItem, saveAsItem, saveAllItem,
                new ToolStripSeparator(),
                pageSetupItem, printItem,
                new ToolStripSeparator(),
                closeTabItem, closeWindowItem, exitItem
            });

            // ================== EDIT MENU ==================
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Редагувати");
            
            ToolStripMenuItem undoItem = new ToolStripMenuItem("Скасувати", null, (s, e) => mainForm.tabManager.CurrentEditor?.Undo(), Keys.Control | Keys.Z);
            ToolStripMenuItem cutItem = new ToolStripMenuItem("Вирізати", null, (s, e) => mainForm.tabManager.CurrentEditor?.Cut(), Keys.Control | Keys.X);
            ToolStripMenuItem copyItem = new ToolStripMenuItem("Копіювати", null, (s, e) => mainForm.tabManager.CurrentEditor?.Copy(), Keys.Control | Keys.C);
            ToolStripMenuItem pasteItem = new ToolStripMenuItem("Вставити", null, (s, e) => mainForm.tabManager.CurrentEditor?.Paste(), Keys.Control | Keys.V);
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Видалити", null, (s, e) => { if (mainForm.tabManager.CurrentEditor != null) mainForm.tabManager.CurrentEditor.SelectedText = ""; }, Keys.Delete);
            
            ToolStripMenuItem clearFormatItem = new ToolStripMenuItem("Очистити форматування", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) { rtb.SelectionFont = rtb.Font; rtb.SelectionColor = rtb.ForeColor; }
            });
            
            ToolStripMenuItem bingItem = new ToolStripMenuItem("Пошук у Bing", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null && !string.IsNullOrEmpty(rtb.SelectedText))
                    System.Diagnostics.Process.Start($"https://www.bing.com/search?q={Uri.EscapeDataString(rtb.SelectedText)}");
            }, Keys.Control | Keys.E);

            ToolStripMenuItem findItem = new ToolStripMenuItem("Пошук", null, null, Keys.Control | Keys.F);
            ToolStripMenuItem findNextItem = new ToolStripMenuItem("Знайти далі", null, null, Keys.F3);
            ToolStripMenuItem findPrevItem = new ToolStripMenuItem("Знайти попереднє", null, null, Keys.Shift | Keys.F3);
            ToolStripMenuItem replaceItem = new ToolStripMenuItem("Замінити", null, null, Keys.Control | Keys.H);
            ToolStripMenuItem gotoItem = new ToolStripMenuItem("Перейти", null, null, Keys.Control | Keys.G);
            
            ToolStripMenuItem selectAllItem = new ToolStripMenuItem("Вибрати все", null, (s, e) => mainForm.tabManager.CurrentEditor?.SelectAll(), Keys.Control | Keys.A);
            ToolStripMenuItem dateTimeItem = new ToolStripMenuItem("Дата й час", null, (s, e) => {
                if (mainForm.tabManager.CurrentEditor != null)
                    mainForm.tabManager.CurrentEditor.SelectedText = DateTime.Now.ToString("HH:mm dd.MM.yyyy");
            }, Keys.F5);

            ToolStripMenuItem fontMenuItem = new ToolStripMenuItem("Шрифт", null, (s, e) => mainForm.formatHandler.ChangeFont(mainForm.tabManager.CurrentEditor));
            ToolStripMenuItem colorMenuItem = new ToolStripMenuItem("Колір", null, (s, e) => mainForm.formatHandler.ChangeColor(mainForm.tabManager.CurrentEditor));

            editMenu.DropDownItems.AddRange(new ToolStripItem[] {
                undoItem, cutItem, copyItem, pasteItem, deleteItem,
                new ToolStripSeparator(),
                clearFormatItem,
                new ToolStripSeparator(),
                bingItem,
                new ToolStripSeparator(),
                findItem, findNextItem, findPrevItem, replaceItem, gotoItem,
                new ToolStripSeparator(),
                selectAllItem, dateTimeItem,
                new ToolStripSeparator(),
                fontMenuItem, colorMenuItem
            });

            // ================== VIEW MENU ==================
            ToolStripMenuItem viewMenu = new ToolStripMenuItem("Переглянути");
            
            ToolStripMenuItem zoomMenu = new ToolStripMenuItem("Масштаб");
            ToolStripMenuItem zoomInItem = new ToolStripMenuItem("Збільшити", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null && rtb.ZoomFactor < 64.0f) { rtb.ZoomFactor = Math.Min(64.0f, rtb.ZoomFactor + 0.1f); UpdateZoomLabel(mainForm); }
            }, Keys.Control | Keys.Oemplus);
            
            ToolStripMenuItem zoomOutItem = new ToolStripMenuItem("Зменшити", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null && rtb.ZoomFactor > 0.1f) { rtb.ZoomFactor = Math.Max(0.1f, rtb.ZoomFactor - 0.1f); UpdateZoomLabel(mainForm); }
            }, Keys.Control | Keys.OemMinus);
            
            ToolStripMenuItem zoomResetItem = new ToolStripMenuItem("Відновити масштаб за замовчуванням", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) { rtb.ZoomFactor = 1.0f; UpdateZoomLabel(mainForm); }
            }, Keys.Control | Keys.D0);
            
            zoomMenu.DropDownItems.AddRange(new ToolStripItem[] { zoomInItem, zoomOutItem, zoomResetItem });

            ToolStripMenuItem statusBarItem = new ToolStripMenuItem("Рядок стану", null, (s, e) => {
                mainForm.statusStrip1.Visible = !mainForm.statusStrip1.Visible;
                ((ToolStripMenuItem)s).Checked = mainForm.statusStrip1.Visible;
            });
            statusBarItem.Checked = true;

            ToolStripMenuItem wordWrapItem = new ToolStripMenuItem("Перенос по словах", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) {
                    rtb.WordWrap = !rtb.WordWrap;
                    ((ToolStripMenuItem)s).Checked = rtb.WordWrap;
                }
            });
            wordWrapItem.Checked = true;

            ToolStripMenuItem markdownMenu = new ToolStripMenuItem("Markdown");
            ToolStripMenuItem formattedItem = new ToolStripMenuItem("Форматований", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) mainForm.tabManager.PreviewManager.SetPreviewMode(rtb, true, rtb);
            });
            ToolStripMenuItem syntaxItem = new ToolStripMenuItem("Синтаксис Markdown", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null) mainForm.tabManager.PreviewManager.SetPreviewMode(rtb, false, rtb);
            });
            markdownMenu.DropDownItems.AddRange(new ToolStripItem[] { formattedItem, syntaxItem });

            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("Про програму", null, mainForm.ShowAboutForm);

            viewMenu.DropDownItems.AddRange(new ToolStripItem[] {
                zoomMenu, statusBarItem, wordWrapItem, markdownMenu,
                new ToolStripSeparator(),
                aboutMenuItem
            });

            // Style dropdown items
            foreach (ToolStripMenuItem menu in new[] { fileMenu, editMenu, viewMenu }) {
                StyleMenu(menu);
            }

            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(editMenu);
            mainMenu.Items.Add(viewMenu);

            return mainMenu;
        }



        private static void UpdateZoomLabel(Form1 mainForm)
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb != null && mainForm.zoomLabel != null)
            {
                mainForm.zoomLabel.Text = $"{(int)(rtb.ZoomFactor * 100)}%";
            }
        }

        private static void StyleMenu(ToolStripDropDownItem menu)
        {
            foreach (ToolStripItem item in menu.DropDownItems)
            {
                item.BackColor = AppTheme.Surface;
                item.ForeColor = AppTheme.TextPrimary;
                if (item is ToolStripMenuItem subMenu)
                {
                    StyleMenu(subMenu);
                }
            }
        }
    }
}
