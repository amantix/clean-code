using MDprocessor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDprocessor;

public interface IHtmlRenderer
{
    string Render(List<IMarkdownElement> elements);
}
