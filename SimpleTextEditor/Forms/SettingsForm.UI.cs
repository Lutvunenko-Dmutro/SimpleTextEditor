using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor.UI
{
    public partial class SettingsForm
    {
        private void InitializeComponent()
        {
            this.Text = "Налаштування";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = AppTheme.Background;
            this.ForeColor = AppTheme.TextPrimary;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            var titleLbl = new Label
            {
                Text = "Налаштування",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(30, 20)
            };
            this.Controls.Add(titleLbl);

            var mainPanel = new FlowLayoutPanel
            {
                Location = new Point(30, 80),
                Size = new Size(620, 460),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };
            mainPanel.HandleCreated += (s, e) => WindowsTheme.ApplyDarkThemeToScrollbars(mainPanel.Handle);
            this.Controls.Add(mainPanel);

            // ── Section: Text Formatting (Шрифт) ──
            mainPanel.Controls.Add(CreateSectionHeader("Форматування тексту"));

            var fontPanel = new Panel { Width = 600, Height = 240, BackColor = AppTheme.Surface, Margin = new Padding(0, 5, 0, 5) };
            mainPanel.Controls.Add(fontPanel);

            var lblFont = new Label { Text = "Шрифт", Font = new Font("Segoe UI", 11f), AutoSize = true, Location = new Point(20, 20) };
            fontPanel.Controls.Add(lblFont);

            cbFamily = CreateComboBox(330, 50, 250);
            cbStyle  = CreateComboBox(330, 90, 250);
            cbSize   = CreateComboBox(330, 130, 250);
            
            fontPanel.Controls.Add(CreateRowLabel("Родина", 20, 53));
            fontPanel.Controls.Add(cbFamily);
            fontPanel.Controls.Add(CreateRowLabel("Стиль", 20, 93));
            fontPanel.Controls.Add(cbStyle);
            fontPanel.Controls.Add(CreateRowLabel("Розмір", 20, 133));
            fontPanel.Controls.Add(cbSize);

            lblPreview = new Label { Text = "чує океану заспокоює мою душу.", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Location = new Point(20, 170), Size = new Size(560, 60), ForeColor = AppTheme.TextPrimary };
            fontPanel.Controls.Add(lblPreview);

            // Word Wrap
            var pnlWrap = CreateTogglePanel("Перенос по словах", "Припасовувати текст у вікні за замовчуванням");
            var swWrap = (ToggleSwitch)pnlWrap.Controls["toggle"];
            swWrap.Checked = _editor != null ? _editor.WordWrap : true;
            swWrap.CheckedChanged += (s, e) => { if (_editor != null) _editor.WordWrap = swWrap.Checked; };
            mainPanel.Controls.Add(pnlWrap);

            // Formatting
            var pnlFmt = CreateTogglePanel("Форматування", null);
            ((ToggleSwitch)pnlFmt.Controls["toggle"]).Checked = true;
            mainPanel.Controls.Add(pnlFmt);

            // ── Section: Startup (Відкриття блокнота) ──
            mainPanel.Controls.Add(CreateSectionHeader("Відкриття блокнота"));
            
            var pnlOpenFiles = CreateComboPanel("Відкриття файлів", "Виберіть розташування для відкриття файлів", new[] { "Відкрити на новій вкладці", "Відкрити у новому вікні" });
            mainPanel.Controls.Add(pnlOpenFiles);

            var pnlStartup = CreateComboPanel("Під час запуску Блокнота", null, new[] { "Відновити попередній сеанс", "Відкрити нове вікно" });
            mainPanel.Controls.Add(pnlStartup);

            var pnlRecent = CreateTogglePanel("Останні файли", null);
            ((ToggleSwitch)pnlRecent.Controls["toggle"]).Checked = true;
            mainPanel.Controls.Add(pnlRecent);

            // ── Section: Spellcheck (Правопис) ──
            mainPanel.Controls.Add(CreateSectionHeader("Правопис"));

            var pnlSpell = CreateTogglePanel("Перевірка правопису", null);
            ((ToggleSwitch)pnlSpell.Controls["toggle"]).Checked = true;
            mainPanel.Controls.Add(pnlSpell);

            var pnlAuto = CreateTogglePanel("Автоматичне виправлення", "Якщо ввімкнуто перевірку орфографії, помилки виправляються автоматично");
            ((ToggleSwitch)pnlAuto.Controls["toggle"]).Checked = true;
            mainPanel.Controls.Add(pnlAuto);

            // ── Section: Additional (Додаткові функції) ──
            mainPanel.Controls.Add(CreateSectionHeader("Додаткові функції"));

            var pnlWrite = CreateTogglePanel("Інструменти для написання", null);
            ((ToggleSwitch)pnlWrite.Controls["toggle"]).Checked = true;
            mainPanel.Controls.Add(pnlWrite);
        }

        private Label CreateSectionHeader(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = AppTheme.TextSecondary,
                AutoSize = true,
                Margin = new Padding(0, 15, 0, 5)
            };
        }

        private Label CreateRowLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10f),
                ForeColor = AppTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private SimpleTextEditor.Controls.DarkComboBox CreateComboBox(int x, int y, int width)
        {
            var cb = new SimpleTextEditor.Controls.DarkComboBox
            {
                Location = new Point(x, y),
                Width = width
            };
            return cb;
        }
        
        private Panel CreateTogglePanel(string title, string subtitle)
        {
            var pnl = new Panel { Width = 600, Height = string.IsNullOrEmpty(subtitle) ? 50 : 60, BackColor = AppTheme.Surface, Margin = new Padding(0, 0, 0, 5) };
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 11f), AutoSize = true, Location = new Point(20, 10), ForeColor = AppTheme.TextPrimary };
            pnl.Controls.Add(lblTitle);

            if (!string.IsNullOrEmpty(subtitle))
            {
                var lblSub = new Label { Text = subtitle, Font = new Font("Segoe UI", 8.5f), ForeColor = AppTheme.TextMuted, AutoSize = true, Location = new Point(20, 32) };
                pnl.Controls.Add(lblSub);
            }

            var sw = new ToggleSwitch { Name = "toggle", Location = new Point(540, string.IsNullOrEmpty(subtitle) ? 15 : 20) };
            pnl.Controls.Add(sw);
            return pnl;
        }

        private Panel CreateComboPanel(string title, string subtitle, string[] options)
        {
            var pnl = new Panel { Width = 600, Height = string.IsNullOrEmpty(subtitle) ? 50 : 60, BackColor = AppTheme.Surface, Margin = new Padding(0, 0, 0, 5) };
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 11f), AutoSize = true, Location = new Point(20, 10), ForeColor = AppTheme.TextPrimary };
            pnl.Controls.Add(lblTitle);

            if (!string.IsNullOrEmpty(subtitle))
            {
                var lblSub = new Label { Text = subtitle, Font = new Font("Segoe UI", 8.5f), ForeColor = AppTheme.TextMuted, AutoSize = true, Location = new Point(20, 32) };
                pnl.Controls.Add(lblSub);
            }

            var cb = CreateComboBox(330, string.IsNullOrEmpty(subtitle) ? 10 : 15, 250);
            cb.Name = "combo";
            cb.Items.AddRange(options);
            cb.SelectedIndex = 0;
            pnl.Controls.Add(cb);
            return pnl;
        }
    }
}
