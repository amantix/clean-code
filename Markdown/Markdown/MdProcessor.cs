using Markdown.Classes;
using Markdown.Classes.Tags;
using Markdown.Interfaces;


namespace Markdown
{
    public class MdProcessor
    {
        private readonly LineParser lineParser;
        private readonly WordParser wordParser;
        private readonly IRenderer renderer;

        private readonly Dictionary<string, TagType> tags = new Dictionary<string, TagType>()
        {
            { "#", new HeaderTag()},
            { "_", new ItalicTag()},
            { "__", new BoldTag()},
        };

        public MdProcessor()
        {
            wordParser = new WordParser(tags);
            lineParser = new LineParser(wordParser);
            renderer = new Renderer(tags);
        }


        public string GetHtml(string MdText)
        {
            var lines = lineParser.Parse(MdText);
            return renderer.Render(lines);
        }
    }
}
