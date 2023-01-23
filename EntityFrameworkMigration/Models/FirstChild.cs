using System;
using System.Collections.Generic;

namespace EntityFrameworkMigration.Models
{
    public partial class FirstChild
    {
        public FirstChild()
        {
            SecondChild = new HashSet<SecondChild>();
        }

        public int IdFirstChild { get; set; }
        public string? FirstChildName { get; set; }
        public int IdParent { get; set; }

        public virtual Parent IdParentNavigation { get; set; } = null!;
        public virtual ICollection<SecondChild> SecondChild { get; set; }
    }
}
