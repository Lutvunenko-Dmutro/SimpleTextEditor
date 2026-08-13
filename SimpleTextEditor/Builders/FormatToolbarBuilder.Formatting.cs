using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;


namespace SimpleTextEditor.UI
{
    public static partial class FormatToolbarBuilder
    {
        private static ToolStripDropDownButton BuildHeadingsDropdown(Form1 mainForm)
        {
            var btnHeadings = new ToolStripDropDownButton(null, Icons.Headings, null, "btnHeadings")
            {
                Padding      = new Padding(6, 1, 6, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None,
                ToolTipText  = "Заголовки"
            };

            Action<int, string> applyHeading = (level, mdText) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                
                if (mainForm.tabManager.IsFormattedMode(rtb))
                {
                    mainForm.formatHandler.InsertHeading(rtb, level);
                }
                else
                {
                    ToolbarItemFactory.InsertText(mainForm, mdText);
                }
            };

            btnHeadings.DropDownItems.AddRange(new ToolStripItem[]
            {
                ToolbarItemFactory.DdItem("Заголовок 1", () => applyHeading(1, "# ")),
                ToolbarItemFactory.DdItem("Заголовок 2", () => applyHeading(2, "## ")),
                ToolbarItemFactory.DdItem("Заголовок 3", () => applyHeading(3, "### ")),
                ToolbarItemFactory.DdItem("Заголовок 4", () => applyHeading(4, "#### ")),
                ToolbarItemFactory.DdItem("Заголовок 5", () => applyHeading(5, "##### ")),
                ToolbarItemFactory.DdItem("Заголовок 6", () => applyHeading(6, "###### "))
            });
            ToolbarItemFactory.StyleDropDown(btnHeadings);
            return btnHeadings;
        }

        private static ToolStripDropDownButton BuildListsDropdown(Form1 mainForm)
        {
            var btnLists = new ToolStripDropDownButton(null, Icons.Lists, null, "btnLists")
            {
                Padding      = new Padding(4, 1, 4, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None,
                ToolTipText  = "Списки"
            };
            btnLists.DropDownItems.AddRange(new ToolStripItem[]
            {
                ToolbarItemFactory.DdItem("Маркований список",  () => 
                {
                    var rtb = mainForm.tabManager.CurrentEditor;
                    if (rtb == null) return;
                    if (mainForm.tabManager.IsFormattedMode(rtb)) mainForm.formatHandler.ToggleBulletList(rtb);
                    else ToolbarItemFactory.InsertText(mainForm, "- ");
                }, Icons.ListBulleted),
                ToolbarItemFactory.DdItem("Нумерований список",  () => 
                {
                    var rtb = mainForm.tabManager.CurrentEditor;
                    if (rtb == null) return;
                    if (mainForm.tabManager.IsFormattedMode(rtb)) { rtb.SelectedText = "1. "; }
                    else ToolbarItemFactory.InsertText(mainForm, "1. ");
                }, Icons.ListNumbered),
                ToolbarItemFactory.DdItem("Список завдань",      () => ToolbarItemFactory.InsertText(mainForm, "- [ ] "), Icons.ListTodo)
            });
            ToolbarItemFactory.StyleDropDown(btnLists);
            return btnLists;
        }

        private static ToolStripDropDownButton BuildTableDropdown(Form1 mainForm)
        {
            var btnTable = new ToolStripDropDownButton(null, Icons.Table, null, "btnTable")
            {
                Padding      = new Padding(4, 1, 4, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None,
                ToolTipText  = "Таблиця"
            };
            btnTable.DropDownItems.AddRange(new ToolStripItem[]
            {
                ToolbarItemFactory.DdItem("Вставити таблицю 2×2", () =>
                    ToolbarItemFactory.InsertText(mainForm, "| Стовпець | Стовпець |\n|-----------|----------|\n| Дані      | Дані     |")),
                ToolbarItemFactory.DdItem("Додати рядок",          () => ToolbarItemFactory.InsertText(mainForm, "\n| Новий | Новий |")),
                ToolbarItemFactory.DdItem("Таблиця 3×3",           () =>
                    ToolbarItemFactory.InsertText(mainForm, "| A | B | C |\n|---|---|---|\n| 1 | 2 | 3 |\n| 4 | 5 | 6 |"))
            });
            ToolbarItemFactory.StyleDropDown(btnTable);
            return btnTable;
        }

        private static ToolStripDropDownButton BuildCodeBlockDropdown(Form1 mainForm)
        {
            var btnCodeBlock = new ToolStripDropDownButton(null, Icons.CodeBlock, null, "btnCodeBlock")
            {
                Padding      = new Padding(4, 1, 4, 1),
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                ImageScaling = ToolStripItemImageScaling.None,
                ToolTipText  = "Блок коду"
            };

            Action<string> insertCode = (mdLang) =>
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                
                if (mainForm.tabManager.IsFormattedMode(rtb))
                {
                    mainForm.formatHandler.InsertCodeBlock(rtb);
                }
                else
                {
                    ToolbarItemFactory.InsertText(mainForm, mdLang, "\n```");
                }
            };

            btnCodeBlock.DropDownItems.AddRange(new ToolStripItem[]
            {
                ToolbarItemFactory.DdItem("Без мови",     () => insertCode("```\n")),
                ToolbarItemFactory.DdItem("C#",           () => insertCode("```csharp\n")),
                ToolbarItemFactory.DdItem("JavaScript",   () => insertCode("```js\n")),
                ToolbarItemFactory.DdItem("Python",       () => insertCode("```python\n")),
                ToolbarItemFactory.DdItem("Bash / Shell", () => insertCode("```bash\n")),
                ToolbarItemFactory.DdItem("SQL",          () => insertCode("```sql\n"))
            });
            ToolbarItemFactory.StyleDropDown(btnCodeBlock);
            return btnCodeBlock;
        }

        private static ToolStripItem[] BuildInlineFormatting(Form1 mainForm)
        {
            var btnBold = ToolbarItemFactory.MakeIconBtn(Icons.Bold, "Жирний — виділи текст та натисни", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) mainForm.formatHandler.ToggleBold(rtb);
                else ToolbarItemFactory.InsertText(mainForm, "**", "**");
            });
            var btnItalic = ToolbarItemFactory.MakeIconBtn(Icons.Italic, "Курсив", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) mainForm.formatHandler.ToggleItalic(rtb);
                else ToolbarItemFactory.InsertText(mainForm, "_", "_");
            });
            var btnStrike = ToolbarItemFactory.MakeIconBtn(Icons.Strike, "Закреслений", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) mainForm.formatHandler.ToggleStrike(rtb);
                else ToolbarItemFactory.InsertText(mainForm, "~~", "~~");
            });
            var btnCode = ToolbarItemFactory.MakeIconBtn(Icons.Code, "Інлайн-код  (`код`)", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) {
                    rtb.SelectionFont = new System.Drawing.Font("Consolas", rtb.Font.Size);
                    rtb.SelectionColor = System.Drawing.Color.LightBlue;
                }
                else ToolbarItemFactory.InsertText(mainForm, "`", "`");
            });
            var btnLink = ToolbarItemFactory.MakeIconBtn(Icons.Link, "Посилання  [текст](url)", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                using (var dlg = new LinkDialog(rtb.SelectedText))
                {
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (mainForm.tabManager.IsFormattedMode(rtb)) 
                            mainForm.formatHandler.InsertLink(rtb, dlg.LinkText, dlg.LinkUrl);
                        else 
                            ToolbarItemFactory.InsertText(mainForm, $"[{dlg.LinkText}](", $"{dlg.LinkUrl})");
                    }
                }
            });
            var btnImage = ToolbarItemFactory.MakeIconBtn(Icons.Image, "Зображення  ![alt](url)", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                using (var dlg = new ImageDialog())
                {
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (mainForm.tabManager.IsFormattedMode(rtb)) 
                            mainForm.formatHandler.InsertImage(rtb, dlg.ImageUrl, dlg.AltText);
                        else 
                            ToolbarItemFactory.InsertText(mainForm, $"![{dlg.AltText}](", $"{dlg.ImageUrl})");
                    }
                }
            });
            var btnQuote = ToolbarItemFactory.MakeIconBtn(Icons.Quote, "Цитата", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) mainForm.formatHandler.InsertQuote(rtb);
                else ToolbarItemFactory.InsertText(mainForm, "> ");
            });
            var btnHR = ToolbarItemFactory.MakeIconBtn(Icons.HR, "Горизонтальна лінія  (---)", (s, e) => 
            {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null) return;
                if (mainForm.tabManager.IsFormattedMode(rtb)) 
                {
                    rtb.SelectedText = "\n────────────────────────────────────────\n";
                }
                else ToolbarItemFactory.InsertText(mainForm, "\n---\n");
            });

            return new ToolStripItem[]
            {
                btnBold, btnItalic, btnStrike, btnCode,
                new ToolStripSeparator(),
                btnLink, btnImage, btnQuote, btnHR,
                new ToolStripSeparator()
            };
        }

        public static ToolStripItem BuildFontFamilyDropdown(Form1 mainForm)
        {
            var btnFont = new ToolStripDropDownButton
            {
                Text         = "Segoe UI",
                ToolTipText  = "Шрифт",
                AutoSize     = false,
                Width        = 130,
                BackColor    = AppTheme.Surface,
                ForeColor    = AppTheme.TextPrimary,
                Font         = new System.Drawing.Font("Segoe UI", 9.5f),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            var popupList = new ListBox
            {
                BackColor      = AppTheme.Surface,
                ForeColor      = AppTheme.TextPrimary,
                BorderStyle    = BorderStyle.None,
                IntegralHeight = false,
                Height         = 300,
                Width          = 180,
                Font           = new System.Drawing.Font("Segoe UI", 9.5f)
            };
            popupList.HandleCreated += (s, e) => Theme.WindowsTheme.ApplyDarkThemeToScrollbars(popupList.Handle);

            foreach (var family in System.Drawing.FontFamily.Families)
                popupList.Items.Add(family.Name);

            int segoeIdx = popupList.Items.IndexOf("Segoe UI");
            if (segoeIdx >= 0) popupList.SelectedIndex = segoeIdx;

            var host = new ToolStripControlHost(popupList) { Margin = Padding.Empty, Padding = Padding.Empty };
            var dropDown = new ToolStripDropDown { Padding = new Padding(1), BackColor = AppTheme.Border };
            dropDown.Items.Add(host);
            btnFont.DropDown = dropDown;

            popupList.SelectedIndexChanged += (s, e) =>
            {
                if (popupList.SelectedItem == null) return;
                btnFont.Text = popupList.SelectedItem.ToString();
                dropDown.Close();
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb == null || !mainForm.tabManager.IsFormattedMode(rtb)) return;
                System.Drawing.Font currentFont = rtb.SelectionFont ?? rtb.Font;
                rtb.SelectionFont = new System.Drawing.Font(btnFont.Text, currentFont.Size, currentFont.Style);
                rtb.Focus();
            };

            return btnFont;
        }

        public static ToolStripItem BuildFontSizeDropdown(Form1 mainForm)
        {
            var btnSize = new ToolStripDropDownButton
            {
                Text         = "12",
                ToolTipText  = "Розмір шрифту",
                AutoSize     = false,
                Width        = 52,
                BackColor    = AppTheme.Surface,
                ForeColor    = AppTheme.TextPrimary,
                Font         = new System.Drawing.Font("Segoe UI", 9.5f),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            var popupList = new ListBox
            {
                BackColor      = AppTheme.Surface,
                ForeColor      = AppTheme.TextPrimary,
                BorderStyle    = BorderStyle.None,
                IntegralHeight = false,
                Height         = 200,
                Width          = 60,
                Font           = new System.Drawing.Font("Segoe UI", 9.5f)
            };
            popupList.HandleCreated += (s, e) => Theme.WindowsTheme.ApplyDarkThemeToScrollbars(popupList.Handle);

            string[] sizes = { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };
            popupList.Items.AddRange(sizes);
            popupList.SelectedItem = "12";

            var host = new ToolStripControlHost(popupList) { Margin = Padding.Empty, Padding = Padding.Empty };
            var dropDown = new ToolStripDropDown { Padding = new Padding(1), BackColor = AppTheme.Border };
            dropDown.Items.Add(host);
            btnSize.DropDown = dropDown;

            popupList.SelectedIndexChanged += (s, e) =>
            {
                if (popupList.SelectedItem == null) return;
                btnSize.Text = popupList.SelectedItem.ToString();
                dropDown.Close();
                ApplyFontSize(mainForm, btnSize.Text);
                mainForm.tabManager.CurrentEditor?.Focus();
            };

            return btnSize;
        }

        private static void ApplyFontSize(Form1 mainForm, string sizeText)
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb == null || !mainForm.tabManager.IsFormattedMode(rtb)) return;
            
            if (float.TryParse(sizeText, out float size))
            {
                if (size > 0 && size < 1600) 
                {
                    System.Drawing.Font currentFont = rtb.SelectionFont ?? rtb.Font;
                    rtb.SelectionFont = new System.Drawing.Font(currentFont.FontFamily, size, currentFont.Style);
                }
            }
        }
    }
}
