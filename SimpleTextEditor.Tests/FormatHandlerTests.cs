using System;
using System.Drawing;
using System.Windows.Forms;
using SimpleTextEditor.Services;
using Xunit;

namespace SimpleTextEditor.Tests
{
    public class FormatHandlerTests
    {
        [Fact]
        public void ToggleBold_ShouldToggleBoldStyle()
        {
            var rtb = new RichTextBox();
            rtb.Text = "Hello World";
            rtb.SelectAll();
            rtb.SelectionFont = new Font("Arial", 12, FontStyle.Regular);

            var handler = new FormatHandler();
            handler.ToggleBold(rtb);

            Assert.True(rtb.SelectionFont.Bold);

            // Toggle again should remove bold
            handler.ToggleBold(rtb);
            Assert.False(rtb.SelectionFont.Bold);
        }

        [Fact]
        public void ToggleItalic_ShouldToggleItalicStyle()
        {
            var rtb = new RichTextBox();
            rtb.Text = "Test";
            rtb.SelectAll();
            rtb.SelectionFont = new Font("Arial", 12, FontStyle.Regular);

            var handler = new FormatHandler();
            handler.ToggleItalic(rtb);

            Assert.True(rtb.SelectionFont.Italic);
        }

        [Fact]
        public void InsertHeading_ShouldSetCorrectFontSize()
        {
            var rtb = new RichTextBox();
            rtb.Text = "Heading Test";
            rtb.SelectAll();
            rtb.SelectionFont = new Font("Arial", 12, FontStyle.Regular);

            var handler = new FormatHandler();
            handler.InsertHeading(rtb, 1);

            Assert.Equal(26f, rtb.SelectionFont.Size);
            Assert.True(rtb.SelectionFont.Bold);
        }

        [Fact]
        public void ToggleBulletList_ShouldToggleBullet()
        {
            var rtb = new RichTextBox();
            rtb.Text = "Item 1";
            rtb.SelectAll();
            Assert.False(rtb.SelectionBullet);

            var handler = new FormatHandler();
            handler.ToggleBulletList(rtb);

            Assert.True(rtb.SelectionBullet);
        }

        [Fact]
        public void InsertCodeBlock_ShouldSetConsolasFontAndBackColor()
        {
            var rtb = new RichTextBox();
            rtb.Text = "int x = 5;";
            rtb.SelectAll();
            rtb.SelectionFont = new Font("Arial", 12, FontStyle.Regular);

            var handler = new FormatHandler();
            handler.InsertCodeBlock(rtb);

            Assert.Equal("Consolas", rtb.SelectionFont.FontFamily.Name);
            Assert.Equal(Color.FromArgb(40, 40, 40), rtb.SelectionBackColor);
        }
    }
}
