using AutoPlusPlusMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoPlusPlusMVC.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<listing_remembrance> Listing_remembrance { get; set; }
        public DbSet<Inspection> Inspection { get; set; }
        public DbSet<Inspector_times> Inspector_times { get; set; }
        public DbSet<Balance_payments> Balance_payments { get; set; }
        public DbSet<Forum_topic> Forum_topic { get; set; }
        public DbSet<Forum_topic_remembrance> Forum_topic_remembrance { get; set; }
        public DbSet<Forum_answer> Forum_answer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => new { x.id_User });
            modelBuilder.Entity<Photo>().HasKey(x => new { x.id_Photo });
            modelBuilder.Entity<listing_remembrance>().HasKey(x => new { x.id_Listing_remembrance });
            modelBuilder.Entity<Listing>().HasMany(x => x.photos).WithOne(x => x.Listing).IsRequired();
            modelBuilder.Entity<User>().HasMany(x => x.times).WithOne(x => x.fk_User).IsRequired();
            modelBuilder.Entity<Listing>().HasMany(x => x.remembrances).WithOne(x => x.Listing).IsRequired();
            modelBuilder.Entity<Forum_topic>().HasMany(x => x.answers).WithOne(x => x.fk_Forum_topic).IsRequired();
            //modelBuilder.Entity<Inspector_times>().HasOne(x => x.fk_User).WithOne(x => x.inspector).IsRequired();
            //modelBuilder.Entity<Inspection>().HasOne(x => x.fk_User).WithOne(x => x.user).IsRequired();
            //modelBuilder.Entity<Inspection>().HasOne(x => x.fk_Inspector).WithOne(x => x.inspector1).IsRequired();
            //modelBuilder.Entity<Inspection>().HasOne(x => x.fk_Listing).WithOne(x => x.inspection).IsRequired();
        }
    }
}
