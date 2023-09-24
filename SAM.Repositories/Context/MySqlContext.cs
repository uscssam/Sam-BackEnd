using Microsoft.EntityFrameworkCore;
using SAM.Entities;

namespace SAM.Repositories.Database.Context
{
    public class MySqlContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=localhost;Database=SAM;User=root;Password=123456;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela Machine
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("Machines");
                entity.HasKey(e => e.IdMachine);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.LastMaintenance);
                entity.Property(e => e.Preventive);
            });

            // Configuração da tabela OrderService
            modelBuilder.Entity<OrderService>(entity =>
            {
                entity.ToTable("OrderServices");
                entity.HasKey(e => e.IdOrderService);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>();
                entity.Property(e => e.Opening);
                entity.Property(e => e.Closed);

                // Configuração da relação com a tabela Unit
                entity.HasOne(os => os.Unit);

                // Configuração da relação com a tabela Machine
                entity.HasOne(os => os.Machine);
            });

            // Configuração da tabela Unit
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Units");
                entity.HasKey(e => e.IdUnit);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Road).HasMaxLength(50);
                entity.Property(e => e.Neighborhood).HasMaxLength(50);
                entity.Property(e => e.CEP).HasMaxLength(20);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Configuração da tabela User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.IdUser);
                entity.Property(e => e.UserName).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(20);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasConversion<int>();
                entity.Property(e => e.Speciality)
                    .HasConversion<int>();
                entity.Property(e => e.Available);
            });
        }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<User> User { get; set; }
    }
}
