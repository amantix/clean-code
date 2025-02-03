using Application.Interfaces.Services;
using Application.Utils;
using MdProcessor.Interfaces;

namespace Application.Services;

public class MdService : IMdService
{
    private readonly IMdProcessor _markdownProcessor;

    public MdService(IMdProcessor mdProcessor)
    {
        _markdownProcessor = mdProcessor;
    }

    public async Task<Result<string>> RenderHtml(string markdown)
    {
        try
        {
            return Result<string>.Success(_markdownProcessor.Process(markdown));
        }
        catch
        {
            return Result<string>.Failure("Failed to render markdown");
        }
    }
}