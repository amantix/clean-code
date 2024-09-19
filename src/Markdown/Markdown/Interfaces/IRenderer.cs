using Markdown.Enums;

namespace Markdown.Interfaces;

public interface IRenderer
{
    bool RenderMarkdown(List<TokenType> tokens);
}