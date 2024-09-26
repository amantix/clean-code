using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownRender
{
    public class MDRender : IMarkdownRender
    {
        static void Main()
        {
        }

        public string ConvertToHtml(string markdownString, Dictionary<string, Tag>? Tags)
        {
            string tag = markdownString.Split()[0];
            var mdText = markdownString.Trim(tag[0]);
            string result = "";
            
            if (Tags != null)
            {
                Console.WriteLine(tag);
                if (Tags[tag].DoubleTagHtml == true) { result = $"<{Tags[tag].HtmlTag}>{mdText}</{Tags[tag].HtmlTag}>"; }
                else { result = $"<{Tags[tag].HtmlTag}>{mdText}"; }
            }
            return result ;
        }

        public static Dictionary<string, Tag> CreateTagTable()
        {
            Dictionary<string, Tag>? Tags = new Dictionary<string, Tag>();
            Tags?.Add("#", new Tag { MarkdownTag = "#", HtmlTag = "h1", DoubleTagHtml = true });
            return Tags;
        }
    }
}
