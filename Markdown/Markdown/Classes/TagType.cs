using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public abstract class TagType
    {
        public abstract string MarkdownTag { get; }
        public abstract string HtmlOpenTag { get; }
        public abstract string HtmlCloseTag { get; }
        public abstract bool IsDoubleTag { get; }
    }
}
