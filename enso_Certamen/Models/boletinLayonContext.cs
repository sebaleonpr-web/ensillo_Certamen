using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

public partial class boletinLayonContext : DbContext
{
    public boletinLayonContext()
    {
    }

    public boletinLayonContext(DbContextOptions<boletinLayonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BoletinGeneral> BoletinGenerals { get; set; }

    public virtual DbSet<ComentarioGeneral> ComentarioGenerals { get; set; }

    public virtual DbSet<NoticiaGeneral> NoticiaGenerals { get; set; }

    public virtual DbSet<RolGeneral> RolGenerals { get; set; }

    public virtual DbSet<SuscripcionGeneral> SuscripcionGenerals { get; set; }

    public virtual DbSet<UsuariosGeneral> UsuariosGenerals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoletinGeneral>(entity =>
        {
            entity.HasKey(e => e.IdBoletin).HasName("PK__boletinG__7E13FAF7BC1064BB");

            entity.ToTable("boletinGeneral");

            entity.Property(e => e.IdBoletin)
                .ValueGeneratedNever()
                .HasColumnName("idBoletin");
            entity.Property(e => e.DescripcionBoletin)
                .HasColumnType("text")
                .HasColumnName("descripcionBoletin");
            entity.Property(e => e.FechaBoletin)
                .HasColumnType("datetime")
                .HasColumnName("fechaBoletin");
            entity.Property(e => e.IdNoticia).HasColumnName("idNoticia");
            entity.Property(e => e.TituloBoletin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tituloBoletin");

            entity.HasOne(d => d.IdNoticiaNavigation).WithMany(p => p.BoletinGenerals)
                .HasForeignKey(d => d.IdNoticia)
                .HasConstraintName("FK__boletinGe__idNot__44FF419A");
        });

        modelBuilder.Entity<ComentarioGeneral>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__comentar__C74515DA4665DA84");

            entity.ToTable("comentarioGeneral");

            entity.Property(e => e.IdComentario)
                .ValueGeneratedNever()
                .HasColumnName("idComentario");
            entity.Property(e => e.ContenidoComentario)
                .HasColumnType("text")
                .HasColumnName("contenidoComentario");
            entity.Property(e => e.EmailLectorComentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emailLectorComentario");
            entity.Property(e => e.FechaComentario)
                .HasColumnType("datetime")
                .HasColumnName("fechaComentario");
            entity.Property(e => e.IdNoticia).HasColumnName("idNoticia");
            entity.Property(e => e.NombrelectorComentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombrelectorComentario");

            entity.HasOne(d => d.IdNoticiaNavigation).WithMany(p => p.ComentarioGenerals)
                .HasForeignKey(d => d.IdNoticia)
                .HasConstraintName("FK__comentari__idNot__4222D4EF");
        });

        modelBuilder.Entity<NoticiaGeneral>(entity =>
        {
            entity.HasKey(e => e.IdNoticia).HasName("PK__noticiaG__F682B59E28F03A23");

            entity.ToTable("noticiaGeneral");

            entity.HasIndex(e => e.TituloNoticia, "UQ__noticiaG__3FD5306E05AE2D0E").IsUnique();

            entity.Property(e => e.IdNoticia)
                .ValueGeneratedNever()
                .HasColumnName("idNoticia");
            entity.Property(e => e.ContenidoNoticia)
                .HasColumnType("text")
                .HasColumnName("contenidoNoticia");
            entity.Property(e => e.FechaNoticia)
                .HasColumnType("datetime")
                .HasColumnName("fechaNoticia");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.ResumenNoticia)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("resumenNoticia");
            entity.Property(e => e.TituloNoticia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tituloNoticia");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.NoticiaGenerals)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__noticiaGe__idUse__3F466844");
        });

        modelBuilder.Entity<RolGeneral>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__rolGener__3C872F76A605C8CC");

            entity.ToTable("rolGeneral");

            entity.HasIndex(e => e.NombreRol, "UQ__rolGener__2787B00CE1CDAF0B").IsUnique();

            entity.Property(e => e.IdRol)
                .ValueGeneratedNever()
                .HasColumnName("idRol");
            entity.Property(e => e.DescripRol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<SuscripcionGeneral>(entity =>
        {
            entity.HasKey(e => e.IdSuscripcion).HasName("PK__suscripc__B00C839B8FD1B420");

            entity.ToTable("suscripcionGeneral");

            entity.Property(e => e.IdSuscripcion)
                .ValueGeneratedNever()
                .HasColumnName("idSuscripcion");
            entity.Property(e => e.EmailSuscripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emailSuscripcion");
            entity.Property(e => e.FechaSuscripcion)
                .HasColumnType("datetime")
                .HasColumnName("fechaSuscripcion");
            entity.Property(e => e.IdBoletin).HasColumnName("idBoletin");
            entity.Property(e => e.NombreSuscripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreSuscripcion");

            entity.HasOne(d => d.IdBoletinNavigation).WithMany(p => p.SuscripcionGenerals)
                .HasForeignKey(d => d.IdBoletin)
                .HasConstraintName("FK__suscripci__idBol__47DBAE45");
        });

        modelBuilder.Entity<UsuariosGeneral>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__usuarios__3717C98289C5971E");

            entity.ToTable("usuariosGeneral");

            entity.HasIndex(e => e.EmailUser, "UQ__usuarios__AF638C4CFE55DE30").IsUnique();

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("idUser");
            entity.Property(e => e.ApellidoUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoUser");
            entity.Property(e => e.ContraUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contraUser");
            entity.Property(e => e.EmailUser)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("emailUser");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreUser");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuariosGenerals)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuariosG__idRol__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
