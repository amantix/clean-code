using Application.Utils;
using Core.Models;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<bool>> ExistById(Guid id);
    Task<Result> Register(string userName, string passwordHash);
    Task<Result<string>> Login(string userName, string passwordHash);
    public Task<Result<User>> GetByLogin(string login);
}