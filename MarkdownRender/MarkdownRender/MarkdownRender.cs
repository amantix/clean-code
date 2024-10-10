using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownRender
{
    internal class MarkdownRender
    {
        static void Main(string[] args)
        {
            var t = new MarkdownRender();
            StringBuilder sb = new("__нач_але");
            var result = t.MdRender(sb);
            Console.WriteLine(result);
        }

        public StringBuilder MdRender(StringBuilder mdString)
        {
            //доделать условия
            int emLength = 0;
            int strongLength = 0;
            bool flagEm = false;
            bool flagStrong = false;
            int lastStrongIndex = 0;
            int lastEmIndex = 0;
            int index = 0;
            while (index < mdString.Length - 1)
            {
                if (flagStrong)
                {
                    strongLength++;
                }
                if (flagEm)
                {
                    emLength++;
                }
                if (mdString[index] == '_')
                {

                    if (mdString[index + 1] == '_')
                    {

                        lastStrongIndex = index;
                        if (flagStrong == false)
                        {
                            flagStrong = true;
                        }

                        else
                        {
                            flagStrong = false;
                            mdString.Replace("_", "</strong>", index + 1, 1);
                            mdString.Replace("_", "", index, 1);
                            mdString.Replace("_", "<strong>", index - strongLength + 1, 1);
                            mdString.Replace("_", "", index - strongLength, 1);
                            strongLength = 0;
                            if (flagEm)
                            {
                                emLength -= 6;
                            }
                        }
                    }
                    else
                    {
                        if (index != 0 && mdString[index - 1] != '_')
                        {
                            lastEmIndex = index;

                            if (flagEm == false)
                            {
                                flagEm = true;
                            }
                            else if (flagStrong == false)
                            {
                                flagEm = false;
                                mdString.Replace("_", "</em>", index, 1);
                                mdString.Replace("_", "<em>", index - emLength, 1);
                                emLength = 0;
                            }
                        }
                        if (index == 0)
                        {
                            if (flagEm == false)
                            {
                                flagEm = true;
                            }
                            else
                            {
                                flagStrong = false;
                                flagEm = false;
                                mdString.Replace("_", "</em>", index, 1);
                                mdString.Replace("_", "<em>", index - emLength, 1);
                            }
                        }
                    }

                }


                index++;
            }
            if (flagEm == true && mdString[index] == '_' && mdString[index - 1] != '_')
            {
                mdString.Replace("_", "</em>", index, 1);
                mdString.Replace("_", "<em>", index - emLength - 1, 1);
            }
            if (flagEm == true && mdString[index] == '_' && mdString[index - 1] == '_' && flagStrong == true)
            {
                mdString.Replace("_", "<strong>", lastStrongIndex + 1, 1);
                mdString.Replace("_", "", lastStrongIndex, 1);
                mdString.Replace("_", "</strong>", index - strongLength + 1, 1);
                mdString.Replace("_", "", index - strongLength, 1);
            }
            return mdString;
        }

        //public bool NumberInString(int indexStart, int indexEnd, string partOfString)
        //{
        //    foreach(char i in partOfString.Substring(indexStart,indexEnd))
        //    {
        //        if
        //    }
        //}
    }
}

