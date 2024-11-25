namespace MDConverter.Renderer;

public class HeaderRenderer : MarkdownParserBase
{
    protected override string ProcessChar(ref int index, string text)
    {
        if (IsHeader(text, index) && !IsEscaped(text, index))
        {
            return ProcessHeader(ref index, text);
        }

        return text[index++].ToString();
    }

    private bool IsHeader(string text, int index)
    {
        int level = 0;
        while (level < text.Length && level < 5 && text[index + level] == '#')
        {
            level++;
        }

        return level > 0 && (index + level < text.Length) && text[index + level] == ' ';
    }

    private string ProcessHeader(ref int index, string text)
    {
        int level = 0;
        while (level < text.Length && level < 7 && text[index + level] == '#')
        {
            level++;
        }

        index += level + 1;
        string headerContent = text.Substring(index).Split('\n')[0].Trim();
        index += headerContent.Length;
        return $"<h{level}>{headerContent}</h{level}>";
    }
}
