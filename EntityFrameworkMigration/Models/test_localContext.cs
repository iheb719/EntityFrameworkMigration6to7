using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkMigration.Models
{
    public partial class test_localContext : DbContext
    {
        public test_localContext()
        {
        }

        public test_localContext(DbContextOptions<test_localContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FirstChild> FirstChild { get; set; } = null!;
        public virtual DbSet<Parent> Parent { get; set; } = null!;
        public virtual DbSet<SecondChild> SecondChild { get; set; } = null!;
        public virtual DbSet<ThirdChild> ThirdChild { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FirstChild>(entity =>
            {
                entity.HasKey(e => e.IdFirstChild)
                    .HasName("first_child_pkey");

                entity.ToTable("first_child");

                entity.Property(e => e.IdFirstChild)
                    .ValueGeneratedNever()
                    .HasColumnName("id_first_child");

                entity.Property(e => e.FirstChildName)
                    .HasMaxLength(50)
                    .HasColumnName("first_child_name");

                entity.Property(e => e.IdParent).HasColumnName("id_parent");

                entity.HasOne(d => d.IdParentNavigation)
                    .WithMany(p => p.FirstChild)
                    .HasForeignKey(d => d.IdParent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_firstchild_parent");
            });

            modelBuilder.Entity<Parent>(entity =>
            {
                entity.HasKey(e => e.IdParent)
                    .HasName("Parent_pkey");

                entity.ToTable("parent");

                entity.Property(e => e.IdParent)
                    .ValueGeneratedNever()
                    .HasColumnName("id_parent");

                entity.Property(e => e.ParentName)
                    .HasMaxLength(50)
                    .HasColumnName("parent_name");
            });

            modelBuilder.Entity<SecondChild>(entity =>
            {
                entity.HasKey(e => e.IdSecondChild)
                    .HasName("second_child_pkey");

                entity.ToTable("second_child");

                entity.Property(e => e.IdSecondChild)
                    .ValueGeneratedNever()
                    .HasColumnName("id_second_child");

                entity.Property(e => e.IdFirstChild).HasColumnName("id_first_child");

                entity.Property(e => e.SecondChildName)
                    .HasMaxLength(50)
                    .HasColumnName("second_child_name");

                entity.HasOne(d => d.IdFirstChildNavigation)
                    .WithMany(p => p.SecondChild)
                    .HasForeignKey(d => d.IdFirstChild)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_secondchild_firstchild");
            });

            modelBuilder.Entity<ThirdChild>(entity =>
            {
                entity.HasKey(e => e.IdThirdChild)
                    .HasName("third_child_pkey");

                entity.ToTable("third_child");

                entity.Property(e => e.IdThirdChild)
                    .ValueGeneratedNever()
                    .HasColumnName("id_third_child");

                entity.Property(e => e.IdSecondChild).HasColumnName("id_second_child");

                entity.Property(e => e.ThirdChildName)
                    .HasMaxLength(50)
                    .HasColumnName("third_child_name");

                entity.HasOne(d => d.IdSecondChildNavigation)
                    .WithMany(p => p.ThirdChild)
                    .HasForeignKey(d => d.IdSecondChild)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_thirdchild_secondchild");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
