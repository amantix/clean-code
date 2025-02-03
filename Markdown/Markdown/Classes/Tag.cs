using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Tag
    {
        public int Index { get; set; }
        public bool IsStart { get; set; }
        public TagType Type { get; set; }

        public Tag(int index, bool isStart, TagType type)
        {
            Index = index;
            IsStart = isStart;
            Type = type;
        }

        public Tag(int index, TagType type) : this(index, false, type) { }
    }
}
