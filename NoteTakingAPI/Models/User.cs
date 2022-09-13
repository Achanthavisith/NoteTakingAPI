using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NoteTakingAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        [JsonIgnore]
        public string? Name { get; set; }

        [JsonIgnore]
        public string? LastName { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }

        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<Note>? Notes { get; set; }
        [JsonIgnore]
        public ICollection<Request>? Requests { get; set; }

        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    }
}
