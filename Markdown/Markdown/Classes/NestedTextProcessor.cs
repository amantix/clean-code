using System.Text;

namespace Markdown;

public class NestedTextProcessor
{
    private string text;
    private TypeOfElement _typeOfElement;

    public NestedTextProcessor(string nestedText, TypeOfElement typeOfElement)
    {
        text = nestedText;
        _typeOfElement = typeOfElement;

    }
    public string GetNestedHtmlLine()
    {
        var result = ProcessNestedText(text);
        return result.ToString();
    }
    private string ProcessNestedText(string text)
    {
        if (_typeOfElement == TypeOfElement.StrongMarkdownElement)
        {
            return ProcessNestedTextForStrongElement(text);
        }
        return ProcessNestedTextForHeaderOrParagraphElement(text);
    }

    private string ProcessNestedTextForStrongElement(string text)
    {
        var result = new StringBuilder();
        var currentText = new StringBuilder();
        bool isItalic = false;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            if (currentChar == '\\')
            {
                if (i + 1 < text.Length)
                {
                    char nextChar = text[i + 1];
                    if (nextChar == '_' || nextChar == '\\')
                    {
                        currentText.Append(nextChar); 
                        i++; 
                        continue;
                    }
                }
                currentText.Append(currentChar);
                continue;
            }
            if (currentChar == '_')
            {
                // Проверяем окружение подчеркивания
                bool isPreviousCharDigit = (i > 0 && char.IsDigit(text[i - 1]));
                bool isNextCharDigit = (i + 1 < text.Length && char.IsDigit(text[i + 1]));

                if (isPreviousCharDigit || isNextCharDigit)
                {
                    currentText.Append(currentChar);
                    continue;
                }

                // Проверка для начала и закрытия курсивного выделения
                bool isStartOfItalic = (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]) && char.IsLetter(text[i + 1]));
                bool isEndOfItalic = (i > 0 && !char.IsWhiteSpace(text[i - 1]) && char.IsLetter(text[i - 1]));

                if (isItalic) 
                {
                    if (isEndOfItalic)
                    {
                        result.Append("<em>").Append(currentText.ToString().Substring(1)).Append("</em>");
                        currentText.Clear();
                        isItalic = false; 
                    }
                    else
                    {
                        currentText.Append(currentChar);
                    }
                }
                else 
                {
                    if (isStartOfItalic)
                    {
                        result.Append(currentText);
                        currentText.Clear();
                        currentText.Append('_');
                        isItalic = true; 
                    }
                    else
                    {
                        currentText.Append(currentChar);
                    }
                }
            }
            else
            {
                currentText.Append(currentChar); 
            }
        }

        if (currentText.Length > 0)
        {
            result.Append(currentText);
        }
        
        return result.ToString();
    }

    private string ProcessNestedTextForHeaderOrParagraphElement(string text)
    {
        StringBuilder result = new StringBuilder();
        StringBuilder currentText = new StringBuilder();
        bool isBold = false; 
        bool isItalic = false; 

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            if (currentChar == '\\')
            {
                if (i + 1 < text.Length)
                {
                    char nextChar = text[i + 1];
                    if (nextChar == '_' || nextChar == '\\')
                    {
                        currentText.Append(nextChar);
                        i++;
                        continue;
                    }
                }

                currentText.Append(currentChar);
                continue;
            }

            if (currentChar == '_')
            {
                bool isStartOfItalic = (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]) && char.IsLetter(text[i + 1]));
                bool isEndOfItalic = (i > 0 && !char.IsWhiteSpace(text[i - 1]) && char.IsLetter(text[i - 1]) || text[i - 2] == '\\');
                bool isPreviousCharDigit = (i > 0 && char.IsDigit(text[i - 1]));
                bool isNextCharDigit = (i + 1 < text.Length && char.IsDigit(text[i + 1]));

                if (isPreviousCharDigit || isNextCharDigit)
                {
                    currentText.Append(currentChar);
                    continue;
                }

                if (i + 1 < text.Length && text[i + 1] == '_')
                {
                    i++; 
                    if (isItalic)
                    {
                        if (currentChar == '_')
                            currentText.Append(currentChar);
                        currentText.Append(currentChar);
                        continue;
                    }
                    else
                    {
                        if (isBold) // Закрываем жирный текст
                        {
                            if (currentText.Length > 0)
                            {
                                result.Append(currentText);
                                currentText.Clear();
                            }
                            result.Append("</strong>");
                            isBold = false;
                        }
                        else // Открываем жирный текст
                        {
                            if (currentText.Length > 0)
                            {
                                result.Append(currentText);
                                currentText.Clear();
                            }
                            result.Append("<strong>");
                            isBold = true;
                        }
                    }
                }
                else 
                {
                    if (isItalic) // Закрываем курсивный текст
                    {
                        if (isEndOfItalic)
                        {
                            result.Append("<em>").Append(currentText.ToString().Substring(1)).Append("</em>");
                            currentText.Clear();
                            isItalic = false;
                        }
                        else
                        {
                            currentText.Append(currentChar);
                        }
                    }
                    else // Открываем курсивный текст
                    {
                        if (isStartOfItalic)
                        {
                            result.Append(currentText);
                            currentText.Clear();
                            currentText.Append('_');
                            isItalic = true;
                        }
                        else
                        {
                            currentText.Append(currentChar);
                        }
                    }
                }
            }
            else
            {
                currentText.Append(currentChar); 
            }
        }
        if (currentText.Length> 0)
        {
            result.Append(currentText.ToString());
        }

        return result.ToString();
    }
}