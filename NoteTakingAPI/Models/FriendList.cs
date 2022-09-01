using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class FriendList
    {
        [ForeignKey("User")]
        public int FriendListId { get; set; }
        public List<int> Friends { get; set; } = new List<int>();
        public virtual User User { get; set; }
    }
}