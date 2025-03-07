using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class ThirdLevelBranch
{
    public int ThirdLevelBranchId { get; set; }

    public string ThirdLevelBranchName { get; set; } = null!;

    public int ThirdLevelBranchParent { get; set; }

    public string? ThirdLevelBranchDescription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual SecondLevelBranch ThirdLevelBranchParentNavigation { get; set; } = null!;
}
