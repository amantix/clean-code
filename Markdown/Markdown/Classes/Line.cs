using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Line
    {
        public List<Token> Tokens { get; set; }

        public Line(List<Token> tokens)
        {
            Tokens = tokens;
        }
    }
}
