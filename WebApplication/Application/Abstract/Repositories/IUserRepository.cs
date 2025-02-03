using Domain.Models;

namespace Application.Abstract.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUserById(Guid userId);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
}