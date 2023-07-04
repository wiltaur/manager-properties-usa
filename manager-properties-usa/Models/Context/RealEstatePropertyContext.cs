using System;
using System.Collections.Generic;
using manager_properties_usa.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace manager_properties_usa.Models.Context;

public partial class RealEstatePropertyContext : DbContext
{
    public RealEstatePropertyContext()
    {
    }

    public RealEstatePropertyContext(DbContextOptions<RealEstatePropertyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyImage> PropertyImages { get; set; }

    public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.IdOwner);

            entity.ToTable("Owner");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.IdProperty);

            entity.ToTable("Property");

            entity.HasIndex(e => e.CodeInternal, "IX_Code_Internal").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Year).HasColumnType("numeric(4, 0)");

            entity.HasOne(d => d.IdOwnerNavigation).WithMany(p => p.Properties)
                .HasForeignKey(d => d.IdOwner)
                .HasConstraintName("FK_Property_Owner");
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.IdPropertyImage);

            entity.ToTable("PropertyImage");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyImages)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_PropertyImage_Property");
        });

        modelBuilder.Entity<PropertyTrace>(entity =>
        {
            entity.HasKey(e => e.IdPropertyTrace);

            entity.ToTable("PropertyTrace");

            entity.Property(e => e.DateSale).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Tax).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyTraces)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_PropertyTrace_Property");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
