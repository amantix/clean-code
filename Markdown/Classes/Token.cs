namespace Markdown.Classes
{
    public class Token
    {
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public Tag Tag { get; private set; }

        public Token(int startIndex, int endIndex, Tag tag)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
            Tag = tag;
        }
    }
}
