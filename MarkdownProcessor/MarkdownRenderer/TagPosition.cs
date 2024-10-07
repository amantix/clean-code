using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class TagPosition
    {
        public TagType TagType { get; set; }
        public TagState TagState { get; set; }
        public int TagIndex { get; set; }

        public TagPosition(TagType tag, TagState tagState, int tagIndex)
        {
            TagType = tag;
            TagState = tagState;
            TagIndex = tagIndex;
        }
    }
}
