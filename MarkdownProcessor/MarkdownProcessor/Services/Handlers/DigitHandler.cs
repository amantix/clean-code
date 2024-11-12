using MarkdownProcessorLib.Models;
namespace MarkdownProcessorLib.Services.Handlers;

public class DigitHandler
{
    public static void HandleDigit(
        List<string> html,
        List<Block> blocks,
        ref int i)
    {
        if (i > 0)
        {
            var lastTagValue = blocks[i - 1].Value;
            if (lastTagValue == "_" || lastTagValue == "__")
                html[^1] = lastTagValue;
        }

        html.Add(blocks[i].Value);

        if (i < blocks.Count - 1)
        {
            var nextTagValue = blocks[i + 1].Value;
            if (nextTagValue == "_" || nextTagValue == "__")
            {
                html.Add(nextTagValue);
                i++;
            }
        }
    }
}
