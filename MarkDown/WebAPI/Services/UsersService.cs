using MarkDown.DataBase.models;
using MarkDown.DataBase.repository;
using MarkDown.Infastructure;
using System.Security.Claims;

namespace WebAPI.Services
{
    public class UsersService
    {
        private readonly PasswordHasher _passwordHasher;
        private readonly UsersRepository _usersRepository;
        private readonly JwtProvider _jwtProvider;

        public UsersService(
            UsersRepository usersRepository,
            PasswordHasher passwordHasher,
            JwtProvider jwtProvider) 
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }
        public async Task Register(string email, string password) 
        {
            var hashedPassword = _passwordHasher.GenerateTokenSHA(password);

            var user = Users.Create(Guid.NewGuid(), email, hashedPassword);

            await _usersRepository.Add(user);
        }

        public async Task<string> Login(string email, string password) 
        {
            var user = await _usersRepository.GetByEmail(email);

            var result = _passwordHasher.Verify(password, user.Password);

            if (!result) throw new Exception("Неверный пароль");

            if (user == null) throw new Exception("Нет такой почты");

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<Guid> GetUserIdByToken(ClaimsPrincipal claims) 
        {
            var userId = claims.Claims.FirstOrDefault(x => x.Type == "userid");
            if (userId == null) 
            {
                throw new Exception("Не авторизованный пользователь");
            }
            return Guid.Parse(userId.Value);
        }

        public async Task<Users> GetEntityUserById(Guid userId) 
        {
            return await _usersRepository.GetById(userId);
        }
    }
}
