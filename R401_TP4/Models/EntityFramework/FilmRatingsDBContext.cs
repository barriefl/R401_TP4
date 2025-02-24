using Microsoft.EntityFrameworkCore;

namespace R401_TP4.Models.EntityFramework
{
    public partial class FilmRatingsDBContext : DbContext
    {
        public FilmRatingsDBContext()
        {
        }

        public FilmRatingsDBContext(DbContextOptions<FilmRatingsDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Film> Films { get; set; }

        public virtual DbSet<Notation> Notations { get; set; }

        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=localhost; port=5432; Database=FilmsDB; uid=postgres; password=postgres;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.FilmId).HasName("pk_ser");
            });

            modelBuilder.Entity<Notation>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurId, e.FilmId } ).HasName("pk_not");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.UtilisateurId).HasName("pk_utl");
                entity.Property(e => e.Pays)
                    .HasDefaultValue("France");
                entity.Property(e => e.DateCreation)
                    .HasDefaultValueSql("now()");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
