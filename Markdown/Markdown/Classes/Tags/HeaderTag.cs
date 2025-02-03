using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes.Tags
{
    public class HeaderTag : TagType
    {
        public override string MarkdownTag => "#";
        public override string HtmlOpenTag => "<h1>";
        public override string HtmlCloseTag => "</h1>";
        public override bool IsDoubleTag => false;
    }
}