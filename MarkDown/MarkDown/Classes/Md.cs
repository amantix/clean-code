using MarkDown.Interfaces;
using System.Text;

namespace MarkDown.Classes
{
    public class MD : IMarkDown
    {
        public string Render(string markDownText)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var convertLine = new Converter();

            var lines = StringParser.SplitTextOnLines(markDownText);
            
            foreach (var line in lines ) 
            {  
               stringBuilder.Append(convertLine.ConvertToHTML(line));
            }
            return stringBuilder.ToString();
        }
    }
}
