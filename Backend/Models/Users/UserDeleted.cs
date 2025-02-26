using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Users
{
    public class UserDeleted
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}