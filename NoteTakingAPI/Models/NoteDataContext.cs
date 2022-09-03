using Microsoft.EntityFrameworkCore;

namespace NoteTakingAPI.Models
{
    public class NoteDataContext : DbContext
    {
        public NoteDataContext(DbContextOptions<NoteDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<FriendList> FriendLists { get; set; }

        public DbSet<UserNames> UserNames { get; set; }

        public DbSet<Request> Requests { get; set; }   
    }
}
