namespace NoteTakingAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }    

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Note> Notes { get; set; }

        public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    }
}
