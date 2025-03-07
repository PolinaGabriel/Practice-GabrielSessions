using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class SecondLevelBranch
{
    public int SecondLevelBranchId { get; set; }

    public string SecondLevelBranchName { get; set; } = null!;

    public int SecondLevelBranchParent { get; set; }

    public string? SecondLevelBranchDescription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual FirstLevelBranch SecondLevelBranchParentNavigation { get; set; } = null!;

    public virtual ICollection<ThirdLevelBranch> ThirdLevelBranches { get; set; } = new List<ThirdLevelBranch>();
}
