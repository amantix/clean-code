using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Enums;

namespace MarkdownRenderer
{
    public class TagPosition
    {
        public TagType TagType { get; set; }
        public bool IsOpened { get; set; }
        public int TagIndex { get; set; }

        public TagPosition(TagType tag, bool isOpened, int tagIndex)
        {
            TagType = tag;
            IsOpened = isOpened;
            TagIndex = tagIndex;
        }
    }
}
