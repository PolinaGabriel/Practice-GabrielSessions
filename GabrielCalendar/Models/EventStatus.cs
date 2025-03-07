using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class EventStatus
{
    public int EventStatusId { get; set; }

    public string EventStatusName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
