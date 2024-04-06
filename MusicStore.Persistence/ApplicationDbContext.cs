using Microsoft.EntityFrameworkCore;
using MusicStore.Entities.Info;
using System.Reflection;

namespace MusicStore.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ConcertInfo>().HasNoKey();
        }
        //Entities to tables
       // public DbSet<Genre> Genres { get; set; }
    }
}
