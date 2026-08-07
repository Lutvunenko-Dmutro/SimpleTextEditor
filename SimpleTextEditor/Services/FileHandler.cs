using System;
using System.IO;
using System.Windows.Forms;

namespace SimpleTextEditor.Services
{
    public static class FileHandler
    {
        public static (string fileName, string content) OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileContent = File.ReadAllText(openFileDialog.FileName);
                        return (openFileDialog.FileName, fileContent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading the file: " + ex.Message);
                    }
                }
            }
            return (null, null);
        }

        public static string SaveFile(string content, string currentFileName = null)
        {
            if (!string.IsNullOrEmpty(currentFileName) && File.Exists(currentFileName))
            {
                try
                {
                    File.WriteAllText(currentFileName, content);
                    return currentFileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving the file: " + ex.Message);
                }
                return null;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, content);
                        return saveFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving the file: " + ex.Message);
                    }
                }
            }
            return null;
        }
    }
}
