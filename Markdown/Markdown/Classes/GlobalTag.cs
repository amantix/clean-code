using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class GlobalTag : Tag
    {
        public int GlobalIndex {  get; set; }

        public GlobalTag(int globalIndex, Tag tag) : base(tag.Index, tag.IsStart, tag.Type)
        {
            GlobalIndex = globalIndex;
        }
    }
}
