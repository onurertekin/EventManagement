using DatabaseModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseModel
{
    public class MainDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Participant> Participants { get; set; }


        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Intermediate Tables

            #region Categories_Products

            modelBuilder.Entity<Event>().HasMany(r => r.Participants).WithMany(a => a.Events)
                .UsingEntity<Dictionary<string, object>>("Events_Participants",
                    j => j.HasOne<Participant>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.ClientCascade),
                    j => j.HasOne<Event>().WithMany().HasForeignKey("ParticipantId").OnDelete(DeleteBehavior.ClientCascade));

            #endregion

            #endregion

        }
    }
}
