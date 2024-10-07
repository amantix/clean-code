using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
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
            string[] words = unprocessedText.Split(' ');

            foreach (string word in words)
            {
                var token = ProcessWord(word);
                _tokens.Add(token);
            }

            return _tokens;
        }

        private Token ProcessWord(string word)
        {
            var currentToken = new Token(word);
            for (int i = 0; i < word.Length; i++)
            {
                // TODO: Добавить проверку на наличие цифр в слове

                char currentChar = word[i];

                // TODO: Добавить распознование __
                if (currentChar == '_')
                {
                    var tagType = TagType.ItalicTag;
                    var tagState = DetermineTagState(word, tagType, i);

                    bool res = HandleTagStack(tagType, tagState, i);

                    if (res)
                    {
                        currentToken.TagPositions.Add(_tagPositionsStack.Peek());
                    }
                }
            }

            return currentToken;
        }

        private bool HandleTagStack(TagType tagType, TagState tagState, int tagIndex)
        {
            if (tagState == TagState.TemporarilyOpen)
            {
                var tagPosition = new TagPosition(tagType, tagState, tagIndex);
                _tagPositionsStack.Push(tagPosition);

                return true;
            }

            if (tagState == TagState.TemporarilyClose)
            {
                TagPosition matchingOpenTag = null;
                Stack<TagPosition> tempStack = new Stack<TagPosition>();

                while (_tagPositionsStack.Count > 0)
                {
                    var previousTagPosition = _tagPositionsStack.Pop();

                    if (previousTagPosition.TagType == tagType && previousTagPosition.TagState == TagState.TemporarilyOpen)
                    {
                        matchingOpenTag = previousTagPosition;
                        break;
                    }

                    tempStack.Push(previousTagPosition);
                }

                while (tempStack.Count > 0)
                {
                    _tagPositionsStack.Push(tempStack.Pop());
                }

                if (matchingOpenTag is null)
                {
                    return false;
                }

                matchingOpenTag.TagState = TagState.Open;
                var tagPosition = new TagPosition(tagType, TagState.Close, tagIndex);
                _tagPositionsStack.Push(tagPosition);

                return true;
            }

            return false;
        }

        private TagState DetermineTagState(string word, TagType tagType, int symbolIndex)
        {
            if (symbolIndex == 0)
            {
                return TagState.TemporarilyOpen;
            }

            if (tagType == TagType.ItalicTag && symbolIndex == word.Length - 1)
            {
                return TagState.TemporarilyClose;
            }

            if (tagType == TagType.BoldTag && symbolIndex == word.Length - 2)
            {
                return TagState.TemporarilyClose;
            }

            return TagState.NotTag;
        }
    }
}
