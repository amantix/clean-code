using Markdown.Classes;

namespace Markdown.Interfaces;

public interface IParser
{
    public List<Token> Parse(string text);
}