using System.Text;

namespace Markdown;

public class Markdown
{

    public string GetHtml(string markdownText)
    {
        var html = Render(markdownText);
        return html.ToString();
        //хотел сам рэндер текста и получение html разделить, но не уверен нужно ли это
    }

    private StringBuilder Render(string markdownText)
    {
        var html = new StringBuilder();
        return html;
    }
    
    private IMarkdownElement CreateElement(string line)
    {
        return null; //продумать логигу создания подходящего класса элемента, скорее всего как-то через стэк
    }
}