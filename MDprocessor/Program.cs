using MDprocessor;
using System.Diagnostics;

var processor = new MarkdownProcessor();
TestMarkdown(processor, "### Заголовок", "Пример заголовка h3");
TestMarkdown(processor, "Привет, _курсив_!", "Курсив внутри строки");
TestMarkdown(processor, "И это __жирный__ текст", "Жирный текст");
TestMarkdown(processor, "\\_Экранированный подчеркивание_?", "Экранирование");
TestMarkdown(processor, "__жирный и _курсив_ вместе__", "Жирное с вкраплённым курсивом (упрощённо)");
static void TestMarkdown(MarkdownProcessor processor, string markdown, string description)
{
    Console.WriteLine("====================================");
    Console.WriteLine($"Описание теста: {description}");
    Console.WriteLine($"Исходный Markdown: {markdown}");
    string html = processor.Parse(markdown);
    Console.WriteLine("Результат HTML:");
    Console.WriteLine(html); 
    Console.WriteLine();
}