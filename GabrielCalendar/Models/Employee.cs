using System;
using System.Collections.Generic;

namespace GabrielCalendar.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int EmployeePosition { get; set; }

    public int EmployeeFirst { get; set; }

    public int? EmployeeSecond { get; set; }

    public int? EmployeeThird { get; set; }

    public DateTime EmployeeBirth { get; set; }

    public string EmployeePhone { get; set; } = null!;

    public string EmployeeEmail { get; set; } = null!;

    public int EmployeeCabinet { get; set; }

    public bool EmployeeFired { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Cabinet EmployeeCabinetNavigation { get; set; } = null!;

    public virtual FirstLevelBranch EmployeeFirstNavigation { get; set; } = null!;

    public virtual Position EmployeePositionNavigation { get; set; } = null!;

    public virtual SecondLevelBranch? EmployeeSecondNavigation { get; set; }

    public virtual ThirdLevelBranch? EmployeeThirdNavigation { get; set; }

    public virtual ICollection<Timetable> TimetableTimetableEmployeeNavigations { get; set; } = new List<Timetable>();

    public virtual ICollection<Timetable> TimetableTimetableShiftmanNavigations { get; set; } = new List<Timetable>();
}
