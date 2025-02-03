using Data.Entities;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstracts
{
    public interface IUserService
    {
        Task<ActionResult<string>> RegistrationAsync(string email, string password, string username);
        Task<ActionResult<string>> AuthorizeAsync(string email, string password);
        Task<ActionResult<int>> GetUserIdByEmailAsync(string email);
        Task<bool> IsUserExistAsync(int accountId);
        Task<ActionResult<User>> GetUserByEmailAsync(string email);
    }
}
