using System;
using System.Text;

namespace MarkDown.Classes
{
    public class Converter
    {
        public string ConvertToHTML(string line)
        {
            line = RenderHeaders(line);
            line = RenderTags(line);
            return line;
        }

        private string RenderHeaders(string line)
        {
            int headerLevel = GetHeaderLevel(line);
            if (headerLevel > 0)
            {
                string headerContent = line.Substring(headerLevel).Trim();
                return $"<h{headerLevel}>{headerContent}</h{headerLevel}>";
            }
            return line;
        }

        private int GetHeaderLevel(string text)
        {
            int level = 0;
            while (level < text.Length && level < 7 && text[level] == '#')
            {
                level++;
            }
            return level > 0 && level < text.Length && text[level] == ' ' ? level : 0;
        }

        private string RenderTags(string text)
        {
            var tagStack = new TagStack();
            var processedText = new StringBuilder();
            int i = 0;

            while (i < text.Length)
            {
                if (text[i] == '\\')
                {
                    processedText.Append(HandleEscaping(ref i, text));
                }
                else if (StartWithDelimiter(text, i, "__"))
                {
                    HandleTagWithStack(ref i, text, "__", "<strong>", "</strong>", tagStack, processedText);
                }
                else if (StartWithDelimiter(text, i, "_"))
                {
                    HandleTagWithStack(ref i, text, "_", "<em>", "</em>", tagStack, processedText);
                }
                else
                {
                    processedText.Append(text[i]);
                    i++;
                }
            }

            foreach (var tagInfo in tagStack.GetTags())
            {
                processedText.Insert(tagInfo.StartIndex, tagInfo.Delimiter);
            }

            return processedText.ToString();
        }

        private void HandleTagWithStack(ref int index, string text, string delimiter, string openTag, string closeTag,
    TagStack tagStack, StringBuilder result)
        {
            // Если закрываем текущий тег
            if (tagStack.Pop(delimiter) is Tag tagInfo)
            {
                result.Insert(tagInfo.StartIndex, tagInfo.OpenTag);
                result.Append(tagInfo.CloseTag);
                index += delimiter.Length;
            }
            else
            {
                if (delimiter == "__" && tagStack.Contains("_"))
                {
                    result.Append(delimiter);
                    index += delimiter.Length;
                }
                else
                {
                    tagStack.Push(new Tag(delimiter, openTag, closeTag, result.Length));
                    index += delimiter.Length;
                }
            }
        }

        private string HandleEscaping(ref int index, string text)
        {
            if (index + 1 < text.Length)
            {
                char escapedChar = text[index + 1];
                index += 2;
                return escapedChar.ToString();
            }
            return "\\";
        }

        private bool StartWithDelimiter(string text, int index, string delimiter)
        {
            return index + delimiter.Length <= text.Length && text.Substring(index, delimiter.Length) == delimiter;
        }
    }
}
