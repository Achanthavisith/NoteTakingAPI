using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class FriendList
    {
        public int FriendListId { get; set; }
        public int UserId { get; set; }//Fk
        public User User { get; set; }//this FK will relate back to the UserId;
        public List<int> Friends { get; set; } = new List<int>();
    }
}