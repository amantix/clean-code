using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Utils;
using Core.Models;

namespace Application.Services;

public class UserService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IJwtProvider jwtProvider)
    : IUserService
{
    public async Task<Result<bool>> ExistById(Guid id)
    {
        var existResult = await userRepository.Exist(id);
        
        return existResult.IsFailure
            ? Result<bool>.Failure(existResult.Message!)
            : Result<bool>.Success(existResult.Data);
    }

    public async Task<Result> Register(string username, string password)
    {
        var hashedPassword = passwordHasher.Generate(password);

        var repositoryResult = await userRepository.Create(Guid.NewGuid(), username, hashedPassword);

        return repositoryResult.IsFailure
            ? Result.Failure(repositoryResult.Message)
            : Result.Success();
    }

    public async Task<Result<string>> Login(string userName, string passwordHash)
    {
        var repositoryResult = await userRepository.GetByLogin(userName);

        if (repositoryResult.IsFailure)
        {
            return Result<string>.Failure(repositoryResult.Message!);
        }

        var user = repositoryResult.Data;
        var verificationResult = passwordHasher.Verify(passwordHash, user!.Password);
        if (!verificationResult)
        {
            return Result<string>.Failure("Неверный пароль");
        }

        var token = jwtProvider.GenerateToken(user);

        return Result<string>.Success(token);
    }

    public async Task<Result<User>> GetByLogin(string login)
    {
        var repositoryResult = await userRepository.GetByLogin(login);
        return repositoryResult.IsFailure
            ? Result<User>.Failure(repositoryResult.Message!)
            : Result<User>.Success(repositoryResult.Data!);
    }
}