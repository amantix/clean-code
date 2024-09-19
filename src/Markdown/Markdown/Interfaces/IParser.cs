using Markdown.Enums;

namespace Markdown.Interfaces;

public interface IParser
{
    // Различные алгоритмы парсинга, например, через стек или еще как-нибудь
    // В оригинальный List добавляем токены в процессе парсинга
    bool Parse(string textToBeMarkdown, List<TokenType> tokens);
}