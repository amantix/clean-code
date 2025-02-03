using Data.Abstracts;
using Data.Utils;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService(IUserRepository userRepository, JWTService jwtService) : IUserService
    {
        private static PasswordHasher<User> hasher = new PasswordHasher<User>();
        public async Task<ActionResult<string>> AuthorizeAsync(string email, string password)
        {
            var res = await userRepository.GetByEmailAsync(email);

            if (!res.IsSuccess)
                return ActionResult<string>.Fail(res.ErrorMessage!)!;

            var account = res.Data;

            if (hasher.VerifyHashedPassword(account!, account!.Password!, password) != PasswordVerificationResult.Success)
            {
                return ActionResult<string>.Fail("Invalid password!"!)!;
            }

            var token = jwtService.GenerateToken(account!);
            return ActionResult<string>.Ok(token);
        }

        public async Task<ActionResult<string>> RegistrationAsync(string email, string password, string username)
        {
            if(email == "")
            {
                return ActionResult<string>.Fail("Email is empty")!;
            }
            User user = new()
            {
                Email = email,
                Username = username
            };

            user.Password = hasher.HashPassword(user, password);

            if ((await userRepository.AddUserAsync(user)) is ActionResult res && !res.IsSuccess)
            {
                return ActionResult<string>.Fail(res.ErrorMessage!)!;
            }
            return ActionResult<string>.Ok(jwtService.GenerateToken(user!));
        }

        public async Task<ActionResult<int>> GetUserIdByEmailAsync(string email)
        {
            return (await userRepository.GetByEmailAsync(email)).withData(x => x.Id);
        }

        public async Task<bool> IsUserExistAsync(int userId)
        {
            return await userRepository.IsUserExistsByIdAsync(userId);
        }

        public async Task<ActionResult<User>> GetUserByEmailAsync(string email)
        {
            return await userRepository.GetUserByEmailAsync(email);
        }
    }
}
