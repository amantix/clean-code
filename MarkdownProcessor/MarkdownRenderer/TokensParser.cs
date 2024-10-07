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
                char currentChar = word[i];

                if (currentChar == '_')
                {
                    if (_tagPositionsStack.TryPeek(out var previousTag))
                    {
                        if (previousTag.IsOpened)
                        {
                            var tagPos = new TagPosition(TagType.ItalicTag, false, i);
                            currentToken.TagPositions.Add(tagPos);
                        }
                    }
                    else
                    {
                        var tagPos = new TagPosition(TagType.ItalicTag, true, i);
                        _tagPositionsStack.Push(tagPos);
                        currentToken.TagPositions.Add(tagPos);
                    }
                }
            }

            return currentToken;
        }
    }
}
