using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class DocumentField
{
    public int DocumentFieldId { get; set; }

    public string DocumentFieldName { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
