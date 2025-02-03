using Markdown.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Renderer : IRenderer
    {
        private readonly Dictionary<string, TagType> tagDictionaries;

        public Renderer(Dictionary<string, TagType> tagDictionaries)
        {
            this.tagDictionaries = tagDictionaries;
        }

        public string Render(List<Line> lines)
        {
            var htmlText = new List<string>();
            foreach (var line in lines)
            {
                var tokens = line.Tokens;
                htmlText.Add(GetHtmlLine(tokens));
            }
            return string.Join('\n', lines.Select(x => GetHtmlLine(x.Tokens)).ToArray());// htmlText);
        }

        private string GetHtmlLine(List<Token> tokens)
        {
            var result = new List<string>();
            foreach (var token in tokens)
            {
                var word = GetHtmlWord(token);
                result.Add(word);
            }
            if (result.Count > 0 && result[0].Equals("<h1>", StringComparison.Ordinal))
                result.Add("</h1>");

            return string.Join(' ', result);
        }

        private string GetHtmlWord(Token token)
        {
            var result = new StringBuilder(token.Word);
            var tags = token.Tags.OrderByDescending(x => x.Index);

            foreach (var tag in tags)
            {
                if (tag.Type.MarkdownTag.Equals("#"))
                    return $"{tag.Type.HtmlOpenTag}";

                var htmlTag = tag.IsStart ? tag.Type.HtmlOpenTag : tag.Type.HtmlCloseTag;
                result.Remove(tag.Index, tag.Type.MarkdownTag.Length);
                result.Insert(tag.Index, htmlTag);
            }

            return result.ToString();
        }
    }
}
