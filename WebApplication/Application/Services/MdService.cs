using Application.Abstract.Services;
using Application.ResponseResult;
using MarkdownProccesor;

namespace Application.Services;

public class MdService(MarkdownToHtmlProcessor processor): IMdService
{
    public async Task<Result> GetHtml(string mdText)
    {
        try
        {
            var htmlRaw = processor.ConvertToHtml(mdText);
            return Result<string>.Success(htmlRaw);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Failed to convert md text to html", ErrorType.ServerError));
        }
    }
}