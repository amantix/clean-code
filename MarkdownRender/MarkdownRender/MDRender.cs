using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MdRender;

namespace MarkdownRender
{
    public class MDRender : IMarkdownRender
    {
        

        public string MarkdownRender(string markdownString)
        {
            string result = "";
            
            var markdownParagraph = markdownString.Split("\r\n");
            StringBuilder[] htmlParagraph = new StringBuilder[markdownParagraph.Length];
            for (int i = 0; i < markdownParagraph.Length; i++) 
            {
                htmlParagraph[i] = new StringBuilder(markdownParagraph[i]);
                List<Tag> tags = SearchForTagIndexes(markdownParagraph[i]);


            }
            return result;
        }
        /// <summary>
        /// найти все теги и заменять  их используя стек
        /// сделал поиск тегов
        /// </summary>
        /// <param name="markdownParagraph"></param>
        /// <returns></returns>
        public List<Tag> SearchForTagIndexes(string markdownParagraph)
        {

            List<Tag> result = [];
            for (int i = 1; i < markdownParagraph.Length; i++) 
            { 
                
                if ($"{markdownParagraph[i]}{markdownParagraph[i-1]}" == "__")
                {
                    Tag tag = new(){ 
                        IndexStart = i-1, 
                        IndexEnd = i,
                        MarkdownTag= "__",
                        HtmlTag="<strong>"};
                    result.Add(tag);
                }
                else if  ($"{markdownParagraph[i]}{markdownParagraph[i - 1]}" == " _" || (markdownParagraph[i]!='_' && markdownParagraph[i - 1]=='_'))
                {
                    if ((1 + i) < markdownParagraph.Length && markdownParagraph[i + 1].ToString() != "_")
                    {
                        Tag tag = new()
                        {
                            IndexStart = i - 1,
                            IndexEnd = i,
                            MarkdownTag = $"{markdownParagraph[i]}{markdownParagraph[i - 1]}",
                            HtmlTag = $"{markdownParagraph[i]}<em>"
                        };
                        result.Add(tag);
                    }
                }
                else if ($"{markdownParagraph[i]}{markdownParagraph[i - 1]}" == "\\_" )
                {
                    Tag tag = new()
                    {
                        IndexStart = i - 1,
                        IndexEnd = i,
                        MarkdownTag = "\\_",
                        HtmlTag = ""
                    };
                    result.Add(tag);
                }
            }
            return result;
        }
        public string ConvertToHtml(StringBuilder markdownString, List<Tag> tags)
        {
            Stack<StringBuilder> markdownTags = new();
            throw new NotImplementedException();
        }
    }
}
