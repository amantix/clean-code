using MarkdownProccesor.Renderer.Abstract;
using MarkdownProccesor.Tokens.Abstract;
using System.Text;

namespace MarkdownProccesor.Renderer;

internal sealed class MarkdownToHtmlRenderer : IMarkdownRenderer<List<Token>>
{
    public string RenderMarkdwonText(List<Token> tokens)
    {
        //простой алгорит-заглушка
        StringBuilder htmlText = new StringBuilder();
        foreach (var token in tokens)
        {
            htmlText.Append(token.TagValue);
        }
        return htmlText.ToString();
    }
}
