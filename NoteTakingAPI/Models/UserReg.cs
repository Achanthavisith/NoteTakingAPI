using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace NoteTakingAPI.Models
{
    public class UserReg
    {
        public string Email { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string Password { get; set; }
    }
}
