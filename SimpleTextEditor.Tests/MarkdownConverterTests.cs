using Xunit;
using Markdig;
using ReverseMarkdown;

namespace SimpleTextEditor.Tests
{
    public class MarkdownConverterTests
    {
        [Fact]
        public void MarkdownToHtml_ShouldConvertBold()
        {
            string markdown = "**Hello**";
            var pipeline = new MarkdownPipelineBuilder().Build();
            string html = Markdown.ToHtml(markdown, pipeline).Trim();

            Assert.Equal("<p><strong>Hello</strong></p>", html);
        }

        [Fact]
        public void HtmlToMarkdown_ShouldConvertStrong()
        {
            string html = "<strong>Hello</strong>";
            var converter = new Converter();
            string markdown = converter.Convert(html).Trim();

            Assert.Equal("**Hello**", markdown);
        }
    }
}
