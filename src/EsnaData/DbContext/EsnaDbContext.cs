namespace EsnaData.DbContexts
{
    using EsnaData.Entities;

    using Microsoft.EntityFrameworkCore;

    public class EsnaDbContext : DbContext
    {
        public EsnaDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Command> Commands { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Recorde> Recordes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>().HasKey(x => x.Id);
            modelBuilder.Entity<Command>().HasKey(x => x.Id);
            modelBuilder.Entity<Configuration>().HasKey(x => x.Id);
            modelBuilder.Entity<Recorde>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}