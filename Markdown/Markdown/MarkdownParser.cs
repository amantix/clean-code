using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownParser
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
            return null; //продумать логику создания подходящего класса элемента
        }
    }
}
