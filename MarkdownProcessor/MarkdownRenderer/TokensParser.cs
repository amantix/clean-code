using System;
using System.Reflection;
using System.Text;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;
using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer
{
    public class TokensParser : ITokensParser
    {
        public IDictionary<string, Tag> TagsDictionary { get; set; }
        private Stack<TagPosition> _tagPositionsStack = new Stack<TagPosition>();
        private List<Token> _tokens = new List<Token>();

        public TokensParser(IDictionary<string, Tag> tagsDictionary)
        {
            TagsDictionary = tagsDictionary;
        }

        public IEnumerable<Token> ParseTokens(string unprocessedText)
        {
            for (int i = 0; i < unprocessedText.Length; i++)
            {
                string currentSymbol = HandleMarkdownSymbols(i, unprocessedText);

                int offset = CountOffset(currentSymbol);

                if (!CheckIsTag(currentSymbol))
                {
                    i += offset;
                    continue;
                }

                var currentTag = TagsDictionary[currentSymbol];

                if (CheckTagCompatibility(currentSymbol, i, unprocessedText))
                {
                    if (CheckIfClosingTag(currentSymbol))
                    {
                        AddToken(i, currentTag);
                    }
                    else
                    {
                        PushTagPosition(i, currentTag);
                    }
                }

                i += offset;
            }

            return _tokens;
        }

        private void AddToken(int index, Tag currentTag)
        {
            var openingTag = _tagPositionsStack.Pop();
            _tokens.Add(new Token(openingTag.Tag,
                openingTag.StartIndex, index, currentTag.TagType));
        }

        private void PushTagPosition(int index, Tag currentTag)
        {
            var currentTagPosition = new TagPosition(currentTag, index, currentTag.TagType);
            _tagPositionsStack.Push(currentTagPosition);
        }

        private bool CheckIfClosingTag(string mdSymbol)
        {
            if (_tagPositionsStack.TryPeek(out var previousTagPosition) && previousTagPosition.Tag.MarkdownSymbol == mdSymbol)
            {
                return true;
            }

            return false;
        }

        private bool CheckTagCompatibility(string mdSymbol, int index, string unprocessedText)
        {
            bool isPossibleAdd = CheckPreviousTagCompatibility(mdSymbol)
                                 && CheckForSpacesAfter(mdSymbol, index, unprocessedText)
                                 && CheckForSpacesBefore(mdSymbol, index, unprocessedText)
                                 && CheckForWithinNumbers(unprocessedText, index);

            return isPossibleAdd;
        }

        private bool CheckForWithinNumbers(string unprocessedText, int index)
        {
            bool isSurroundedByNumbers = false;

            return (index > 0 && char.IsDigit(unprocessedText[index - 1])) &&
                   (index < unprocessedText.Length - 1 && char.IsDigit(unprocessedText[index + 1]));
        }

        private bool CheckForSpacesBefore(string mdSymbol, int index, string unprocessedText)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (index > 0 && unprocessedText[index - 1] == ' ')
            {
                // Если тег первый, то все равно что перед ним было, значит он точно открывающийся 
                if (isFirstTag)
                {
                    return true;
                }

                if (previousTagPosition.Tag.MarkdownSymbol == mdSymbol)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckForSpacesAfter(string mdSymbol, int index, string unprocessedText)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (index < unprocessedText.Length - 1 && unprocessedText[index + 1] == ' ')
            {
                // Если даже это первый тег, то после него все равно не должно быть никаких пробелов, так как он сто процентов откывающийся
                if (isFirstTag)
                {
                    return false;
                }

                if (previousTagPosition.Tag.MarkdownSymbol != mdSymbol)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckPreviousTagCompatibility(string mdSymbol)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (mdSymbol == "__")
            {
                if (isFirstTag)
                {
                    return true;
                }

                if (previousTagPosition.Tag.MarkdownSymbol == "_")
                {
                    return false;
                }
            }

            return true;
        }

        private int CountOffset(string currentSymbol)
        {
            int offset = currentSymbol.Length - 1;

            return offset;
        }

        private string HandleMarkdownSymbols(int index, string unprocessedText)
        {
            if (unprocessedText[index].ToString() == "_" && index < unprocessedText.Length - 1
                                                         && unprocessedText[index + 1] == '_')
                return "__";

            if (unprocessedText[index].ToString() == "\\" && index > 0
                                                          && unprocessedText[index - 1] == '\\')
                return "\\";

            if (unprocessedText[index].ToString() == "\\" && index < unprocessedText.Length - 1
                                                          && unprocessedText[index + 1] == '_')
                return "\\_";

            if (unprocessedText[index].ToString() == "\\" && index < unprocessedText.Length - 2
                                                          && unprocessedText[index + 1] == '_' && unprocessedText[index + 2] == '_')
                return "\\__";

            return unprocessedText[index].ToString();
        }

        private bool CheckIsTag(string symbol)
        {
            return TagsDictionary.ContainsKey(symbol);
        }
    }
}
