using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDprocessor.Elements;
namespace MDprocessor;

using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Text;



public class MarkdownParser : IMarkdownParser
{
    private readonly IMarkdownLexer _lexer;
    private readonly IHtmlRenderer _renderer;

    public MarkdownParser(IMarkdownLexer lexer, IHtmlRenderer renderer)
    {
        _lexer = lexer;
        _renderer = renderer;
    }

    public string RenderToHtml(string markdownText)
    {
        var tokens = _lexer.Tokenize(markdownText);
        int index = 0;
        var elements = ParseElements(tokens, ref index, endMarker: null);
        return _renderer.Render(elements);
    }
    private List<IMarkdownElement> ParseElements(List<Token> tokens, ref int i, TokenType? endMarker)
    {
        var result = new List<IMarkdownElement>();

        while (i < tokens.Count)
        {
            if (endMarker.HasValue && tokens[i].Type == endMarker.Value)
            {
                return result;
            }
            var current = tokens[i];
            if (current.Type == TokenType.HeadingMarker && !endMarker.HasValue)
            {
                int level = 1;
                int.TryParse(current.Value, out level);
                i++;
                var headingChildren = ParseLineAsElements(tokens, ref i);
                result.Add(new HeadingElement(level, headingChildren));
            }
            else if (current.Type == TokenType.DoubleUnderscore)
            {
                i++;
                var children = ParseElements(tokens, ref i, TokenType.DoubleUnderscore);
                if (i < tokens.Count && tokens[i].Type == TokenType.DoubleUnderscore)
                {
                    i++;
                }

                result.Add(new StrongElement(children));
            }
            else if (current.Type == TokenType.Underscore)
            {
                i++;
                var children = ParseElements(tokens, ref i, TokenType.Underscore);

                if (i < tokens.Count && tokens[i].Type == TokenType.Underscore)
                {
                    i++;
                }

                result.Add(new EmphasisElement(children));
            }
            else
            {
                result.Add(new TextElement(current.Value));
                i++;
            }
        }

        return result;
    }
    private List<IMarkdownElement> ParseLineAsElements(List<Token> tokens, ref int i)
    {
        var lineTokens = new List<Token>();
        while (i < tokens.Count)
        {
            if (tokens[i].Type == TokenType.Text && tokens[i].Value.Contains('\n'))
            {
                int idx = tokens[i].Value.IndexOf('\n');
                string beforeNewLine = tokens[i].Value.Substring(0, idx);
                if (!string.IsNullOrEmpty(beforeNewLine))
                {
                    lineTokens.Add(new Token(TokenType.Text, beforeNewLine));
                }
                string afterNewLine = tokens[i].Value.Substring(idx + 1);
                if (!string.IsNullOrEmpty(afterNewLine))
                {
                    tokens[i] = new Token(TokenType.Text, afterNewLine);
                }
                else
                {
                    tokens.RemoveAt(i);
                }
                break;
            }
            else
            {
                lineTokens.Add(tokens[i]);
                i++;
            }
        }
        int localIndex = 0;
        return ParseElements(lineTokens, ref localIndex, null);
    }
}





