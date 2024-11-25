using System.Text;

namespace MDConverter;

public abstract class MarkdownParserBase : IMarkdownRenderer
{
    public string Render(string input)
    {
        var result = new StringBuilder();
        int i = 0;
        while (i < input.Length)
        {
            result.Append(ProcessChar(ref i, input));
        }
        return result.ToString();
    }

    protected abstract string ProcessChar(ref int index, string text);

    // Проверка на экранированный символ
    protected bool IsEscaped(string text, int index)
    {
        return index > 0 && text[index - 1] == '\\';
    }
}