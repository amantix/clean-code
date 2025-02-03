using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.Classes
{
    public class StringParser
    {
        public static string[] SplitTextOnLines(string markdownText)
        {
            var result = new List<string>();
            var lines = markdownText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                result.Add(line);
            }
            return result.ToArray();
        }
    }
}
