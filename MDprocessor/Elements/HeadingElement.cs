using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor.Elements;

public class HeadingElement : IMarkdownElement
{
    public int Level { get; }
    public List<IMarkdownElement> Children { get; }

    public HeadingElement(int level, List<IMarkdownElement> children)
    {
        Level = level;
        Children = children;
    }
}
