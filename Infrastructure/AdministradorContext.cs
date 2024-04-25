using Adm.Domain;
using dotenv.net;
using Microsoft.EntityFrameworkCore;

namespace Adm.Infrastructure
{
    public partial class AdministradorContext : DbContext
    {
        private readonly IDictionary<string, string> _envVariables;

        public AdministradorContext(DbContextOptions<AdministradorContext> options)
            : base(options)
        {
            DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
            _envVariables = DotEnv.Read();
        }

        public virtual DbSet<Administrador> Administradors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine(_envVariables["connectionString"]);
            optionsBuilder.UseSqlServer(_envVariables["connectionString"]);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Admnistr__3214EC07C30A476E");

                entity.ToTable("Administrador");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .IsUnicode(false);
                entity.Property(e => e.Nome)
                    .HasMaxLength(45)
                    .IsUnicode(false);
                entity.Property(e => e.Senha)
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
