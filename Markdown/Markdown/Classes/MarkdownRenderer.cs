using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown 
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        public string Render(List<IMarkdownElement> elements)
        {
            var html = new StringBuilder();

            foreach (var element in elements)
            {
                html.Append(element.GetHtmlLine());
            }

            return html.ToString();
        }
    }
}
