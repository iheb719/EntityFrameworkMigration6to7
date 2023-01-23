using System;
using System.Collections.Generic;

namespace EntityFrameworkMigration.Models
{
    public partial class ThirdChild
    {
        public int IdThirdChild { get; set; }
        public string? ThirdChildName { get; set; }
        public int IdSecondChild { get; set; }

        public virtual SecondChild IdSecondChildNavigation { get; set; } = null!;
    }
}
