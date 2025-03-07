using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Applicant
{
    public int ApplicantId { get; set; }

    public string ApplicantName { get; set; } = null!;

    public string ApplicantPhone { get; set; } = null!;

    public string ApplicantEmail { get; set; } = null!;

    public string ApplicantCv { get; set; } = null!;

    public int ApplicantInterviewed { get; set; }

    public DateTime ApplicantDate { get; set; }

    public int ApplicantPosition { get; set; }

    public virtual Employee ApplicantInterviewedNavigation { get; set; } = null!;

    public virtual Position ApplicantPositionNavigation { get; set; } = null!;
}
