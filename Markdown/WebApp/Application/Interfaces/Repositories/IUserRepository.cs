using Application.Utils;
using Core.Models;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<List<User>>> GetAll();
    Task<Result<User>> GetById(Guid id);
    Task<Result<User>> GetByLogin(string login);
    Task<Result<bool>> Exist(Guid id);

    Task<Result> Create(Guid userId, string userName, string passwordHash);
    Task<Result> AddDocument(Guid userId, Guid documentId, string documentName);
}