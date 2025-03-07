using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Cabinet
{
    public int CabinetId { get; set; }

    public string CabinetName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
