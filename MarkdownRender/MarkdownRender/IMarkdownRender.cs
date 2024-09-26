using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownRender
{
    internal interface IMarkdownRender
    {
        /// <summary>
        /// Интерфейс для рееализации преобразования md в html
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public string ConvertToHtml(string htmlString, Dictionary<string, Tag>? Tags);
    }
}
