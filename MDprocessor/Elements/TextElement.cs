using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor.Elements;

public class TextElement : IMarkdownElement
{
    public string Text { get; }
    public TextElement(string text)
    {
        Text = text;
    }
}