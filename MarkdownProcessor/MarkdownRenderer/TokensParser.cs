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
                int currentIndex = i;
                var result = ProcessCharacter(unprocessedText, ref i, ref isEscaped);
                if (result.HasValue)
                {
                    var (currentSymbol, isValid) = result.Value;

                    if (!isValid)
                        continue;

                    var currentTag = TagsDictionary[currentSymbol];

                    if (TagPositionsStack.TryPeek(out var tagFromStack) && tagFromStack.Tag.MarkdownSymbol == currentSymbol)
                    {

                        if (i > 0 && !char.IsWhiteSpace(unprocessedText[currentIndex - 1]))
                        {
                            var openingTag = TagPositionsStack.Pop();
                            _tokens.Add(new Token(openingTag.Tag,
                                openingTag.StartIndex, i, currentTag.TagType));
                        }
                    }
                    else
                    {
                        if (TagPositionsStack.Count > 0)
                        {
                            var topTag = TagPositionsStack.Peek().Tag;

                            if (topTag.MarkdownSymbol == "_" && currentSymbol == "__")
                            {
                                continue;
                            }
                        }

                        TagPositionsStack.Push(new TagPosition(currentTag, i, currentTag.TagType));
                    }
                }
            }

            return _tokens;
        }

        private (string currentSymbol, bool isValid)? ProcessCharacter(string text, ref int index, ref bool isEscaped)
        {
            if (text[index] == '\\')
            {
                if (index + 1 < text.Length && (text[index + 1] == '_' || text[index + 1] == '\\'))
                {
                    index++; 
                    isEscaped = true;
                    return (text[index].ToString(), false);
                }
                else
                {
                    isEscaped = false;
                    return null;
                }
            }

            string currentSymbol = HandleMarkdownSymbols(text[index].ToString(), ref index, text);

            if (isEscaped || !CheckIsTag(currentSymbol))
            {
                isEscaped = false;
                return null;
            }

            return (currentSymbol, true);
        }

        private string HandleMarkdownSymbols(string symbol, ref int index, string unprocessedText)
        {
            if (symbol == "_" && index < unprocessedText.Length - 1 && unprocessedText[index + 1] == '_')
            {
                index++; 
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
