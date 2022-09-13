using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NoteTakingAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        [JsonIgnore]
        private ICollection<Note>? Notes { get; set; }
        [JsonIgnore]
        private ICollection<Request>? Requests { get; set; }

        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    }
}
