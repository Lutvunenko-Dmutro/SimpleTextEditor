using System.Collections.Generic;
using System.Windows.Forms;
using Markdig;
using ReverseMarkdown;

namespace SimpleTextEditor.UI
{
    public class MarkdownPreviewManager
    {
        public WebBrowser Previewer { get; private set; }
        private Dictionary<RichTextBox, bool> editorPreviewState = new Dictionary<RichTextBox, bool>();

        public MarkdownPreviewManager(Panel contentPanel)
        {
            Previewer = new WebBrowser();
            Previewer.Dock = DockStyle.Fill;
            Previewer.Visible = false;
            Previewer.ScriptErrorsSuppressed = true;
            contentPanel.Controls.Add(Previewer);
        }

        public bool IsPreviewMode(RichTextBox rtb) => editorPreviewState.GetValueOrDefault(rtb, false);

        public void SetPreviewMode(RichTextBox rtb, bool isPreview, RichTextBox currentEditor)
        {
            if (rtb == null) return;
            
            bool wasPreview = editorPreviewState.GetValueOrDefault(rtb, false);
            
            if (wasPreview && !isPreview)
            {
                SyncPreviewToEditor(rtb);
            }
            
            editorPreviewState[rtb] = isPreview;
            if (rtb == currentEditor)
            {
                ShowPreviewIfActive(rtb);
            }
        }

        public void ShowPreviewIfActive(RichTextBox rtb)
        {
            if (IsPreviewMode(rtb))
            {
                UpdatePreview(rtb);
            }
            else
            {
                Previewer.Visible = false;
            }
        }

        public void SyncPreviewToEditor(RichTextBox rtb)
        {
            if (IsPreviewMode(rtb) && Previewer.Document?.Body != null)
            {
                try
                {
                    var html = Previewer.Document.Body.InnerHtml;
                    if (!string.IsNullOrEmpty(html))
                    {
                        var config = new ReverseMarkdown.Config
                        {
                            GithubFlavored = true
                        };
                        // Note: In newer versions of ReverseMarkdown, we can omit these or use the appropriate namespaces, 
                        // but setting GithubFlavored = true is usually enough for standard Markdown conversion.
                        var converter = new ReverseMarkdown.Converter(config);
                        string md = converter.Convert(html);
                        
                        if (rtb.Text != md) {
                            rtb.Text = md;
                        }
                    }
                }
                catch { }
            }
        }

        private void UpdatePreview(RichTextBox rtb)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string html = Markdig.Markdown.ToHtml(rtb.Text, pipeline);
            
            string styledHtml = $@"
            <html>
            <head>
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                <style>
                    html, body {{ height: 100%; margin: 0; }}
                    body {{ background-color: #0D0D12; color: #E6E4FA; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; padding: 32px 40px; font-size: 15px; line-height: 1.7; min-height: 100%; cursor: text; }}
                    h1, h2, h3, h4, h5, h6 {{ color: #F0EEFF; font-weight: 600; margin-top: 28px; margin-bottom: 14px; border-bottom: 1px solid #2A2846; padding-bottom: 0.35em; }}
                    h1 {{ font-size: 2.1em; }}
                    h2 {{ font-size: 1.55em; }}
                    h3 {{ font-size: 1.25em; }}
                    table {{ border-collapse: collapse; width: 100%; margin-bottom: 20px; border-radius: 8px; overflow: hidden; border: 1px solid #2A2846; }}
                    th, td {{ border: 1px solid #2A2846; padding: 10px 14px; text-align: left; }}
                    th {{ background-color: #20203050; font-weight: 600; color: #A78BFA; }}
                    tr:nth-child(even) {{ background-color: #16162060; }}
                    a {{ color: #A78BFA; text-decoration: none; border-bottom: 1px dotted #7C4DFF; }}
                    a:hover {{ color: #C4B5FD; border-bottom: 1px solid #A78BFA; }}
                    pre {{ background: #16162050; border: 1px solid #2A2846; padding: 18px; border-radius: 10px; overflow-x: auto; line-height: 1.5; }}
                    code {{ background: #20203070; padding: 0.2em 0.45em; border-radius: 5px; font-family: 'Cascadia Code', Consolas, 'Courier New', monospace; font-size: 86%; color: #C4B5FD; }}
                    pre code {{ background: transparent; padding: 0; font-size: 100%; color: #E4D7FF; }}
                    blockquote {{ border-left: 4px solid #7C4DFF; margin: 0 0 18px 0; padding: 4px 18px; color: #AC9FD8; background: #1C1A2E; border-radius: 0 8px 8px 0; }}
                    ul, ol {{ padding-left: 2em; margin-top: 0; margin-bottom: 18px; }}
                    img {{ max-width: 100%; box-sizing: content-box; border-radius: 6px; }}
                    hr {{ height: 1px; padding: 0; margin: 28px 0; background-color: #2A2846; border: 0; }}
                </style>
            </head>
            <body contenteditable='true'>
                {html}
                <p><br></p>
            </body>
            </html>";

            Previewer.DocumentText = styledHtml;
            Previewer.Visible = true;
            Previewer.BringToFront();
        }

        public void RemoveEditor(RichTextBox rtb)
        {
            editorPreviewState.Remove(rtb);
        }
    }
}
