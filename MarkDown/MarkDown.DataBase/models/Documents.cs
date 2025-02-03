using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown.DataBase.models
{
    public class Documents
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string NameFile { get; set; }

        public string Text { get; set; }

        public Users Users { get; set; }

        public static Documents Create(Guid Id, Guid userId, string nameFile, string Text, Users users) 
        {
            return new Documents
            {
                Id = Id,
                UserId = userId,
                NameFile = nameFile,
                Text = Text,
                Users = users
            };
        }
    }
}
