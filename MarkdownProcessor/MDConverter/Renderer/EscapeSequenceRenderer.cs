namespace MDConverter.Renderer;

public class EscapeSequenceRenderer : MarkdownParserBase
{
    protected override string ProcessChar(ref int index, string text)
    {
        // Обрабатываем экранированный символ
        if (text[index] == '\\' && !IsEscaped(text, index))
        {
            return HandleEscapeSequence(ref index, text);
        }
        return text[index++].ToString();
    }

    private string HandleEscapeSequence(ref int index, string text)
    {
        if (index + 1 < text.Length)
        {
            char escapedChar = text[index + 1];
            index += 2; // Пропускаем экранирующий символ и символ, который экранирован
            return escapedChar.ToString();
        }
        return "\\"; 
    }
}