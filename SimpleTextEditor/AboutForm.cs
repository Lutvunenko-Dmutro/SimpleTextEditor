using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Theme;

namespace SimpleTextEditor
{
    public partial class AboutForm : Form
    {
        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

        public AboutForm()
        {
            InitializeComponent();
            
            this.Text = "About Simple Text Editor";
            
            // Застосуємо темну тему і сюди для консистентності
            this.BackColor = AppTheme.Background;
            this.ForeColor = AppTheme.TextPrimary;
            
            this.labelAbout.Text = "Simple Text Editor v1.0\n\nLitvinenko Dmitro";
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Theme.WindowsTheme.EnableDarkMode(this.Handle);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // StatusStrip
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();

            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 178);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(400, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.BackColor = AppTheme.Accent;
            this.statusStrip1.ForeColor = AppTheme.TextPrimary;

            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);

            // 
            // AboutForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Name = "AboutForm";
            this.Text = "About Simple Text Editor";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // labelAbout
            this.labelAbout = new System.Windows.Forms.Label();
            this.labelAbout.AutoSize = true;
            this.labelAbout.Location = new System.Drawing.Point(20, 20);
            this.labelAbout.Size = new System.Drawing.Size(300, 100);
            this.labelAbout.TabIndex = 0;
            this.labelAbout.Text = "labelAbout";
            this.labelAbout.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.Controls.Add(this.labelAbout);

            this.Controls.Add(this.statusStrip1);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
