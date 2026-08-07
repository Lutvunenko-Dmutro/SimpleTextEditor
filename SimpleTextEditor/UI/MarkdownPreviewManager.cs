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
                    body {{ background-color: #1E1E1E; color: #CCCCCC; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif; padding: 20px; font-size: 15px; line-height: 1.6; min-height: 100%; cursor: text; }}
                    h1, h2, h3, h4, h5, h6 {{ color: #FFFFFF; font-weight: 600; margin-top: 24px; margin-bottom: 16px; border-bottom: 1px solid #333; padding-bottom: 0.3em; }}
                    h1 {{ font-size: 2em; }}
                    h2 {{ font-size: 1.5em; }}
                    table {{ border-collapse: collapse; width: 100%; margin-bottom: 16px; background-color: #1E1E1E; border-radius: 6px; overflow: hidden; }}
                    th, td {{ border: 1px solid #444; padding: 10px 13px; text-align: left; }}
                    th {{ background-color: #252526; font-weight: 600; color: #FFF; }}
                    tr:nth-child(even) {{ background-color: #252526; }}
                    a {{ color: #3794FF; text-decoration: none; }}
                    a:hover {{ text-decoration: underline; }}
                    pre {{ background: #1E1E1E; border: 1px solid #333; padding: 16px; border-radius: 6px; overflow-x: auto; line-height: 1.45; }}
                    code {{ background: #2D2D30; padding: 0.2em 0.4em; border-radius: 6px; font-family: Consolas, 'Courier New', monospace; font-size: 85%; }}
                    pre code {{ background: transparent; padding: 0; font-size: 100%; color: #DCDCAA; }}
                    blockquote {{ border-left: 4px solid #007ACC; margin: 0 0 16px 0; padding: 0 15px; color: #8B949E; }}
                    ul, ol {{ padding-left: 2em; margin-top: 0; margin-bottom: 16px; }}
                    img {{ max-width: 100%; box-sizing: content-box; background-color: #1E1E1E; }}
                    hr {{ height: 0.25em; padding: 0; margin: 24px 0; background-color: #333; border: 0; }}
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
