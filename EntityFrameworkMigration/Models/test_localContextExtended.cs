using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkMigration.Models
{
    public partial class test_localContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FirstChild>(entity =>
            {
                entity.HasOne(d => d.IdParentNavigation)
                    .WithMany(p => p.FirstChild)
                    .HasForeignKey(d => d.IdParent)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_firstchild_parent");
            });

            modelBuilder.Entity<SecondChild>(entity =>
            {
                entity.HasOne(d => d.IdFirstChildNavigation)
                    .WithMany(p => p.SecondChild)
                    .HasForeignKey(d => d.IdFirstChild)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_secondchild_firstchild");
            });

            modelBuilder.Entity<ThirdChild>(entity =>
            {
                entity.HasOne(d => d.IdSecondChildNavigation)
                    .WithMany(p => p.ThirdChild)
                    .HasForeignKey(d => d.IdSecondChild)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_thirdchild_secondchild");
            });
        }
    }
}
