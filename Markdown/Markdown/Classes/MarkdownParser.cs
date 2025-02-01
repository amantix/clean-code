using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownParser : IMarkdownParser
    {
        public List<IMarkdownElement> Parse(string markdownText)
        {
            var elements = new List<IMarkdownElement>();
            var lines = markdownText.Split('\n');

            foreach (var line in lines)
            {
                var element = CreateMarkdownElement(line);
                if (element != null)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }
        private IMarkdownElement CreateMarkdownElement(string line)
        {
            if (line.StartsWith("#"))
            {
                return new HeaderMarkdownElement(line);
            }
            else if (line.StartsWith("__") && line.EndsWith("__"))
            {
                return new StrongMarkdownElement(line);
            }
            else if (line.StartsWith("_") && line.EndsWith("_"))
            {
                return new ItalicMarkdownElement(line);
            }
            else if (line.StartsWith("[") && line.EndsWith(")"))
            {
                return new LinkMarkdownElement(line);
            }
            else
            {
                return new ParagraphMarkdownElement(line);
            }
        }
    }
}
