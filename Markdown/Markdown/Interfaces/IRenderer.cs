using Markdown.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Interfaces
{
    public interface IRenderer
    {
        public string Render(List<Line> lines);
    }
}
