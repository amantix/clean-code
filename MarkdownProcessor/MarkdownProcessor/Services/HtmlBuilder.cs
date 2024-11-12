using MarkdownProcessorLib.Models;
using MarkdownProcessorLib.Services.Handlers;

namespace MarkdownProcessorLib.Services;

public class HtmlBuilder
{
    public string Build(List<Block> blocks)
    {
        var html = new List<string>();
        var tagStack = new Stack<string>();
        var tagIndexStack = new Stack<int>();
        var insideCharFlag = "";
        var headFlag = false;
     
        for (var i = 0; i < blocks.Count; i++)
        {
            switch (blocks[i].Type)
            {
                case BlockType.Italic:
                case BlockType.Bold:
                    {
                        var tag = blocks[i].Type == BlockType.Bold ? new Tag("strong", "__") : new Tag("em", "_");
                        TagHandler.Handle(html, tagStack, tagIndexStack, tag, ref insideCharFlag, blocks, ref i);
                        if (headFlag && i == blocks.Count - 1) 
                            html.Add("</h1>");
                    }
                    break;
                
                case BlockType.Heading:
                    {
                        html.Add("<h1>");
                        headFlag = true;
                        break;
                    }
                
                case BlockType.Text:
                    TextHandler.Handle(html, blocks, ref i, ref headFlag);
                    
                 break;
            }
            
        }


        if (insideCharFlag == "")
        {
            TagHandler.CloseTags(html, tagStack, tagIndexStack);
        }

        return string.Join("", html);
    }
}
