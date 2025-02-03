using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes.Tags
{
    public class BoldTag : TagType
    {
        public override string MarkdownTag => "__";
        public override string HtmlOpenTag => "<strong>";
        public override string HtmlCloseTag => "</strong>";
        public override bool IsDoubleTag => true;
    }
}