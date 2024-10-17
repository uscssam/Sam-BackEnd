using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SAM.Entities;

namespace SAM.Repositories.Database.Context
{
    public class MySqlContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MySqlContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("MySqlConnection");
            optionsBuilder.UseMySQL(connectionString!);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela Machine
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("Machines");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdMachine");
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Status)
                        .IsRequired()
                        .HasConversion<int>();
                entity.Property(e => e.LastMaintenance);

                // Configuração da relação com a tabela Unit
                entity.HasOne<Unit>()
                      .WithMany()
                      .HasForeignKey(m => m.IdUnit)
                      .IsRequired();
            });

            // Configuração da tabela OrderService
            modelBuilder.Entity<OrderService>(entity =>
            {
                entity.ToTable("OrderServices");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdOrderService");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>();
                entity.Property(e => e.Opening);
                entity.Property(e => e.Closed);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(os => os.CreatedBy)
                      .IsRequired();

                // Configuração da relação com a tabela Machine
                entity.HasOne<Machine>()
                  .WithMany()
                  .HasForeignKey(x => x.IdMachine)
                  .IsRequired();

                entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(x => x.IdTechnician)
                  .IsRequired();
            });

            // Configuração da tabela Unit
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Units");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdUnit");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Street).HasMaxLength(50);
                entity.Property(e => e.Neighborhood).HasMaxLength(50);
                entity.Property(e => e.CEP).HasMaxLength(20);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            // Configuração da tabela User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("IdUser");
                entity.Property(e => e.UserName).HasMaxLength(50);
                entity.Property(e => e.Fullname).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(20);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasConversion<int>();
                entity.Property(e => e.Speciality)
                    .HasConversion<int>();
            });
        }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<User> User { get; set; }
    }
}
