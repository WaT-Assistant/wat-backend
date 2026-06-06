using Microsoft.EntityFrameworkCore;
using WatApi.Models;

namespace WatApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.JobOffer)
                .WithOne(u => u.User)
                .HasForeignKey<JobOffer>(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOffer>()
            .Property(j => j.Status)
            .HasConversion<string>();
        }
    }
}
