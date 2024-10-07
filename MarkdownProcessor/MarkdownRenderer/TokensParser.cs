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
                var tagType = HandleMarkdownSymbol(word, i);

                if (tagType is TagType.NotTag)
                    continue;

                var tagState = DetermineTagState(word, tagType, i, currentToken);

                if (tagState is TagState.NotTag)
                    continue;

                bool isPossibleToAddTag = HandleTagStack(tagType, tagState, i, word);

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

        private bool HandleTagStack(TagType tagType, TagState tagState, int tagIndex, string word)
        {
            if (tagState == TagState.TemporarilyOpen || tagState == TagState.TemporarilyOpenInWord)
            {
                var tagPosition = new TagPosition(tagType, tagState, tagIndex, word);
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

                    if (previousTagPosition.TagType == tagType
                        && previousTagPosition.TagState == TagState.TemporarilyOpen)
                    {
                        matchingOpenTag = previousTagPosition;
                        break;
                    }

                    if (previousTagPosition.TagType == tagType
                        && previousTagPosition.TagState == TagState.TemporarilyOpenInWord
                        && previousTagPosition.Content == word)
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
                var tagPosition = new TagPosition(tagType, TagState.Close, tagIndex, word);

                tagPosition.TagPair = matchingOpenTag;
                matchingOpenTag.TagPair = tagPosition;

                _tagPositionsStack.Push(tagPosition);

                return true;
            }

            return false;
        }

        private TagState DetermineTagState(string word, TagType tagType, int symbolIndex, Token currentToken)
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

            if (currentToken.TagPositions.Count(t => t.TagType == tagType) % 2 == 0)
            {
                return TagState.TemporarilyOpenInWord;
            }

            if (currentToken.TagPositions.Count(t => t.TagType == tagType) % 2 == 1)
            {
                return TagState.TemporarilyClose;
            }

            return TagState.NotTag;
        }
    }
}
