using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SimpleTextEditor.Services
{
    public class PrintHandler
    {
        private string printText = string.Empty;
        private Font printFont = new Font("Consolas", 11F);

        public void PrintFile(string textToPrint, Font font)
        {
            printText = textToPrint;
            printFont = font;

            using (PrintDialog printDialog = new PrintDialog())
            {
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    using (PrintDocument printDocument = new PrintDocument())
                    {
                        printDocument.PrintPage += PrintDocument_PrintPage;
                        printDocument.Print();
                    }
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (e.Graphics == null || printFont == null || printText == null)
                return;

            int charactersOnPage = 0;
            int linesPerPage = 0;
            
            e.Graphics.DrawString(printText, printFont, Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic);
            e.Graphics.MeasureString(printText, printFont, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
            
            printText = printText.Substring(charactersOnPage);
            e.HasMorePages = printText.Length > 0;
        }
    }
}
