using Application.Abstract.Repositories;
using Application.Abstract.Services;
using Application.ResponseResult;
using Domain.Models;

namespace Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher hasher,
    IJwtWorker worker): IUserService
{
    public async Task<Result> AddUser(string userName, string userEmail, string password)
    {
        var user = await userRepository.GetUserByEmail(userEmail);
        if (user != null)
        {
            return Result.Failure(new Error("User already exists", ErrorType.BadRequest));
        }
        
        string passwordHash = hasher.Hash(password);

        var newUser = new User
        {
            Name = userName,
            Email = userEmail,
            PasswordHash = passwordHash,
        };

        var result = await userRepository.CreateUser(newUser);

        if (!result)
        {
            return Result.Failure(new Error("Failed to create user", ErrorType.ServerError));
        }
        
        return Result.Success();
    }

    public async Task<Result> AuthenticateUserByEmail(string userEmail, string password)
    {
        var user = await userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            return Result.Failure(new Error("User not found", ErrorType.NotFound));
        }
        
        var isValidPassword = hasher.Validate(user.PasswordHash, password);

        if (!isValidPassword)
        {
            return Result.Failure(new Error("Invalid password", ErrorType.AuthenticationError));
        }

        var token = worker.GenerateJwtToken(user);

        if (token == null)
        {
            return Result.Failure(new Error("Сould not create an access token", ErrorType.ServerError));
        }
        
        return Result<string>.Success(token);
    }

    public async Task<Result> GetUserById(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            return Result.Failure(new Error("User not found", ErrorType.NotFound));
        }
        return Result<User>.Success(user);
    }

    public async Task<Result> GetUserByEmail(string email)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user == null)
        {
            return Result.Failure(new Error("User not found", ErrorType.NotFound));
        }
        return Result<User>.Success(user);
    }
}