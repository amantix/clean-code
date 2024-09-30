using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class TagPosition
    {
        public Tag Tag { get; set; }
        public int StartIndex { get; set; }

        public TagPosition(Tag tag, int startIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
        }
    }
}
