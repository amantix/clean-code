using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.Interfaces
{
    public interface IMarkDown
    {
        public string Render(string markDownText);
        public string ProcessingText(string text, ref int index);
    }
}
