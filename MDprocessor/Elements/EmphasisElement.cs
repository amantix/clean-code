using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor.Elements;

public class EmphasisElement : IMarkdownElement
{
    public List<IMarkdownElement> Children { get; }
    public EmphasisElement(List<IMarkdownElement> children)
    {
        Children = children;
    }
}

