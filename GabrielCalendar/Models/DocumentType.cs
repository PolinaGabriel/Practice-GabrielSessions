using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class DocumentType
{
    public int DocumentTypeId { get; set; }

    public string DocumentTypeName { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
