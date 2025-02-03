namespace MarkDown.Classes
{
    public class Tag
    {
        public string Delimiter { get; }
        public string OpenTag { get; }
        public string CloseTag { get; }
        public int StartIndex { get; }

        public Tag(string delimiter, string openTag, string closeTag, int startIndex)
        {
            Delimiter = delimiter;
            OpenTag = openTag;
            CloseTag = closeTag;
            StartIndex = startIndex;
        }
    }
}
