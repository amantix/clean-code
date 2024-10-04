namespace MarkdownDraft;

public struct SpecialSymbol
{
    public TokenType Type { get; set; }
    public int Index { get; set; }
    
    // Длина специального символа
    public bool IsPairedTag { get; set; }
    // Длина специального символа
    public int TagLength { get; set; }
}