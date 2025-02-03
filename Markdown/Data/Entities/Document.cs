using Data.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Document : AEntity
    {
        [Required]
        public string Title { get; set; }
        public bool isPublic { get; set; } = false;
        public string? Link { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
