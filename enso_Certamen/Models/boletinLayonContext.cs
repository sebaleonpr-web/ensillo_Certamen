using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using enso_Certamen.Models;

namespace enso_Certamen.Data;

public partial class BoletinLayonContext : DbContext
{
    public BoletinLayonContext()
    {
    }

    public BoletinLayonContext(DbContextOptions<BoletinLayonContext> options)
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
            entity.HasKey(e => e.GuidBoletin);

            entity.ToTable("boletinGeneral");

            entity.HasIndex(e => e.GuidNoticia, "IX_boletinGeneral_GuidNoticia");

            entity.Property(e => e.GuidBoletin).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.DescripcionBoletin)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.FechaBoletin)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.TituloBoletin)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.GuidNoticiaNavigation).WithMany(p => p.BoletinGenerals)
                .HasForeignKey(d => d.GuidNoticia)
                .HasConstraintName("FK_boletinGeneral_Noticia");
        });

        modelBuilder.Entity<ComentarioGeneral>(entity =>
        {
            entity.HasKey(e => e.GuidComentario);

            entity.ToTable("comentarioGeneral");

            entity.HasIndex(e => e.GuidNoticia, "IX_comentarioGeneral_GuidNoticia");

            entity.Property(e => e.GuidComentario).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ContenidoComentario)
                .HasColumnType("text")
                .HasColumnName("contenidoComentario");
            entity.Property(e => e.EmailLectorComentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("emailLectorComentario");
            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaComentario");
            entity.Property(e => e.NombrelectorComentario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombrelectorComentario");

            entity.HasOne(d => d.GuidNoticiaNavigation).WithMany(p => p.ComentarioGenerals)
                .HasForeignKey(d => d.GuidNoticia)
                .HasConstraintName("FK_comentarioGeneral_Noticia");
        });

        modelBuilder.Entity<NoticiaGeneral>(entity =>
        {
            entity.HasKey(e => e.GuidNoticia);

            entity.ToTable("noticiaGeneral");

            entity.HasIndex(e => e.GuidUsuario, "IX_noticiaGeneral_GuidUsuario");

            entity.Property(e => e.GuidNoticia).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ContenidoNoticia)
                .HasDefaultValue("")
                .HasColumnType("text")
                .HasColumnName("contenidoNoticia");
            entity.Property(e => e.FechaNoticia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaNoticia");
            entity.Property(e => e.ResumenNoticia)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("resumenNoticia");
            entity.Property(e => e.TituloNoticia)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("tituloNoticia");

            entity.HasOne(d => d.GuidUsuarioNavigation).WithMany(p => p.NoticiaGenerals)
                .HasForeignKey(d => d.GuidUsuario)
                .HasConstraintName("FK_noticiaGeneral_Usuario");
        });

        modelBuilder.Entity<RolGeneral>(entity =>
        {
            entity.HasKey(e => e.GuidRol);

            entity.ToTable("rolGeneral");

            entity.Property(e => e.GuidRol).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.DescripRol)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("descripRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<SuscripcionGeneral>(entity =>
        {
            entity.HasKey(e => e.GuidSuscripcion);

            entity.ToTable("suscripcionGeneral");

            entity.HasIndex(e => e.GuidBoletin, "IX_suscripcionGeneral_GuidBoletin");

            entity.Property(e => e.GuidSuscripcion).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.EmailSuscripcion).HasColumnName("emailSuscripcion");
            entity.Property(e => e.FechaSuscripcion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaSuscripcion");
            entity.Property(e => e.NombreSuscripcion).HasColumnName("nombreSuscripcion");

            entity.HasOne(d => d.GuidBoletinNavigation).WithMany(p => p.SuscripcionGenerals)
                .HasForeignKey(d => d.GuidBoletin)
                .HasConstraintName("FK_suscripcionGeneral_Boletin");
        });

        modelBuilder.Entity<UsuariosGeneral>(entity =>
        {
            entity.HasKey(e => e.GuidUsuario);

            entity.ToTable("usuariosGeneral");

            entity.HasIndex(e => e.GuidRol, "IX_usuariosGeneral_GuidRol");

            entity.Property(e => e.GuidUsuario).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.ApellidoUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoUser");
            entity.Property(e => e.ContraUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contraUser");
            entity.Property(e => e.EmailUser)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailUser");
            entity.Property(e => e.NombreUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombreUser");

            entity.HasOne(d => d.GuidRolNavigation).WithMany(p => p.UsuariosGenerals)
                .HasForeignKey(d => d.GuidRol)
                .HasConstraintName("FK_usuariosGeneral_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
