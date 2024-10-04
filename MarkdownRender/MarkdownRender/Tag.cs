using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MdRender;

namespace MarkdownRender
{
    public class Tag
    {
        public int IndexStart {  get; set; }
        public int IndexEnd { get; set; }
        public string? HtmlTag { get; set; }
        public string? MarkdownTag { get; set; }
        /// <summary>
        ///  md и html теги
        /// </summary>
        ///
     }
}
