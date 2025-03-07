using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public string DocumentName { get; set; } = null!;

    public DateTime DocumentMade { get; set; }

    public DateTime DocumentEdited { get; set; }

    public int DocumentStatus { get; set; }

    public int DocumentType { get; set; }

    public int DocumentField { get; set; }

    public int DocumentAuthor { get; set; }

    public int DocumentEvent { get; set; }

    public virtual Employee DocumentAuthorNavigation { get; set; } = null!;

    public virtual Event DocumentEventNavigation { get; set; } = null!;

    public virtual DocumentField DocumentFieldNavigation { get; set; } = null!;

    public virtual DocumentStatus DocumentStatusNavigation { get; set; } = null!;

    public virtual DocumentType DocumentTypeNavigation { get; set; } = null!;
}
