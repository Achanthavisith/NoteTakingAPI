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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserNames>()
                //each user will ONLY have one username, with one is empty because there is no parameter in Users to relate to. Ef will recognize this still 
                .HasOne(b => b.User).WithOne();

            modelBuilder.Entity<FriendList>()
                .HasOne(b => b.User).WithOne();
        }
    }
}
