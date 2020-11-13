using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CONTPAQ_API.Models.DB
{
    public partial class PlantillasContext : DbContext
    {
        public PlantillasContext()
        {
        }

        public PlantillasContext(DbContextOptions<PlantillasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cabeceras> Cabeceras { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<Movimientos> Movimientos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-0IF7KH8\\COMPAC;Initial Catalog=Plantillas;User ID=sa;Password=Supervisor1.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cabeceras>(entity =>
            {
                entity.HasKey(e => e.Documentoid)
                    .HasName("Pk_Cabecera");

                entity.Property(e => e.Documentoid).ValueGeneratedNever();

                entity.Property(e => e.CodConcepto)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CodigoCteProv)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Serie).HasMaxLength(50);

                entity.HasOne(d => d.Documento)
                    .WithOne(p => p.Cabeceras)
                    .HasForeignKey<Cabeceras>(d => d.Documentoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cabeceras__Docum__4D94879B");
            });

            modelBuilder.Entity<Documentos>(entity =>
            {
                entity.HasKey(e => e.Documentoid)
                    .HasName("Pk_Documento");

                entity.Property(e => e.Documentoid).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).HasMaxLength(400);

                entity.Property(e => e.ProximaFactura).HasColumnType("datetime");

                entity.Property(e => e.UltimaVezFacturada).HasColumnType("datetime");
            });

            modelBuilder.Entity<Movimientos>(entity =>
            {
                entity.HasKey(e => new { e.Documentoid, e.NumeroMovimiento })
                    .HasName("Pk_Movimientos");

                entity.Property(e => e.CodAlmacen)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CodProducto)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.Documentoid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Movimient__Docum__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
