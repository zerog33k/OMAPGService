using System;
using Microsoft.EntityFrameworkCore;

namespace OMAPGServiceData.Models
{
	public class OMAPGContext : DbContext
	{
		public OMAPGContext(DbContextOptions<OMAPGContext> options) : base(options)
		{
		}

        public OMAPGContext() : base()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectString != null)
            {
                optionsBuilder.UseNpgsql(ConnectString);
            }
        }

        public string ConnectString { get; set; }

		public DbSet<Device> Devices { get; set; }
        public DbSet<Pokemon>Pokemon { get; set; }
        public DbSet<Raid> Raids { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Device>().HasKey(d => d.Id);
            modelBuilder.Entity<Device>().Property<DateTime>("UpdatedTimestamp").HasDefaultValue();
            modelBuilder.Entity<Pokemon>().HasKey(p => p.idValue);
            modelBuilder.Entity<Pokemon>().Property<DateTime>("UpdatedTimestamp");
            modelBuilder.Entity<Raid>().HasKey(r => r.id);
            modelBuilder.Entity<Raid>().Property<DateTime>("UpdatedTimestamp");
            modelBuilder.Entity<Gym>().HasKey(g => g.id);
            modelBuilder.Entity<Gym>().Property<DateTime>("UpdatedTimestamp");
            modelBuilder.Entity<Notification>().HasKey(n => n.NotifyId);

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			ChangeTracker.DetectChanges();

			return base.SaveChanges();
		}
	}
}
