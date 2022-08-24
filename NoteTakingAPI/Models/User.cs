using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    }
}
