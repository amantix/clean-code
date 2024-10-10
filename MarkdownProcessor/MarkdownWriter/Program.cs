﻿using MarkdownRenderer;

class Program
{
    public static void Main()
    {
        var tokensParser = new TokensParser();
        string text =  "леее jkjkj[программирование][ggffgfg]gbg ипипии\n" +
                       "[ggffgfg]: https://www.google.com";

        var md = new MarkdownConverter(tokensParser);
        string htmlContent = md.ConvertToHtml(text);

        Console.WriteLine(htmlContent);

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");

        if (!File.Exists(filePath))
        {
            using (FileStream fs = File.Create(filePath))
            {
                Console.WriteLine($"Файл {filePath} успешно создан.");
            }
        }

        File.WriteAllText(filePath, htmlContent);

        Console.ForegroundColor = ConsoleColor.Red; 
        Console.WriteLine($"\nHtml разметка записана в {filePath}");
        Console.ResetColor();
    }
}