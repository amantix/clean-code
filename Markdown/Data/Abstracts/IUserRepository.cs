using Data.Entities;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstracts
{
    public interface IUserRepository
    {
        Task<ActionResult> AddUserAsync(AEntity entity);
        Task<ActionResult<User?>> GetByEmailAsync(string email);
        Task<bool> IsUserExistsByIdAsync(int userId);
        Task<ActionResult<User>> GetByAccountIdAsync(int userId);
        Task<ActionResult<User>> GetUserByEmailAsync(string email);
    }
}
