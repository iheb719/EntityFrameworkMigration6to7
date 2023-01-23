using EntityFrameworkMigration;
using EntityFrameworkMigration.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EntityFrameworkMigrationTest
{
    public class ParentRepositoryTest
    {
        private readonly test_localContext _context;
        private readonly ParentServiceRepository _parentRepository;

        public ParentRepositoryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<test_localContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=test_local;User Id=postgres;Password='root';");
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));

            _context = new test_localContext(optionsBuilder.Options);

            _context.Database.ExecuteSqlRaw("DELETE FROM third_child");
            _context.Database.ExecuteSqlRaw("DELETE FROM second_child");
            _context.Database.ExecuteSqlRaw("DELETE FROM first_child");
            _context.Database.ExecuteSqlRaw("DELETE FROM parent");

            _parentRepository = new ParentServiceRepository(_context);
        }

        [Fact]
        public void UpdateParentChild_ifFirstChildAdded_thenNewFirstChildInsertedInTheDb()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent(includeFirstChild1: true, includeSecondChild1:true, includeThirdChild1:true,  includeFirstChild2: true);
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(2);
        }

        [Fact]
        public void UpdateParentChild_ifFirstChildRemoved_thenFirstChildDeletedFromTheDB()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(0);
        }

        [Fact]
        public void UpdateParentChild_ifSecondChildAdded_thenSecondChildInsertedInTheDB()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent(includeFirstChild1: true, includeSecondChild1: true, includeThirdChild1: true, includeFirstChild2: true, includeSecondChild2: true);
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(2);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(2);
        }

        [Fact]
        public void UpdateParentChild_ifSecondChildRemoved_thenSecondChildDeletedFromTheDB()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent(includeFirstChild1: true);
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(0);
        }

        [Fact]
        public void UpdateParentChild_ifThirdChildAdded_thenThirdChildInsertedInTheDB()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent(includeFirstChild1: true, includeSecondChild1: true, includeThirdChild1: true, includeFirstChild2: true, includeSecondChild2: true, includeThirdChild2: true);
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(1);
            _context.ThirdChild.Count(x => x.IdSecondChild == 111).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(2);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(2);
            _context.ThirdChild.Count(x => x.IdSecondChild == 111).Should().Be(2);
        }

        [Fact]
        public void UpdateParentChild_ifThirdChildRemoved_thenThirdChildDeletedFromTheDB()
        {
            // Arrange
            CreateInitialParentInTheDB();
            Parent newParent = CreateNewParent(includeFirstChild1: true, includeSecondChild1: true);
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(1);
            _context.ThirdChild.Count(x => x.IdSecondChild == 111).Should().Be(1);

            // Act
            _parentRepository.UpdateParentChild(newParent);

            // Assert
            _context.ChangeTracker.Clear();
            _context.FirstChild.Count(x => x.IdParent == 1).Should().Be(1);
            _context.SecondChild.Count(x => x.IdFirstChild == 11).Should().Be(1);
            _context.ThirdChild.Count(x => x.IdSecondChild == 111).Should().Be(0);
        }

        private void CreateInitialParentInTheDB()
        {
            var parent = new Parent
            {
                IdParent = 1,
                ParentName = "parent"
            };
            _context.Add(parent);

            var firstChild = new FirstChild
            {
                IdFirstChild = 11,
                FirstChildName = "firstChild1",
                IdParent = 1
            };
            _context.Add(firstChild);

            var secondChild = new SecondChild
            {
                IdSecondChild = 111,
                SecondChildName = "secondChild1",
                IdFirstChild = 11
            };
            _context.Add(secondChild);

            var thirdChild = new ThirdChild
            {
                IdThirdChild = 1111,
                ThirdChildName = "thirdChild1",
                IdSecondChild = 111
            };
            _context.Add(thirdChild);

            _context.SaveChanges();
        }

        private Parent CreateNewParent(bool includeFirstChild1 = false, bool includeFirstChild2 = false, 
            bool includeSecondChild1 = false, bool includeSecondChild2 = false, 
            bool includeThirdChild1 = false, bool includeThirdChild2 = false)
        {
            var parent = new Parent
            {
                IdParent = 1,
                ParentName = "parent"
            };

            if (includeFirstChild1)
            {
                parent.FirstChild.Add(AddFirstChild1(includeSecondChild1, includeSecondChild2, includeThirdChild1, includeThirdChild2));
            }

            if (includeFirstChild2)
            {
                var firstChild = new FirstChild
                {
                    IdFirstChild = 12,
                    FirstChildName = "firstChild2",
                    IdParent = 1
                };

                parent.FirstChild.Add(firstChild);
            }
            
            return parent;
        }

        private FirstChild AddFirstChild1(bool includeSecondChild1, bool includeSecondChild2,
            bool includeThirdChild1, bool includeThirdChild2)
        {
            var firstChild = new FirstChild
            {
                IdFirstChild = 11,
                FirstChildName = "firstChild1",
                IdParent = 1
            };

            if (includeSecondChild1)
            {
                firstChild.SecondChild.Add(AddSecondChild1(includeThirdChild1, includeThirdChild2));
            }

            if (includeSecondChild2)
            {
                var secondChild = new SecondChild
                {
                    IdSecondChild = 112,
                    SecondChildName = "secondChild2",
                    IdFirstChild = 11
                };

                firstChild.SecondChild.Add(secondChild);
            }

            return firstChild;
        }

        private static SecondChild AddSecondChild1(bool includeThirdChild1, bool includeThirdChild2)
        {
            var secondChild = new SecondChild
            {
                IdSecondChild = 111,
                SecondChildName = "secondChild1",
                IdFirstChild = 11
            };

            if (includeThirdChild1)
            {
                var thirdChild = new ThirdChild
                {
                    IdThirdChild = 1111,
                    ThirdChildName = "thirdChild1",
                    IdSecondChild = 111
                };

                secondChild.ThirdChild.Add(thirdChild);
            }

            if (includeThirdChild2)
            {
                var thirdChild = new ThirdChild
                {
                    IdThirdChild = 1112,
                    ThirdChildName = "thirdChild2",
                    IdSecondChild = 111
                };

                secondChild.ThirdChild.Add(thirdChild);
            }

            return secondChild;
        }
    }
}