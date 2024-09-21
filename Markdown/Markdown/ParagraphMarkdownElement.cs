namespace Markdown;
//решил добавить на случай, если просто текст, чтобы он хоть в какой-то тэг был обёрнут
public class ParagraphMarkdownElement 
{
    private string text;
    public ParagraphMarkdownElement(string line)
    {
        text = line;
    }
    public string GetHtmlLine()
    {
        return $"<p>{text}</p>";
    }
}