using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstracts
{
    public interface IMarkdownService
    {
        Task<ActionResult<string>> ConvertToHtmlAsync(string rawMarkdown);
        Task<ActionResult<string>> GetContentMarkdownAsync(int documentId);

    }
}
