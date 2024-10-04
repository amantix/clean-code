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
        public Dictionary<string, bool> _wordChecks = new Dictionary<string, bool>();

        public TokensParser(IDictionary<string, Tag> tagsDictionary)
        {
            TagsDictionary = tagsDictionary;
        }

        public IEnumerable<Token> ParseTokens(string unprocessedText)
        {
            ProcessWords(unprocessedText);

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

        private void ProcessWords(string unprocessedText)
        {
            string[] allWords = unprocessedText.Split(new char[] { ' ', ',', '.' });

            foreach (string word in allWords)
            {
                if (!_wordChecks.ContainsKey(word))
                {
                    bool canBeFormatted = CanBeFormattedWithTags(word);
                    _wordChecks.Add(word, canBeFormatted);
                }
            }

            if (!_wordChecks.ContainsKey(string.Empty))
            {
                _wordChecks.Add(string.Empty, false);
            }
        }

        private bool CanBeFormattedWithTags(string word)
        {
            if (IsWordContainsNumbers(word))
            {
                return false;
            }

            return true;
        }

        private bool IsWordContainsNumbers(string word)
        {
            return word.Any(char.IsDigit);
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


        private bool CheckIsOpenTag(string mdSymbol)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (isFirstTag)
            {
                return true;
            }

            if (previousTagPosition.Tag.MarkdownSymbol == mdSymbol)
            {
                return false;
            }

            return true;
        }

        private bool CheckTagCompatibility(string mdSymbol, int index, string unprocessedText)
        {
            bool isOpenTag = CheckIsOpenTag(mdSymbol);
            bool isPreviousTagCompatible = CheckPreviousTagCompatibility(mdSymbol);
            bool isSpaceAfter = CheckForSpacesAfter(mdSymbol, index, unprocessedText);
            bool isSpaceBefore = CheckForSpacesBefore(mdSymbol, index, unprocessedText);
            bool isEmptyRaw = CheckForEmptyRaw(mdSymbol, index);

            if (isPreviousTagCompatible && isSpaceAfter && isSpaceBefore && !isEmptyRaw)
            {
                if (CheckTagAtWordPhrase(mdSymbol, index, unprocessedText))
                {
                    if (IsValidTagPosition(index, unprocessedText, isOpenTag))
                    {
                        return true;
                    }

                    string currentWord = GetWordForTag(mdSymbol, index, unprocessedText, isOpenTag);

                    if (_wordChecks[currentWord])
                    {
                        return true;
                    }

                    return false;
                }

                if (CheckInSingleWord(mdSymbol, index, unprocessedText))
                {
                    string currentWord = GetWordBetweenTags(mdSymbol, index, unprocessedText);

                    if (_wordChecks[currentWord])
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        private bool IsValidTagPosition(int index, string unprocessedText, bool isOpenTag)
        {
            if (isOpenTag)
            {
                return index == 0 || char.IsWhiteSpace(unprocessedText[index - 1]);
            }

            return index == unprocessedText.Length - 1 || !char.IsWhiteSpace(unprocessedText[index + 1]);
        }

        private string GetWordForTag(string mdSymbol, int index, string unprocessedText, bool isOpenTag)
        {
            int start = index;
            while (start > 0 && !char.IsWhiteSpace(unprocessedText[start - 1]))
            {
                start--;
            }

            int end = index;
            while (end < unprocessedText.Length && !char.IsWhiteSpace(unprocessedText[end]))
            {
                end++;
            }

            return unprocessedText.Substring(start, end - start);
        }

        private string GetWordBetweenTags(string mdSymbol, int end, string unprocessedText)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (isFirstTag)
            {
                return string.Empty;
            }

            int start = previousTagPosition.StartIndex;

            while (start > 0 && !char.IsWhiteSpace(unprocessedText[start - 1]))
            {
                start--;
            }

            while (end < unprocessedText.Length && !char.IsWhiteSpace(unprocessedText[end]))
            {
                end++;
            }

            Console.WriteLine($"слово между символами: {unprocessedText.Substring(start, end - start)}");

            return unprocessedText.Substring(start, end - start);
        }

        private bool CheckForEmptyRaw(string mdSymbol, int index)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (!isFirstTag)
            {
                int offset = CountOffset(previousTagPosition.Tag.MarkdownSymbol);

                if (previousTagPosition.StartIndex == index - 1 - offset && previousTagPosition.Tag.MarkdownSymbol == mdSymbol)
                {
                    _tagPositionsStack.Push(previousTagPosition);
                    return true;
                }
            }

            return false;
        }
        private bool CheckTagAtWordPhrase(string mdSymbol, int index, string unprocessedText)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (!isFirstTag && mdSymbol == previousTagPosition.Tag.MarkdownSymbol)
            {
                int offset = CountOffset(mdSymbol);
                int previousTagIndex = previousTagPosition.StartIndex;

                char charBefore = previousTagIndex > 0
                    ? unprocessedText[previousTagIndex - 1]
                    : ' ';

                char charAfter = index + offset < unprocessedText.Length - 1
                    ? unprocessedText[index + offset + 1]
                    : ' ';

                bool isTagAtWordStart = char.IsWhiteSpace(charBefore);
                bool isTagAtWordEnd = char.IsWhiteSpace(charAfter);

                return isTagAtWordStart && isTagAtWordEnd;
            }

            return true;
        }

        private bool CheckInSingleWord(string mdSymbol, int index, string unprocessedText)
        {
            bool isFirstTag = !(_tagPositionsStack.TryPeek(out var previousTagPosition));

            if (isFirstTag)
            {
                return true;
            }

            if (mdSymbol == previousTagPosition.Tag.MarkdownSymbol)
            {
                var subString = unprocessedText.Substring(previousTagPosition.StartIndex + 1, index - previousTagPosition.StartIndex - 1);

                if (subString.Any(char.IsWhiteSpace))
                {
                    return false;
                }

                return true;
            }

            return false;
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
                                                          && unprocessedText[index + 1] == '_'
                                                          && unprocessedText[index + 2] == '_')
                return "\\__";

            return unprocessedText[index].ToString();
        }

        private bool CheckIsTag(string symbol)
        {
            return TagsDictionary.ContainsKey(symbol);
        }
    }
}
