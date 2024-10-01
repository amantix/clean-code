using System.Security.AccessControl;
using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class Token
    {
        public Tag Tag { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public TagType TagType { get; }

        public Token(Tag tag, int startIndex, int endIndex, TagType tagType)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
            TagType = tagType;
        }
    }
}
