using System.Text;
using MdProcessor.Abstractions;
using MdProcessor.Enums;
using MdProcessor.Interfaces;

namespace MdProcessor.Classes;

public class MdRender : IRender
{
    public string RenderTokens(Token[] tokens)
    {
        var result = new StringBuilder();
            
        foreach ( var token in tokens )
        {
            if (token is not TagToken)
            {
                result.Append(token);
                continue;
            }

            var tag = (TagToken)token;
            switch (tag.Position)
            {
                case TagPosition.Start:
                    result.Append($"<{tag.HtmlTag}>");
                    break;
                case TagPosition.End:
                    result.Append($"</{tag.HtmlTag}>");
                    //if (tag is HeaderToken or ParagraphToken) result.Append('\n');
                    break;
                default:
                    throw new Exception($"Unknown tag position: {tag.Position}");
            }
        }

        return result.ToString();
    }
}