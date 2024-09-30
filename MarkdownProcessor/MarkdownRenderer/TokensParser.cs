using System.Text;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;
using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer
{
    public class TokensParser : ITokensParser
    {
        public IDictionary<string, Tag> TagsDictionary { get; set; }
        private Stack<TagPosition>? TagPositionsStack { get; set; }
        private List<Token> _tokens;

        public TokensParser(IDictionary<string, Tag> tagsDictionary)
        {
            TagsDictionary = tagsDictionary;
            TagPositionsStack = new Stack<TagPosition>();
            _tokens = new List<Token>();
        }

        public IEnumerable<Token> ParseTokens(string unprocessedText)
        {
            bool isEscaped = false;

            for (int i = 0; i < unprocessedText.Length; i++)
            {
                var result = ProcessCharacter(unprocessedText, ref i, ref isEscaped);
                if (result.HasValue)
                {
                    var (currentSymbol, isValid) = result.Value;

                    if (!isValid)
                        continue;

                    var currentTag = TagsDictionary[currentSymbol];

                    if (TagPositionsStack.TryPeek(out var tagFromStack) && tagFromStack.Tag.MarkdownSymbol == currentSymbol)
                    {
                        var openingTag = TagPositionsStack.Pop();
                        _tokens.Add(new Token(openingTag.Tag, openingTag.StartIndex, i));
                    }
                    else
                    {
                        TagPositionsStack.Push(new TagPosition(currentTag, i));
                    }
                }
            }

            return _tokens;
        }

        private (string currentSymbol, bool isValid)? ProcessCharacter(string text, ref int index, ref bool isEscaped)
        {
            if (text[index] == '\\')
            {
                isEscaped = !isEscaped;
                return null;
            }

            string currentSymbol = HandleMarkdownSymbols(text[index].ToString(), index, text);

            if (isEscaped || !CheckIsTag(currentSymbol))
            {
                isEscaped = false;
                return null;
            }

            return (currentSymbol, true);
        }

        private string HandleMarkdownSymbols(string symbol, int index, string unprocessedText)
        {
            if (symbol == "_" && index < unprocessedText.Length - 1 && unprocessedText[index + 1] == '_')
            {  
                return "__"; 
            }

            return unprocessedText[index].ToString();
        }

        private bool CheckIsTag(string symbol)
        {
            return TagsDictionary.ContainsKey(symbol);
        }
    }
}
