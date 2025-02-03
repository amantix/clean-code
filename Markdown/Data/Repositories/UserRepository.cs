using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Abstracts;
using Data.Utils;

namespace Data.Repositories
{
    public class UserRepository(ApplicationContext dbContext) : IUserRepository
    {
        public async Task<ActionResult> AddUserAsync(AEntity entity)
        {
            if (entity is User user){

                var isAccExists = await IsUserExistsByEmailAsync(user.Email!);

                if (isAccExists)
                    return ActionResult.Fail($"User {user.Email} not available");

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                    return ActionResult.Ok();
            }
            return ActionResult.Fail("Noncorrect entity");
        }

        public async Task<ActionResult<User?>> GetByEmailAsync(string email)
        {
            var userEntity = await dbContext.Users
                .FirstOrDefaultAsync(a => a.Email == email);

            if (userEntity == null)
                return ActionResult<User?>.Fail("User with this email dont exist!");


            return ActionResult<User>.Ok(userEntity)!;
        }

        public async Task<ActionResult<User>> GetByAccountIdAsync(int userId)
        {
            var userEntity = await dbContext.Users
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (userEntity == null)
                return ActionResult<User?>.Fail("User with this email dont exist!")!;
           

            return ActionResult<User>.Ok(userEntity)!;

        }

        public async Task<bool> IsUserExistsByIdAsync(int userId)
        {
            return await dbContext.Users.AnyAsync(a => a.Id == userId);
        }

        private async Task<bool> IsUserExistsByEmailAsync(string email)
        {
            return await dbContext.Users.AnyAsync(a => a.Email == email);
        }

        public async Task<ActionResult<User>> GetUserByEmailAsync(string email)
        {
            var userEntity = await dbContext.Users
                            .FirstOrDefaultAsync(a => a.Email == email);

            if (userEntity == null)
                return ActionResult<User?>.Fail("User with this email dont exist!")!;


            return ActionResult<User>.Ok(userEntity)!;
        }
    }
}
