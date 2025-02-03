using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Classes
{
    public class Token
    {
        public string Word { get; set; }
        public List<Tag> Tags { get; set; }

        public Token(string word, List<Tag> tags)
        {
            Word = word;
            Tags = tags;
        }
    }
}
