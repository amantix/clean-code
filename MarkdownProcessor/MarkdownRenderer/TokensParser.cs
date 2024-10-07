using MarkdownRenderer.Enums;
using MarkdownRenderer.Interfaces;
using System.Linq;

namespace MarkdownRenderer;

public class TokensParser : ITokensParser
{
    private Stack<TagPosition> _tagPositionsStack = new();
    private List<Token> _tokens = new();

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
        bool isContainsDigits = word.Any(char.IsDigit);

        if (isContainsDigits)
        {
            ProcessSymbolsInWord(word, 0, currentToken);

            // Обрабатываем последний символ. Но может быть так, что его его длина равна двум, поэтому перестаховываемся
            if (ProcessSymbolsInWord(word, word.Length - 2, currentToken) == TagType.NotTag)
                ProcessSymbolsInWord(word, word.Length - 1, currentToken);

            return currentToken;
        }

        for (int i = 0; i < word.Length; i++)
        {
            var tagType = ProcessSymbolsInWord(word, i, currentToken);

            if (tagType is TagType.BoldTag)
                i++;
        }

        return currentToken;
    }

    private TagType ProcessSymbolsInWord(string word, int index, Token currentToken)
    {
        var tagType = HandleMarkdownSymbol(word, index);

        if (tagType is TagType.NotTag)
            return TagType.NotTag;

        var tagState = DetermineTagState(word, tagType, index, currentToken);

        if (tagState is TagState.NotTag)
            return TagType.NotTag;

        bool isPossibleToAddTag = HandleTagStack(tagType, tagState, index, word);

        if (isPossibleToAddTag)
        {
            currentToken.TagPositions.Add(_tagPositionsStack.Peek());
        }

        return tagType;
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

            // Достаем все из стека пока не найдем открывающий
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

            // Хешсет для хранения всех тегов между открытым и закрытым
            var tagsInRange = new HashSet<TagPosition>();
            
            // Промежуточный тег для проверки условия пересечения двойных и одинарных подчерков
            TagPosition? intersecTagPos = null;

            // Кладем все обратно + учитваем, что между одинарными двойное не работает
            while (tempStack.Count > 0)
            {
                var tempTagPosition = tempStack.Pop();

                if (tagType == TagType.ItalicTag
                    && tempTagPosition.TagType == TagType.BoldTag
                    && tempTagPosition.TagState == TagState.Close)
                {
                    intersecTagPos = tempTagPosition;
                    tagsInRange.Add(tempTagPosition);

                    tempTagPosition.TagPair.TagState = TagState.NotTag;
                    tempTagPosition.TagState = TagState.NotTag;
                }

                _tagPositionsStack.Push(tempTagPosition);
            }

            // Если не нашли открывающий, выходим
            if (matchingOpenTag is null)
            {
                return false;
            }


            var tagPosition = new TagPosition(tagType, TagState.Close, tagIndex, word)
            {
                TagPair = matchingOpenTag
            };

            matchingOpenTag.TagPair = tagPosition;
            matchingOpenTag.TagState = TagState.Open;

            if (intersecTagPos != null && !(tagsInRange.Contains(intersecTagPos.TagPair)))
            {
                matchingOpenTag.TagState = TagState.NotTag;
                tagPosition.TagState = TagState.NotTag;
            }

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