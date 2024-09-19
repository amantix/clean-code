﻿using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown;

public class Md
{
    private List<TokenType> _tokens = new List<TokenType>();
    // Решил вынести поля ниже именно в этот класс, тк посчитал, что именно он должен
    // основным пайплайном md процессинга, включающего парсинг и рендер
    // Также тут хорошо будет заходить DI
    public IRenderer Renderer; 
    public IParser Parser;
    
    public Md(IRenderer renderer, IParser parser)
    {
        Renderer = renderer;
        Parser = parser;
    }

    // Строка textToParse - это либо путь к файлу, либо строка для парсинга в качестве md
    public void ParseAndRender(string textToParse)
    {
        Parser.Parse(textToParse, _tokens);
        Renderer.RenderMarkdown(_tokens);
    }
}
