using Markdown.Enums;

namespace Markdown.Interfaces;

public interface IRenderer
{
    string RenderedMdText { get; }

    bool RenderMarkdown(List<TokenType> tokens);
}