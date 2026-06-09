using Microsoft.EntityFrameworkCore;
using WatApi.Models;

namespace WatApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<ImportantInfo> ImportantInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.JobOffer)
                .WithOne(u => u.User)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOffer>()
            .Property(j => j.Status)
            .HasConversion<string>();

            modelBuilder.Entity<JobOffer>()
                .HasOne(j => j.ImportantInfo)
                .WithOne(i => i.JobOffer)
                .HasForeignKey<ImportantInfo>(i => i.JobOfferId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
