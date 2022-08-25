namespace NoteTakingAPI.Models
{
    public class UserNames
    {
        public int UserNamesId { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }//Fk

        public User User { get; set; }//this FK will relate back to the UserId;
    }
}
