using Application.ResponseResult;
using Domain.Models;

namespace Application.Abstract.Services;

public interface IUserService
{
    Task<Result> AddUser(string userName, string userEmail, string password);
    Task<Result> AuthenticateUserByEmail(string userEmail, string password);
    Task<Result> GetUserById(Guid userId);
    Task<Result> GetUserByEmail(string email);
}