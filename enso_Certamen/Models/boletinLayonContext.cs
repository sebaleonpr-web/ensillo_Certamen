using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace enso_Certamen.Models;

public partial class boletinLayonContext : DbContext
{
    public boletinLayonContext(DbContextOptions<boletinLayonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<boletinGeneral> boletinGenerals { get; set; }

    public virtual DbSet<comentarioGeneral> comentarioGenerals { get; set; }

    public virtual DbSet<noticiaGeneral> noticiaGenerals { get; set; }

    public virtual DbSet<rolGeneral> rolGenerals { get; set; }
    public object RolGenerals { get; internal set; }
    public virtual DbSet<suscripcionGeneral> suscripcionGenerals { get; set; }

    public virtual DbSet<usuariosGeneral> usuariosGenerals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<boletinGeneral>(entity =>
        {
            entity.Property(e => e.GuidBoletin).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.FechaBoletin).HasDefaultValueSql("(CONVERT([date],getdate()))");

            entity.HasOne(d => d.GuidNoticiaNavigation).WithMany(p => p.boletinGenerals).HasConstraintName("FK_boletinGeneral_Noticia");
        });

        modelBuilder.Entity<comentarioGeneral>(entity =>
        {
            entity.Property(e => e.GuidComentario).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.fechaComentario).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.GuidNoticiaNavigation).WithMany(p => p.comentarioGenerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comentarioGeneral_Noticia");
        });

        modelBuilder.Entity<noticiaGeneral>(entity =>
        {
            entity.Property(e => e.GuidNoticia).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.fechaNoticia).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.GuidUsuarioNavigation).WithMany(p => p.noticiaGenerals).HasConstraintName("FK_noticiaGeneral_Usuario");
        });

        modelBuilder.Entity<rolGeneral>(entity =>
        {
            entity.Property(e => e.GuidRol).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<suscripcionGeneral>(entity =>
        {
            entity.Property(e => e.GuidSuscripcion).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.fechaSuscripcion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.GuidBoletinNavigation).WithMany(p => p.suscripcionGenerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_suscripcionGeneral_Boletin");
        });

        modelBuilder.Entity<usuariosGeneral>(entity =>
        {
            entity.Property(e => e.GuidUsuario).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.GuidRolNavigation).WithMany(p => p.usuariosGenerals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuariosGeneral_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
