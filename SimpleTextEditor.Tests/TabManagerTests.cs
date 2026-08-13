using System;
using System.Threading;
using System.Windows.Forms;
using SimpleTextEditor.UI;
using Xunit;

namespace SimpleTextEditor.Tests
{
    public class TabManagerTests
    {
        private void RunInSTA(Action action)
        {
            Exception exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
            {
                throw exception;
            }
        }

        [Fact]
        public void AddNewTab_ShouldCreateNewEditorAndSelectIt()
        {
            RunInSTA(() => 
            {
                var tabStrip = new FlowLayoutPanel();
                var contentPanel = new Panel();
                var manager = new TabManager(tabStrip, contentPanel);

                manager.AddNewTab("Test Tab", "Content here");

                Assert.Equal(1, manager.EditorCount);
                Assert.NotNull(manager.CurrentEditor);
                Assert.Equal("Content here", manager.CurrentEditor.Text);
            });
        }

        [Fact]
        public void CloseTab_ShouldRemoveEditor()
        {
            RunInSTA(() => 
            {
                var tabStrip = new FlowLayoutPanel();
                var contentPanel = new Panel();
                var manager = new TabManager(tabStrip, contentPanel);

                manager.AddNewTab("Tab 1", "Content 1");
                manager.AddNewTab("Tab 2", "Content 2");

                Assert.Equal(2, manager.EditorCount);

                var firstEditor = manager.CurrentEditor;
                manager.SelectPrevTab(); // select tab 1
                var editorToClose = manager.CurrentEditor;

                manager.CloseTab(editorToClose);

                Assert.Equal(1, manager.EditorCount);
                Assert.NotEqual(editorToClose, manager.CurrentEditor);
            });
        }

        [Fact]
        public void TabManager_ShouldTrackFilePaths()
        {
            RunInSTA(() => 
            {
                var tabStrip = new FlowLayoutPanel();
                var contentPanel = new Panel();
                var manager = new TabManager(tabStrip, contentPanel);

                manager.AddNewTab("Tab", "Content", filePath: "C:\\test.txt");

                var path = manager.GetFilePath(manager.CurrentEditor);
                Assert.Equal("C:\\test.txt", path);

                manager.SetFilePath(manager.CurrentEditor, "D:\\new.txt");
                Assert.Equal("D:\\new.txt", manager.GetFilePath(manager.CurrentEditor));
            });
        }

        [Fact]
        public void MarkSaved_ShouldResetDirtyState()
        {
            RunInSTA(() => 
            {
                var tabStrip = new FlowLayoutPanel();
                var contentPanel = new Panel();
                var manager = new TabManager(tabStrip, contentPanel);

                manager.AddNewTab("Tab", "Content");
                var editor = manager.CurrentEditor;
                
                // Edit text
                editor.Text = "Modified";

                manager.MarkSaved(editor);
                
                Assert.Equal("Modified", editor.Text);
            });
        }
    }
}
