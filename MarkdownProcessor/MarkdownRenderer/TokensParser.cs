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

                var tagType = HandleMarkdownSymbol(word, i);

                if (tagType is TagType.NotTag)
                    continue;

                var tagState = DetermineTagState(word, tagType, i);

                if (tagState is TagState.NotTag)
                    continue;

                bool isPossibleToAddTag = HandleTagStack(tagType, tagState, i);

                if (isPossibleToAddTag)
                {
                    currentToken.TagPositions.Add(_tagPositionsStack.Peek());
                }

                if (tagType is TagType.BoldTag)
                {
                    i++;
                }
            }

            return currentToken;
        }

        private TagType HandleMarkdownSymbol(string word, int index)
        {
            if (word[index] == '_')
            {
                if (index < word.Length - 1 && word[index + 1] == '_')
                {   
                    return TagType.BoldTag;
                }

                return TagType.ItalicTag;
            }

            return TagType.NotTag;
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
                    var tempTagPosition = tempStack.Pop();

                    if (tagType == TagType.ItalicTag 
                        && tempTagPosition.TagType == TagType.BoldTag
                        && tempTagPosition.TagState == TagState.Close)
                    {
                        tempTagPosition.TagPair.TagState = TagState.NotTag;
                        tempTagPosition.TagState = TagState.NotTag;
                    }

                    _tagPositionsStack.Push(tempTagPosition);
                }

                if (matchingOpenTag is null)
                {
                    return false;
                }

                matchingOpenTag.TagState = TagState.Open;
                var tagPosition = new TagPosition(tagType, TagState.Close, tagIndex);

                tagPosition.TagPair = matchingOpenTag;
                matchingOpenTag.TagPair = tagPosition;

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
