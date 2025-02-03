using Markdown;

namespace WebAppMarkdown.Services;

public class MarkdownConverterService
{
    private readonly MarkdownToHTML _markdownProcessor;

    public MarkdownConverterService(MarkdownToHTML markdownProcessor)
    {
        _markdownProcessor = markdownProcessor;
    }

    public async Task<string> ConvertMarkdownToHtml(string markdownText)
    {
        return _markdownProcessor.ConvertMarkdownToHtml(markdownText);
    }
}