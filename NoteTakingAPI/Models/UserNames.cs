using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingAPI.Models
{
    public class UserNames
    {
        [ForeignKey("User")]
        public int UserNamesId { get; set; }

        public string UserName { get; set; }

        public virtual User User { get; set; }
    }
}
