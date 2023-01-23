using EntityFrameworkMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkMigration
{
    public class ParentServiceRepository
    {
        private readonly test_localContext _context;

        public ParentServiceRepository(test_localContext context)
        {
            _context = context;
        }

        public void UpdateParentChild(Parent parentUpdate)
        {
            var parentBd = _context.Parent
                .Include(x => x.FirstChild)
                .ThenInclude(x => x.SecondChild)
                .ThenInclude(x => x.ThirdChild)
                .Single(x => x.IdParent == parentUpdate.IdParent);

            parentBd.ParentName = parentUpdate.ParentName;
            parentBd.FirstChild = parentUpdate.FirstChild;

            _context.SaveChanges();
        }
    }
}
