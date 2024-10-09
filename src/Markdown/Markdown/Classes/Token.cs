using Markdown.Enums;

namespace Markdown.Classes;

public struct Token
{
    public readonly TokenType Type;
    public string Content { get; set; }
}