using MarkDown.Interfaces;

namespace MarkDown.Classes
{
    public class MD : IMarkDown
    {
        public string Render(string markDownText)
        {
            throw new NotImplementedException();
        }

        public string ProcessingText(string text, ref int index)
        {
            throw new NotImplementedException();
        }
    }
}
