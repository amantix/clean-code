using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

public class MarkdownProcessor
{
    private readonly IMarkdownParser _parser;

    public MarkdownProcessor()
    {
        IMarkdownLexer lexer = new MarkdownLexer();
        IHtmlRenderer renderer = new HtmlRenderer();
        _parser = new MarkdownParser(lexer, renderer);
    }

    public string Parse(string markdownText)
    {
        return _parser.RenderToHtml(markdownText);
    }
}


