using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor.Elements;

public class StrongElement : IMarkdownElement
{
    public List<IMarkdownElement> Children { get; }
    public StrongElement(List<IMarkdownElement> children)
    {
        Children = children;
    }
}