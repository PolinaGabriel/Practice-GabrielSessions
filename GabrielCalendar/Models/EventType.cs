﻿using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class EventType
{
    public int EventTypeId { get; set; }

    public string EventTypeName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
