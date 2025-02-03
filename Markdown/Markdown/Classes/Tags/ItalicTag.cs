using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes.Tags
{
    public class ItalicTag : TagType
    {
        public override string MarkdownTag => "_";
        public override string HtmlOpenTag => "<em>";
        public override string HtmlCloseTag => "</em>";
        public override bool IsDoubleTag => true;
    }
}