using Markdown.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class LineParser : IParser<Line>
    {
        private readonly WordParser _wordParser;

        public LineParser(WordParser wordParser)
        {
            _wordParser = wordParser;
        }

        public List<Line> Parse(string text)
        {
            var lines = text.Split('\n');
            var result = new List<Line>();
            var tokens = new List<Token>();

            foreach(var line in lines)
            {
                tokens = _wordParser.Parse(line);
                result.Add(new Line(tokens));
            }

            return result;
        }
    }
}
