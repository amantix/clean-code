using MarkdownProccesor.Lexer.Abstract;
using MarkdownProccesor.Tokens.Abstract;
using MarkdownProccesor.Tokens.Types;

namespace MarkdownProccesor.Lexer;

internal class MarkdownLexer : ILexer<List<Token>>
{
    public List<Token> Tokenize(string markdownText)
    {
        //заглушка
        return new List<Token>{
            new HeaderToken(2,true),new TextToken("Заголовок"), new HeaderToken(2,false)
        };
    }
}