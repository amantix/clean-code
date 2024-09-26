using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownRender
{
    internal interface IGettingSending
    {
        /// <summary>
        /// получение md и отправка html
        /// </summary>
        /// <param name="markdownText"></param>
        /// <returns></returns>
        public string Getting(string markdownText);
        public string Sending(string htmlText);
    }
}
