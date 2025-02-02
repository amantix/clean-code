using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Text;

public class MarkdownLexer : IMarkdownLexer
{
    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        bool isEscaped = false; 

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (isEscaped)
            {
                tokens.Add(new Token(TokenType.Text, c.ToString()));
                isEscaped = false;
                continue;
            }
            if (c == '\\')
            {
                isEscaped = true;
                continue;
            }
            if (c == '#' && (i == 0 || input[i - 1] == '\n'))
            {
                int count = 1;
                int pos = i + 1;
                while (pos < input.Length && input[pos] == '#')
                {
                    count++;
                    pos++;
                }

                if (pos < input.Length && input[pos] == ' ')
                {
                    tokens.Add(new Token(TokenType.HeadingMarker, count.ToString()));
                    i = pos; 
                    continue;
                }
                else
                {
                    string hashes = new string('#', count);
                    tokens.Add(new Token(TokenType.Text, hashes));
                    i = pos - 1;
                    continue;
                }
            }

            if (c == '_' && i + 1 < input.Length && input[i + 1] == '_')
            {
                tokens.Add(new Token(TokenType.DoubleUnderscore, "__"));
                i++; 
                continue;
            }
            if (c == '_')
            {
                tokens.Add(new Token(TokenType.Underscore, "_"));
                continue;
            }
            tokens.Add(new Token(TokenType.Text, c.ToString()));
        }
        return MergeTextTokens(tokens);
    }
    private List<Token> MergeTextTokens(List<Token> tokens)
    {
        var merged = new List<Token>();
        var sb = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Type == TokenType.Text)
            {
                sb.Append(token.Value);
            }
            else
            {
                if (sb.Length > 0)
                {
                    merged.Add(new Token(TokenType.Text, sb.ToString()));
                    sb.Clear();
                }
                merged.Add(token);
            }
        }
        if (sb.Length > 0)
        {
            merged.Add(new Token(TokenType.Text, sb.ToString()));
        }

        return merged;
    }
}

