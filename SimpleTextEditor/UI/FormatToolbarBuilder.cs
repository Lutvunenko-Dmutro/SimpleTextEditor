using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public static class FormatToolbarBuilder
    {
        public static ToolStrip BuildFormatToolbar(Form1 mainForm)
        {
            var toolbar = new ToolStrip();
            toolbar.BackColor = AppTheme.Surface;
            toolbar.ForeColor = AppTheme.TextPrimary;
            toolbar.GripStyle = ToolStripGripStyle.Hidden;
            toolbar.RenderMode = ToolStripRenderMode.System;
            toolbar.Padding = new Padding(8, 6, 8, 6);
            
            // Apply custom dark renderer
            toolbar.Renderer = new ToolStripProfessionalRenderer(new Theme.DarkColorTable());

            Font textFont = new Font("Segoe UI", 10F);
            Font iconFont = new Font("Segoe UI Emoji", 11F); // Cross-platform modern emoji/symbols

            var btnHeaders = new ToolStripDropDownButton("H1");
            btnHeaders.ForeColor = Color.White;
            btnHeaders.Font = textFont;
            btnHeaders.Padding = new Padding(4);
            btnHeaders.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnHeaders.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Заголовок 1 (H1)", null, (s, e) => InsertText(mainForm, "# ")),
                new ToolStripMenuItem("Заголовок 2 (H2)", null, (s, e) => InsertText(mainForm, "## ")),
                new ToolStripMenuItem("Заголовок 3 (H3)", null, (s, e) => InsertText(mainForm, "### ")),
                new ToolStripMenuItem("Заголовок 4 (H4)", null, (s, e) => InsertText(mainForm, "#### ")),
                new ToolStripMenuItem("Заголовок 5 (H5)", null, (s, e) => InsertText(mainForm, "##### ")),
                new ToolStripMenuItem("Заголовок 6 (H6)", null, (s, e) => InsertText(mainForm, "###### "))
            });

            var btnLists = new ToolStripDropDownButton(" ☰ ");
            btnLists.ForeColor = AppTheme.AccentLight;
            btnLists.Font = iconFont;
            btnLists.Padding = new Padding(2);
            btnLists.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnLists.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Маркований список", null, (s, e) => InsertText(mainForm, "- ")),
                new ToolStripMenuItem("Нумерований список", null, (s, e) => InsertText(mainForm, "1. ")),
                new ToolStripMenuItem("Список завдань", null, (s, e) => InsertText(mainForm, "- [ ] "))
            });

            var btnBold = new ToolStripButton("B", null, (s, e) => InsertText(mainForm, "**", "**"));
            btnBold.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBold.Padding = new Padding(4);
            btnBold.ForeColor = Color.White;

            var btnItalic = new ToolStripButton("I", null, (s, e) => InsertText(mainForm, "_", "_"));
            btnItalic.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            btnItalic.Padding = new Padding(4);
            btnItalic.ForeColor = Color.White;
            
            var btnStrike = new ToolStripButton("S", null, (s, e) => InsertText(mainForm, "~~", "~~"));
            btnStrike.Font = new Font("Segoe UI", 10F, FontStyle.Strikeout);
            btnStrike.Padding = new Padding(4);
            btnStrike.ForeColor = Color.White;

            var btnLink = new ToolStripButton(" 🔗 ", null, (s, e) => InsertText(mainForm, "[", "](url)"));
            btnLink.ForeColor = AppTheme.AccentLight;
            btnLink.Font = iconFont;
            btnLink.Padding = new Padding(2);

            var btnTable = new ToolStripDropDownButton(" ▦ ");
            btnTable.ForeColor = AppTheme.AccentLight;
            btnTable.Font = iconFont;
            btnTable.Padding = new Padding(2);
            btnTable.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnTable.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Вставити таблицю", null, (s, e) => InsertText(mainForm, "| Колон | Колон |\n|---|---|\n| Дані | Дані |")),
                new ToolStripMenuItem("Додати рядок", null, (s, e) => InsertText(mainForm, "\n| Новий | Новий |")),
                new ToolStripMenuItem("Редагувати таблицю >")
            });

            var btnClearFormat = new ToolStripButton(" ⌫ ", null, (s, e) => {
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null)
                {
                    rtb.SelectedText = rtb.SelectedText.Replace("**", "").Replace("_", "").Replace("~~", "");
                }
            });
            btnClearFormat.ForeColor = Color.White;
            btnClearFormat.Font = iconFont;
            btnClearFormat.Padding = new Padding(2);

            // Style dropdown menus
            foreach (ToolStripItem item in new ToolStripItem[] { btnHeaders, btnLists, btnTable })
            {
                if (item is ToolStripDropDownButton dropDown)
                {
                    foreach (ToolStripItem dropItem in dropDown.DropDownItems)
                    {
                        dropItem.BackColor = AppTheme.Surface;
                        dropItem.ForeColor = AppTheme.TextPrimary;
                    }
                }
            }

            var txtSearch = new ToolStripTextBox();
            txtSearch.Alignment = ToolStripItemAlignment.Right;
            txtSearch.BackColor = AppTheme.Background;
            txtSearch.ForeColor = AppTheme.TextSecondary;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Size = new Size(180, 25);
            txtSearch.Font = textFont;
            txtSearch.Margin = new Padding(0, 0, 10, 0);
            txtSearch.ToolTipText = "Пошук...";
            txtSearch.Text = "Search/replace";
            txtSearch.Enter += (s, e) => { if (txtSearch.Text == "Search/replace") txtSearch.Text = ""; };
            txtSearch.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "Search/replace"; };

            var btnFind = new ToolStripButton(" 🔍 ", null, (s, e) => {
                if (txtSearch.Text == "Search/replace" || string.IsNullOrWhiteSpace(txtSearch.Text)) return;
                var rtb = mainForm.tabManager.CurrentEditor;
                if (rtb != null)
                {
                    int startIndex = rtb.SelectionStart + rtb.SelectionLength;
                    int index = rtb.Find(txtSearch.Text, startIndex, RichTextBoxFinds.None);
                    if (index == -1) // wrap around
                    {
                        index = rtb.Find(txtSearch.Text, 0, RichTextBoxFinds.None);
                    }
                    if (index != -1)
                    {
                        rtb.Select(index, txtSearch.Text.Length);
                        rtb.ScrollToCaret();
                        rtb.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Текст не знайдено.", "Пошук", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            });
            btnFind.Alignment = ToolStripItemAlignment.Right;
            btnFind.ForeColor = AppTheme.AccentLight;
            btnFind.Font = iconFont;
            btnFind.Padding = new Padding(2);

            toolbar.Items.AddRange(new ToolStripItem[] {
                btnHeaders, new ToolStripSeparator(),
                btnLists, new ToolStripSeparator(),
                btnBold, btnItalic, btnStrike, btnLink, btnTable, btnClearFormat,
                btnFind, txtSearch
            });

            return toolbar;
        }

        private static void InsertText(Form1 mainForm, string text, string suffix = "")
        {
            var rtb = mainForm.tabManager.CurrentEditor;
            if (rtb != null) {
                rtb.SelectedText = text + rtb.SelectedText + suffix;
            }
        }
    }
}
