using System;
using System.Collections.Generic;

namespace EntityFrameworkMigration.Models
{
    public partial class SecondChild
    {
        public SecondChild()
        {
            ThirdChild = new HashSet<ThirdChild>();
        }

        public int IdSecondChild { get; set; }
        public string? SecondChildName { get; set; }
        public int IdFirstChild { get; set; }

        public virtual FirstChild IdFirstChildNavigation { get; set; } = null!;
        public virtual ICollection<ThirdChild> ThirdChild { get; set; }
    }
}
