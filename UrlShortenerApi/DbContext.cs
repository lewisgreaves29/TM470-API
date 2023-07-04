using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi
{
    public class MyDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FallBackUrls> FallBackUrls { get; set; }
        public DbSet<UrlExclusion> UrlExclusions { get; set; }
        public DbSet<CustomDomain> CustomDomains { get; set; }

        public DbSet<Url> Urls { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=lg-sql-uni.mysql.database.azure.com; Port=3306; Database=lg-sql-db-uni; Uid=mysqladmin@lg-sql-uni; Pwd=H@Sh1CoR3!; SslMode=Preferred;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>()
                .HasOne(a => a.FallBackUrls)
                .WithOne(f => f.Account)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.UrlExclusions)
                .WithOne(u => u.Account)
                .HasForeignKey(u => u.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.CustomDomains)
                .WithOne(c => c.Account)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Urls)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
