using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Timetable
{
    public int TimetableId { get; set; }

    public int TimetableEmployee { get; set; }

    public int TimetableEvent { get; set; }

    public bool TimetableCharge { get; set; }

    public int? TimetableShiftman { get; set; }

    public int TimetableStatus { get; set; }

    public DateTime TimetableStart { get; set; }

    public DateTime TimetableEnd { get; set; }

    public virtual Employee TimetableEmployeeNavigation { get; set; } = null!;

    public virtual Event TimetableEventNavigation { get; set; } = null!;

    public virtual Employee? TimetableShiftmanNavigation { get; set; }

    public virtual TimetableStatus TimetableStatusNavigation { get; set; } = null!;
}
