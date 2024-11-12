using MarkdownProcessorLib.Models;
namespace MarkdownProcessorLib.Services.Handlers;

public class TextHandler 
{ 
    public static void Handle(List<string> html,
        List<Block> blocks,
        ref int i,
        ref bool headFlag)
    {
        if (int.TryParse(blocks[i].Value, out _))
            DigitHandler.HandleDigit(html, blocks, ref i);
        
        else if (headFlag && (blocks[i].Value.Contains('\n') || i == blocks.Count - 1))
        {
            var str = blocks[i].Value + "</h1>";
            var item = str.StartsWith(" ") ? str.Substring(1) : str;
            html.Add(item);
            headFlag = false;
        }
        
        else if (headFlag && html[^1] == "<h1>")
        {
            var str = blocks[i].Value;
            var item = str.StartsWith(" ") ? str.Substring(1) : str;
            html.Add(item);
        }

        else html.Add(blocks[i].Value);
    }
}
