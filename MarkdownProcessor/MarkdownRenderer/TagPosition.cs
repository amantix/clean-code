using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class TagPosition
    {
        public Tag Tag { get; }
        public int StartIndex { get; }
        public TagType TagType { get; } 

        public TagPosition(Tag tag, int startIndex, TagType tagType)
        {
            Tag = tag;
            StartIndex = startIndex;
            TagType = tagType;
        }
    }
}
