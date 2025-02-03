using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Schemas
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool IsPublic { get; set; }
        public string Link { get; set; }
    }
}
