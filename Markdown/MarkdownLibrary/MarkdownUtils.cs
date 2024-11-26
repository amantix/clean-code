namespace MarkdownLibrary;

public static class MarkdownUtils
    {
        public static bool IsAdjacentToDigits(string input, int index, int tagLength)
        {
            bool leftIsDigit = index > 0 && char.IsDigit(input[index - 1]);
            bool rightIsDigit = index + tagLength < input.Length && char.IsDigit(input[index + tagLength]);
            return leftIsDigit || rightIsDigit;
        }

        public static bool IsEmptyTag(string input, int index, string markdownTag)
        {
            int closingIndex = FindClosingTag(input, markdownTag, index + markdownTag.Length);
            return closingIndex == index + markdownTag.Length;
        }

        public static int FindClosingTag(string input, string markdownTag, int start)
        {
            for (int i = start; i < input.Length - markdownTag.Length + 1; i++)
            {
                if (input.Substring(i, markdownTag.Length) == markdownTag &&
                    !IsAdjacentToDigits(input, i, markdownTag.Length) &&
                    !IsEmptyTag(input, i, markdownTag))
                {
                    return i;
                }
            }
            return -1;
        }
    }