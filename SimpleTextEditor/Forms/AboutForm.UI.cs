using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;
using System.Drawing.Drawing2D;

namespace SimpleTextEditor
{
    public partial class AboutForm
    {
        private void BuildUI()
        {
            // (no drag – this is a fixed modal dialog)

            // ── Close button (top-right) ───────────────────────────────────────────
            var btnClose = new Button
            {
                Text      = "✕",
                Size      = new Size(32, 32),
                Location  = new Point(Width - 42, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.TextMuted,
                Cursor    = Cursors.Hand,
                Font      = new Font("Segoe UI", 10f)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = AppTheme.Error;
            btnClose.Click += (s, e) => Close();
            this.Controls.Add(btnClose);

            // ── App icon / logo area ───────────────────────────────────────────────
            var iconPanel = new Panel
            {
                Size      = new Size(64, 64),
                Location  = new Point(32, 36),
                BackColor = Color.Transparent
            };
            iconPanel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Violet circle background
                using var circleBrush = new LinearGradientBrush(new Rectangle(0, 0, 64, 64),
                    Color.FromArgb(124, 77, 255), Color.FromArgb(91, 33, 182),
                    LinearGradientMode.ForwardDiagonal);
                g.FillEllipse(circleBrush, 0, 0, 63, 63);
                // Pencil emoji text
                using var font = new Font("Segoe UI Emoji", 26f);
                using var brush = new SolidBrush(Color.White);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("✍", font, brush, new RectangleF(0, 0, 64, 64), sf);
            };
            this.Controls.Add(iconPanel);

            // ── App name ──────────────────────────────────────────────────────────
            var lblName = new Label
            {
                Text      = "Simple Text Editor",
                Location  = new Point(112, 40),
                Size      = new Size(280, 36),
                Font      = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = AppTheme.TextPrimary,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblName);

            var lblVersion = new Label
            {
                Text      = "Версія 2.0  ·  .NET 8.0 / WinForms",
                Location  = new Point(114, 78),
                Size      = new Size(280, 20),
                Font      = new Font("Segoe UI", 9f),
                ForeColor = AppTheme.AccentLight,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblVersion);

            // ── Divider ───────────────────────────────────────────────────────────
            var divider = new Panel
            {
                Location  = new Point(32, 118),
                Size      = new Size(Width - 64, 1),
                BackColor = AppTheme.Border
            };
            this.Controls.Add(divider);

            // ── Description ───────────────────────────────────────────────────────
            var lblDesc = new Label
            {
                Text      = "Сучасний, швидкий та мінімалістичний текстовий редактор\nз підтримкою Markdown, вкладок та темної теми.",
                Location  = new Point(32, 132),
                Size      = new Size(Width - 64, 52),
                Font      = new Font("Segoe UI", 10f),
                ForeColor = AppTheme.TextSecondary,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblDesc);

            // ── Feature chips ─────────────────────────────────────────────────────
            string[] chips = { "📑 Вкладки", "Ⓜ️ Markdown", "🔍 Пошук", "🎨 Теми", "🖨️ Друк" };
            int chipX = 32;
            int chipY = 195;
            foreach (var chip in chips)
            {
                var lbl = new Label
                {
                    Text      = chip,
                    AutoSize  = true,
                    Padding   = new Padding(8, 4, 8, 4),
                    Font      = new Font("Segoe UI", 8.5f),
                    ForeColor = AppTheme.AccentLight,
                    BackColor = AppTheme.Surface,
                    Cursor    = Cursors.Default
                };
                lbl.Location = new Point(chipX, chipY);
                this.Controls.Add(lbl);
                chipX += lbl.PreferredWidth + 20;
            }

            // ── Author / footer ───────────────────────────────────────────────────
            var lblAuthor = new Label
            {
                Text      = "Створено Litvinenko Dmytro  ·  2026",
                Location  = new Point(32, 258),
                Size      = new Size(Width - 64, 22),
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = AppTheme.TextMuted,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblAuthor);

            // OK button
            var btnOK = new Button
            {
                Text      = "OK",
                Size      = new Size(70, 28),
                Location  = new Point(Width - 102, 253),
                FlatStyle = FlatStyle.Flat,
                BackColor = AppTheme.Accent,
                ForeColor = Color.White,
                Cursor    = Cursors.Hand,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseOverBackColor = AppTheme.AccentDark;
            btnOK.Click += (s, e) => Close();
            this.Controls.Add(btnOK);


        }
    }
}
