using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class FirstLevelBranch
{
    public int FirstLevelBranchId { get; set; }

    public string FirstLevelBranchName { get; set; } = null!;

    public string? FirstLevelBranchDescription { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<SecondLevelBranch> SecondLevelBranches { get; set; } = new List<SecondLevelBranch>();
}
