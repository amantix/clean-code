using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

public interface IMarkdownLexer
{
    List<Token> Tokenize(string input);
}

