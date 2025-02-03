namespace Markdown;

public class MarkdownToHTML
{
    public string ConvertMarkdownToHtml(string input)
    {
        TokenRenderer mdToHtmlRenderer = new TokenRenderer();
        return mdToHtmlRenderer.Render(input);
    }
}
