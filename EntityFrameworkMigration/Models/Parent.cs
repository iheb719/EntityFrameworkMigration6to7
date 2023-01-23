using System;
using System.Collections.Generic;

namespace EntityFrameworkMigration.Models
{
    public partial class Parent
    {
        public Parent()
        {
            FirstChild = new HashSet<FirstChild>();
        }

        public int IdParent { get; set; }
        public string? ParentName { get; set; }

        public virtual ICollection<FirstChild> FirstChild { get; set; }
    }
}
