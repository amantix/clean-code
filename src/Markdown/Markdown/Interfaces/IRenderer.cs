using Markdown.Enums;

namespace Markdown.Interfaces;

public interface IRenderer
{
    string RenderMarkdown(List<TokenType> tokens);
}