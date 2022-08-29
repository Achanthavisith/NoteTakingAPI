namespace NoteTakingAPI.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        public string Subject { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }//Fk

        public List<int>? SharedUsers { get; set; } = new List<int>();

        public bool IsPublic { get; set; } = false;

        public User User { get; set; }//this FK will relate back to the UserId;
    }
}
