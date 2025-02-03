using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.Infastructure
{
    public class PasswordHasher
    {
        public string GenerateTokenSHA(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashedPassword) => 
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
