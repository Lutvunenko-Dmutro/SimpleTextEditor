using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace SimpleTextEditor
{
    public partial class Form1 : Form
    {
        private string printText;
        public Form1()
        {
            InitializeComponent();
            CreateMainMenu();
        }

        private void CreateMainMenu()
        {
            MenuStrip mainMenu = new MenuStrip();

            // File menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Open");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Save");
            ToolStripMenuItem printMenuItem = new ToolStripMenuItem("Print");  // Додали пункт для друку
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");

            openMenuItem.Click += OpenFile;
            saveMenuItem.Click += SaveFile;
            printMenuItem.Click += PrintFile;  // Додали обробник для друку
            exitMenuItem.Click += ExitApplication;

            fileMenu.DropDownItems.Add(openMenuItem);
            fileMenu.DropDownItems.Add(saveMenuItem);
            fileMenu.DropDownItems.Add(printMenuItem);  // Додали пункт для друку
            fileMenu.DropDownItems.Add(exitMenuItem);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");
            ToolStripMenuItem fontMenuItem = new ToolStripMenuItem("Font");
            ToolStripMenuItem colorMenuItem = new ToolStripMenuItem("Color");

            fontMenuItem.Click += ChangeFont;
            colorMenuItem.Click += ChangeColor;

            editMenu.DropDownItems.Add(fontMenuItem);
            editMenu.DropDownItems.Add(colorMenuItem);
            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");
            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Click += ShowAboutForm;
            helpMenu.DropDownItems.Add(aboutMenuItem);
            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(editMenu);
            mainMenu.Items.Add(helpMenu);

            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    printText = fileContent;
                    richTextBox1.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the file: " + ex.Message);
                }
            }
        }
        private void SaveFile(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the file: " + ex.Message);
                }
            }
        }
        private void ChangeFont(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog.Font;
            }
        }
        private void ChangeColor(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog.Color;
            }
        }
        private void ShowAboutForm(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }
        private void ExitApplication(object sender, EventArgs e)
        {
            if (richTextBox1.Text != printText)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Save Changes", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    SaveFile(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            this.Close();
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem clickedItem = e.ClickedItem;

            if (clickedItem.Text == "Open")
            {
                OpenFile(sender, e);
            }
            else if (clickedItem.Text == "Save")
            {
                SaveFile(sender, e);
            }
            else if (clickedItem.Text == "Font")
            {
                ChangeFont(sender, e);
            }
            else if (clickedItem.Text == "Color")
            {
                ChangeColor(sender, e);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 100);
        }
        private void PrintFile(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;

                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            e.Graphics.DrawString(printText, richTextBox1.Font, Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic);
            e.Graphics.MeasureString(printText, richTextBox1.Font, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
            printText = printText.Substring(charactersOnPage);
            e.HasMorePages = printText.Length > 0;
        }

    }
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Simple Text Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

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
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.Controls.Add(this.statusStrip1);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}