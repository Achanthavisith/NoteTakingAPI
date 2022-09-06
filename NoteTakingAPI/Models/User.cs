using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }    

        public string Name { get; set; }

        public string LastName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        private ICollection<Note> Notes { get; set; }

        private ICollection<Request> Requests { get; set; }

        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    }
}
