using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
