using System.Text;

namespace Markdown;

public class Markdown
{

    public string GetHtml(string markdownString)
    {
        var lines = SplitString(markdownString);
        var html = Render(lines);
        return html.ToString();
    }

    private StringBuilder Render(string[] lines)
    {
        var html = new StringBuilder();
        foreach (var line in lines)
        {
            IMarkdownElement element = CreateMarkdownElement(line);
            html.Append(element.GetHtmlLine());
        }
        return html;
    }
    
    private string[] SplitString(string text)
    {
        return text.Split(new[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries);//строка делится на слова отдельные
    }

    private IMarkdownElement CreateMarkdownElement(string line)
    {
        return null; //продумать логигу создания подходящего markdown элемента, скорее всего как-то через стэк
    }
}