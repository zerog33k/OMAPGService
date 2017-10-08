using System;
using Microsoft.EntityFrameworkCore;

namespace OMAPGService.Models
{
	public class OMAPGContext : DbContext
	{
		public OMAPGContext(DbContextOptions<OMAPGContext> options) : base(options)
		{
		}

		public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Device>().HasKey(d => d.Id);

			// shadow properties
            modelBuilder.Entity<Device>().Property<DateTime>("UpdatedTimestamp");

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			ChangeTracker.DetectChanges();



			return base.SaveChanges();
		}
	}
}
