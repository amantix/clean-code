namespace MarkdownProcessorLib.Models;

public record Block(string Value, BlockType Type);

public enum BlockType
{
    Text,
    Italic,
    Bold,
    Heading,
    Shielding
}