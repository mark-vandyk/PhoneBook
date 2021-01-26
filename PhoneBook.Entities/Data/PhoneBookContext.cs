using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Entities.Data
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions<PhoneBookContext> options) : base(options)
        {
        }

        public DbSet<Models.PhoneBook> PhoneBooks { get; set; }
        public DbSet<Models.Entry> Entries { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Models.PhoneBook>().ToTable("PhoneBooks");
        //    modelBuilder.Entity<Models.Entry>().ToTable("Entries");
        //}
    }
}
