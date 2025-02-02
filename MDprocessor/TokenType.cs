using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

public enum TokenType
{
    Text,
    EscapeSymbol,
    Underscore,
    DoubleUnderscore,
    HeadingMarker
}
