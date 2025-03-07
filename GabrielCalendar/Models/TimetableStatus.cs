using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class TimetableStatus
{
    public int TimetableStatusId { get; set; }

    public string TimetableStatusName { get; set; } = null!;

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
