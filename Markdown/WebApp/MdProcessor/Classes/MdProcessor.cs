using MdProcessor.Interfaces;

namespace MdProcessor.Classes;

public class MdProcessor(IParser parser, IRender render, ITagsResolver resolver) : IMdProcessor
{
    public IParser Parser { get; set; } = parser;
    public IRender Render { get; set; } = render;
    public ITagsResolver TagsResolver { get; set; } = resolver;
    
    public string Process(string markdown) => Render.RenderTokens(TagsResolver.ResolveTokens(Parser.Parse(markdown)));
}