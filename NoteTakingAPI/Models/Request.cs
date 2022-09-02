using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class Request
    {
        public int RequestId { get; set; }

        public User User { get; set; }

        public int ReceiverId { get; set; }

        public bool IsAccepted { get; set; } = false;
    }
}
