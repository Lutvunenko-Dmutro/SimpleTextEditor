using System.IO;
using SimpleTextEditor.Services;
using Xunit;

namespace SimpleTextEditor.Tests
{
    public class FileHandlerTests
    {
        [Fact]
        public void SaveFile_WithExistingPath_ShouldWriteToDisk()
        {
            string tempPath = Path.GetTempFileName();
            string contentToSave = "Test File Content";

            try
            {
                var resultPath = FileHandler.SaveFile(contentToSave, tempPath);
                
                Assert.Equal(tempPath, resultPath);
                Assert.Equal(contentToSave, File.ReadAllText(tempPath));
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }
    }
}
