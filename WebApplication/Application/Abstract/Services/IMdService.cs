using Application.ResponseResult;

namespace Application.Abstract.Services;

public interface IMdService
{
    Task<Result> GetHtml(string mdText);
}