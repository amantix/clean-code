namespace MDConverter.Renderer;

public class BoldRenderer : MarkdownParserBase
{
    protected override string ProcessChar(ref int index, string text)
    {
        if (StartsWith(text, index, "__") && !IsEscaped(text, index))
        {
            return ProcessTag(ref index, text, "__", "<strong>", "</strong>");
        }
        return text[index++].ToString();
    }

    private bool StartsWith(string text, int index, string delimiter)
    {
        return index + delimiter.Length <= text.Length && text.Substring(index, delimiter.Length) == delimiter;
    }

    private string ProcessTag(ref int index, string text, string delimiter, string openTag, string closeTag)
    {
        int start = index + delimiter.Length;
        int end = text.IndexOf(delimiter, start, StringComparison.Ordinal);
        if (end == -1)
        {
            index += delimiter.Length;
            return delimiter;
        }
        string innerText = text.Substring(start, end - start);
        index = end + delimiter.Length;
        return $"{openTag}{innerText}{closeTag}";
    }
}