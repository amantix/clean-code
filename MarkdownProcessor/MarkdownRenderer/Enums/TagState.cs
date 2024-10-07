namespace MarkdownRenderer.Enums
{
    public enum TagState
    {
        Close = 1,
        Open = 2,
        TemporarilyOpen = 3,
        TemporarilyOpenInWord = 4,
        TemporarilyClose = 5,
        CloseInWord = 6,
        OpenInWord = 7,
        NotTag = 8,
    }
}
