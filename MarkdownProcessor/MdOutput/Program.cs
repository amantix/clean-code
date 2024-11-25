using MDConverter;

namespace MdOutput;

internal class Program
{
    static void Main()
    {
        string markdown = "### Заголовок третьего уровня \n _а тут будет __жирный текст__ внутри курсива_";

        var converter = new MarkdownConverter();
        string html = converter.ConvertToHtml(markdown);

        Console.WriteLine(html);
    }
}