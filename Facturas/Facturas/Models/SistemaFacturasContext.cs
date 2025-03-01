using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Facturas.Models;

public partial class SistemaFacturasContext : DbContext
{
    public SistemaFacturasContext()
    {
    }

    public SistemaFacturasContext(DbContextOptions<SistemaFacturasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-4KMBN5C\\SQLEXPRESS;Database=SistemaFacturas;Integrated Security=True;TrustServerCertificate=True");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__E005FBFFCDDDB572");

            entity.HasIndex(e => e.Correo, "UQ__Clientes__60695A19144A9D5F").IsUnique();

            entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PK__Facturas__E9D586A8CF1239E5");

            entity.HasIndex(e => e.NumeroFacturaElectronica, "UQ__Facturas__B92878272EB0E538").IsUnique();

            entity.Property(e => e.IdFactura).HasColumnName("ID_Factura");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NumeroFacturaElectronica).HasMaxLength(50);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Facturas__Client__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
