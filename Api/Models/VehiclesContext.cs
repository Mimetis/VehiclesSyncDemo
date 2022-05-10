using Microsoft.EntityFrameworkCore;

namespace Api.Models
{

    public partial class VehiclesContext : DbContext
    {
        internal bool useSeeding = true;
        public string ConnectionString { get; }

        public VehiclesContext(DbContextOptions<VehiclesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vehicle> Vehicle { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

        }

    }
}
