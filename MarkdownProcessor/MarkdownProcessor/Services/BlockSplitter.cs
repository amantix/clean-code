using MarkdownProcessorLib.Models;
using System.Text;


namespace MarkdownProcessorLib.Services;

public class BlockSplitter
{
    public List<Block> Split(string text)
    {
        var blocks = new List<Block>();
        var currentText = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            var currentItem = text[i];

            switch (currentItem)
            {
                case '_':
                    HandleUnderscore(text, ref i, currentText, blocks);
                    break;
                case '\\':
                    HandleShielding(text, ref i, currentText);
                    break;
                case '#':
                    HandleHeading(text, i, currentText, blocks);
                    break;
                default:
                    currentText.Append(currentItem);
                    break;
            }
        }

        if (currentText.Length > 0)
            blocks.Add(new Block(currentText.ToString(), BlockType.Text));

        return blocks;
        
    }


    private void HandleUnderscore(
        string text,
        ref int index,
        StringBuilder currentText,
        List<Block> blocks)
    {
        if (currentText.Length > 0)
            CleaningCurrentText(blocks, currentText);

        if (index + 1 < text.Length && text[index + 1] == '_')
        {
            blocks.Add(new Block("__", BlockType.Bold));
            index++;
        }
        else blocks.Add(new Block("_", BlockType.Italic));
    }

    private void HandleShielding(
        string text,
        ref int index,
        StringBuilder currentText)
    {
        var specSym = "_#\\";
        if (index + 1 < text.Length && !specSym.Contains(text[index + 1]))
            currentText.Append(text[index]);
        
        else if (index + 1 < text.Length)
        {
            if (text[index + 1] == '\\') index++;
            
            else 
            {
                currentText.Append(text[index + 1]);
                index++;
            }
        }
    }

    private void HandleHeading(
        string text,
        int index,
        StringBuilder currentText,
        List<Block> blocks)
    {
        if (index > 0 && text[index - 1] == '\n' && index + 1 < text.Length)
        {
            if (currentText.Length > 0)
                CleaningCurrentText(blocks, currentText);
            
            blocks.Add(new Block("#", BlockType.Heading));
        }
        else if (index == 0)
            blocks.Add(new Block("#", BlockType.Heading));
    }

    private void CleaningCurrentText(List<Block> blocks, StringBuilder currentText)
    {
        blocks.Add(new Block(currentText.ToString(), BlockType.Text));
        currentText.Clear();
    }
}
