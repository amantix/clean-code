using MarkDown.DataBase.models;
using Microsoft.EntityFrameworkCore;

namespace MarkDown.DataBase.repository
{
    public class UsersRepository
    {
        private readonly MyDbContext _dbContext;

        public UsersRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Users>> GetUsers() 
        {
            return await _dbContext.Users
                .ToListAsync();
        }

        public async Task<Users> GetById(Guid userId) 
        {
             var user = _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == userId);

            return new Users
            {
                Id = userId
            };
        }

        public async Task<Users> GetByEmail(string email) 
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task Add(Users user) 
        {
            var userEntity = new Users
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
            };

            await _dbContext.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id) 
        {
            await _dbContext.Users
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();
        }

    }
}
