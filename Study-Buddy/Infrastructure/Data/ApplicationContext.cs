using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
               .UseLazyLoadingProxies()
               .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Sets a default value for the created field on the database level
            builder.Entity<Message>()
                .Property(msg => msg.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Room>()
                .Property(room => room.Created)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<RoomParticipant>()
                .HasKey(rp => new { rp.RoomId, rp.ParticipantId }); // Composite key

            builder.Entity<RoomParticipant>()
                .HasOne(rp => rp.Room)
                .WithMany(r => r.Participants)
                .HasForeignKey(rp => rp.RoomId);

            builder.Entity<RoomParticipant>()
                .HasOne(rp => rp.Participant)
                .WithMany(u => u.Rooms)
                .HasForeignKey(rp => rp.ParticipantId);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Room && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((Room)entry.Entity).Updated = DateTime.Now;
            }
        }

    }
}


