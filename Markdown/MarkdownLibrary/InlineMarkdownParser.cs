using System.Text;

namespace MarkdownLibrary;

public class InlineMarkdownParser
    {
        private readonly Stack<string> _tagStack;

        public InlineMarkdownParser()
        {
            _tagStack = new Stack<string>();
        }

        public string Parse(string input)
        {
            var result = new StringBuilder();
            int i = 0;

            while (i < input.Length)
            {
                if (input[i] == '\\' && i + 1 < input.Length) // экранирование
                {
                    result.Append(input[i + 1]);
                    i += 2;
                }
                else if (input[i] == '_' && i + 1 < input.Length && input[i + 1] == '_') // полужирный текст
                {
                    HandleBoldTag("__", "<strong>", "</strong>", ref i, input, result);
                }
                else if (input[i] == '_') // курсив
                {
                    HandleTag("_", "<em>", "</em>", ref i, input, result);
                }
                else
                {
                    result.Append(input[i]);
                    i++;
                }
            }

            // закрываем все оставшиеся теги
            while (_tagStack.Count > 0)
            {
                result.Append(_tagStack.Pop());
            }

            return result.ToString();
        }

        private void HandleBoldTag(string markdownTag, string openingTag, string closingTag,
            ref int i, string input, StringBuilder result)
        {
            // полужирный внутри курсива не обрабатывается
            if (_tagStack.Contains("_"))
            {
                result.Append(markdownTag);
                i += markdownTag.Length;
                return;
            }

            HandleTag(markdownTag, openingTag, closingTag, ref i, input, result);
        }

        private void HandleTag(string markdownTag, string openingTag, string closingTag,
            ref int i, string input, StringBuilder result)
        {
            if (MarkdownUtils.IsAdjacentToDigits(input, i, markdownTag.Length) ||
                MarkdownUtils.IsEmptyTag(input, i, markdownTag))
            {
                result.Append(markdownTag);
                i += markdownTag.Length;
                return;
            }

            if (_tagStack.Count > 0 && _tagStack.Peek() == markdownTag)
            {
                _tagStack.Pop();
                result.Append(closingTag);
                i += markdownTag.Length;
            }
            else
            {
                int closingIndex = MarkdownUtils.FindClosingTag(input, markdownTag, i + markdownTag.Length);
                if (closingIndex != -1)
                {
                    _tagStack.Push(markdownTag);
                    result.Append(openingTag);
                    i += markdownTag.Length;
                }
                else
                {
                    result.Append(markdownTag);
                    i += markdownTag.Length;
                }
            }
        }
    }