using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownRenderer
    {
        public string Render(List<IMarkdownElement> elements)
        {
            var html = new StringBuilder();

            foreach (var element in elements)
            {
                html.AppendLine(element.GetHtmlLine());
            }

            return html.ToString();
        }
    }
}
