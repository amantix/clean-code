namespace MdProcessor.Interfaces;

public interface IMdProcessor
{
    IParser Parser { get; set; }
    IRender Render { get; set; }
    ITagsResolver TagsResolver { get; set; }
    
    string Process(string markdown);
}