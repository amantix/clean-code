using Application.Utils;

namespace Application.Interfaces.Services;

public interface IMdService
{
    Task<Result<string>> RenderHtml(string markdown);
}