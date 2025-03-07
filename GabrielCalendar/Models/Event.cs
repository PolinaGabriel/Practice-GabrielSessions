using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public int EventType { get; set; }

    public string? EventDescription { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual EventType EventTypeNavigation { get; set; } = null!;

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
