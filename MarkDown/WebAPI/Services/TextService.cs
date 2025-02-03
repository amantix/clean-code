using MarkDown.Classes;
using MarkDown.Interfaces;

namespace WebAPI.Services
{
    public class TextService
    {
        private readonly MD _markDown;

        public TextService(MD markDown) 
        {
            _markDown = markDown;
        }

        public async Task<string> RenderText(string text) 
        {
            return _markDown.Render(text);
        }
    }
}
