using WebAppMarkdown.Core.Models;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Interfaces;

public interface IUserRepository
{
    Task<UserEntity> GetUserByIdAsync(Guid id);
    Task<UserEntity> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
}