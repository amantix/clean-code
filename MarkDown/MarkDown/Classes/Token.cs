using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.Classes
{
    public class Token
    {
        public readonly int Length;
        public readonly int Position;
        public readonly string Value;

        public Token(int length, int position, string value) 
        {
            Length = length;
            Position = position;
            Value = value;
        }
    }
}
