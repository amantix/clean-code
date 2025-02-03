namespace MarkDown.DataBase.models
{
    public class Users
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<Documents> Documents { get; set; }

        public static Users Create(Guid id, string email, string password) 
        {
            return new Users
            {
                Id = id,
                Email = email,
                Password = password
            };
        }
    }
}
