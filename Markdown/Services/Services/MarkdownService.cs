using Data.Utils;
using Markdown;
using Markdown.Interfaces;
using Services.Abstracts;

namespace Services.Services
{
    public class MarkdownService(MdProcessor mdProcessor, MinioService minioService) : IMarkdownService
    {
        public async Task<ActionResult<string>> ConvertToHtmlAsync(string rawMarkdown)
        {
            try
            {
                var htmlString = await Task.Run(() => mdProcessor.GetHtml(rawMarkdown));
                return ActionResult<string>.Ok(htmlString);
            }
            catch (Exception ex)
            {
                return ActionResult<string>.Fail(ex.Message!)!;
            }
        }

        public async Task<ActionResult<string>> GetContentMarkdownAsync(int documentId)
        {
            var ctx = new CancellationTokenSource();
            var fileName = $"{documentId}.md";

            try
            {
                var content = await minioService.GetMarkdownTextAsync(fileName, ctx.Token);
                return ActionResult<string>.Ok(content);
            }
            catch (Exception ex)
            {
                return ActionResult<string>.Fail(ex.Message)!;
            }
        }



    }
}
