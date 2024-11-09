using Markdown.Enums;

namespace Markdown.Classes
{
    public class Tag
    {
        public readonly int Position;
        public readonly Tags Name;
        public readonly int Length;

        public Tag(Tags name, int position, int length)
        {
            Name = name;
            Position = position;
            Length = length;
        }
    }
}
