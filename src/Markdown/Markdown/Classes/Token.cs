using Markdown.Enums;

namespace Markdown.Classes;

public struct Token
{
    public readonly TokenType type;
    public string Content { get; set; }
}