using MarkdownRenderer.Enums;
using MarkdownRenderer.Interfaces;

namespace MarkdownRenderer;

public class TokensParser : ITokensParser
{
    private readonly Stack<TagPosition> _tagPositionsStack = new();
    private readonly List<Token> _tokens = new();

    public IEnumerable<Token> ParseTokens(string unprocessedText)
    {
        string[] lines = unprocessedText.Split('\n');

        foreach (var line in lines)
        {
            string[] words = line.Split(' ');

            OpenParagraph();

            for (int i = 0; i < words.Length; i++)
            {
                var token = ProcessWord(words[i], i);

                _tokens.Add(token);
            }

            HandleHeaderTag();

            CloseParagraph();

            _tagPositionsStack.Clear();

            //_tokens.Add(new Token("\n"));
        }

        return _tokens;
    }

    private void HandleHeaderTag()
    {
        if (_tagPositionsStack.Any(t => t.TagType == TagType.HeaderTag))
        {
            var token = new Token("#");
            token.TagPositions.Add(new TagPosition(TagType.HeaderTag, TagState.Close, 0, "#"));

            _tokens.Add(token);
        }
    }

    private void OpenParagraph()
    {
        var tokenForParagraph = new Token("");
        tokenForParagraph.TagPositions.Add(new TagPosition(TagType.SpanTag, TagState.Open, 0, string.Empty));

        _tokens.Add(tokenForParagraph);
    }

    private void CloseParagraph()
    {
        var tokenForParagraph = new Token("");
        tokenForParagraph.TagPositions.Add(new TagPosition(TagType.SpanTag, TagState.Close, 0, string.Empty));

        _tokens.Add(tokenForParagraph);
    }

    private Token ProcessWord(string word, int wordIndex)
    {
        var currentToken = new Token(word);

        if (wordIndex == 0 && word.Length == 1
                           && HandleMarkdownSymbol(word, 0) == TagType.HeaderTag)
        {
            var tagPosition = new TagPosition(TagType.HeaderTag, TagState.Open, 0, "#");
            currentToken.TagPositions.Add(tagPosition);
            _tagPositionsStack.Push(tagPosition);

            return currentToken;
        }

        if (word == string.Empty || word == " ")
        {
            return currentToken;
        }

        if (word == "__" || word == "____")
        {
            return currentToken;
        }

        if (word.Any(char.IsDigit))
        {
            ProcessSymbolsInWord(word, 0, currentToken);
            if (ProcessSymbolsInWord(word, word.Length - 2, currentToken) == TagType.NotTag)
                ProcessSymbolsInWord(word, word.Length - 1, currentToken);

            return currentToken;
        }

        for (int i = 0; i < word.Length; i++)
        {
            var tagType = ProcessSymbolsInWord(word, i, currentToken);

            if (tagType is TagType.BoldTag)
                i++;
            else if (tagType is TagType.EscapedBoldTag)
                i += 3;
            else if (tagType is TagType.EscapedItalicTag)
                i += 2;
            else if (tagType is TagType.EscapedTag)
                i++;
        }

        return currentToken;
    }

    private TagType ProcessSymbolsInWord(string word, int index, Token currentToken)
    {
        var tagType = HandleMarkdownSymbol(word, index);

        if (tagType is TagType.NotTag)
            return tagType;

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
        if (word[index] == '\\')
        {
            if (index < word.Length - 1 && word[index + 1] == '_')
            {
                if (index < word.Length - 2 && word[index + 2] == '_')
                {
                    return TagType.EscapedBoldTag;
                }

                return TagType.EscapedItalicTag;
            }

            if (index < word.Length - 1 && word[index + 1] == '\\')
            {
                return TagType.EscapedTag;
            }
        }

        if (word[index] == '_')
        {
            if (index < word.Length - 1 && word[index + 1] == '_')
            {
                return TagType.BoldTag;
            }

            return TagType.ItalicTag;
        }

        if (word[index] == '#')
        {
            return TagType.HeaderTag;
        }

        return TagType.NotTag;
    }

    private bool HandleTagStack(TagType tagType, TagState tagState, int tagIndex, string word)
    {
        if (tagType is TagType.EscapedTag or TagType.EscapedItalicTag or TagType.EscapedBoldTag)
        {
            var tagPosition = new TagPosition(tagType, tagState, tagIndex, word);
            _tagPositionsStack.Push(tagPosition);

            return true;
        }

        if (tagState is TagState.TemporarilyOpen or TagState.TemporarilyOpenInWord)
        {
            var tagPosition = new TagPosition(tagType, tagState, tagIndex, word);
            _tagPositionsStack.Push(tagPosition);

            return true;
        }

        if (tagState == TagState.TemporarilyClose)
        {
            TagPosition matchingOpenTag = null;
            Stack<TagPosition> tempStack = new Stack<TagPosition>();

            // Достаем все из стека пока не найдем открывающий тег
            while (_tagPositionsStack.Count > 0)
            {
                var previousTagPosition = _tagPositionsStack.Pop();

                if (previousTagPosition.TagType == tagType
                    && previousTagPosition.TagState == TagState.TemporarilyOpen)
                {
                    matchingOpenTag = previousTagPosition;
                    tempStack.Push(previousTagPosition);
                    break;
                }

                if (previousTagPosition.TagType == tagType
                    && previousTagPosition.TagState == TagState.TemporarilyOpenInWord
                    && previousTagPosition.Content == word)
                {
                    matchingOpenTag = previousTagPosition;
                    tempStack.Push(previousTagPosition);
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
                tagsInRange.Add(tempTagPosition);

                if (tagType == TagType.ItalicTag
                    && tempTagPosition is { TagType: TagType.BoldTag, TagState: TagState.Close }
                    && matchingOpenTag != null)
                {
                    intersecTagPos = tempTagPosition;

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

            // Учитываем правило с пересечением
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
        if (tagType is TagType.EscapedTag or TagType.EscapedItalicTag or TagType.EscapedBoldTag)
        {
            return TagState.SingleTag;
        }

        int shift = currentToken.TagPositions.Any(t => t.TagType == TagType.EscapedTag) ? 2 : 0;
        if(symbolIndex - shift == 0 && (tagType is TagType.ItalicTag or TagType.BoldTag))
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

        if (tagType is TagType.ItalicTag or TagType.BoldTag
            && currentToken.TagPositions.Count(t => t.TagType == tagType) % 2 == 0)
        {
            return TagState.TemporarilyOpenInWord;
        }

        if (tagType is TagType.ItalicTag or TagType.BoldTag
            && currentToken.TagPositions.Count(t => t.TagType == tagType) % 2 == 1)
        {
            return TagState.TemporarilyClose;
        }

        return TagState.NotTag;
    }
}