namespace NoteTakingAPI.Models
{
    public class FriendList
    {
        public int FriendListId { get; set; }
        public int UserId { get; set; }
        public List<int> Friends { get; set; } = new List<int>();
        public User? User { get; set; }
    }
}