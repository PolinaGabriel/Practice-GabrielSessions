using System;
using System.Collections.Generic;
using GabrielCalendar.Models;
using Microsoft.EntityFrameworkCore;

namespace GabrielCalendar.Context;

public partial class User002Context : DbContext
{
    public User002Context()
    {
    }

    public User002Context(DbContextOptions<User002Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentField> DocumentFields { get; set; }

    public virtual DbSet<DocumentStatus> DocumentStatuses { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventStatus> EventStatuses { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<FirstLevelBranch> FirstLevelBranches { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<SecondLevelBranch> SecondLevelBranches { get; set; }

    public virtual DbSet<ThirdLevelBranch> ThirdLevelBranches { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=89.110.53.87:5522;Database=user002;Username=user002;Password=28310;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.HasKey(e => e.ApplicantId).HasName("applicant_pk");

            entity.ToTable("applicant", "calendar");

            entity.Property(e => e.ApplicantId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("applicant_id");
            entity.Property(e => e.ApplicantCv)
                .HasColumnType("character varying")
                .HasColumnName("applicant_cv");
            entity.Property(e => e.ApplicantDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("applicant_date");
            entity.Property(e => e.ApplicantEmail)
                .HasColumnType("character varying")
                .HasColumnName("applicant_email");
            entity.Property(e => e.ApplicantInterviewed).HasColumnName("applicant_interviewed");
            entity.Property(e => e.ApplicantName)
                .HasColumnType("character varying")
                .HasColumnName("applicant_name");
            entity.Property(e => e.ApplicantPhone)
                .HasColumnType("character varying")
                .HasColumnName("applicant_phone");
            entity.Property(e => e.ApplicantPosition).HasColumnName("applicant_position");

            entity.HasOne(d => d.ApplicantInterviewedNavigation).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.ApplicantInterviewed)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("applicant_employee_fk");

            entity.HasOne(d => d.ApplicantPositionNavigation).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.ApplicantPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("applicant_position_fk");
        });

        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.CabinetId).HasName("cabinet_pk");

            entity.ToTable("cabinet", "calendar");

            entity.Property(e => e.CabinetId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cabinet_id");
            entity.Property(e => e.CabinetName)
                .HasColumnType("character varying")
                .HasColumnName("cabinet_name");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("document_pk");

            entity.ToTable("document", "calendar");

            entity.Property(e => e.DocumentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("document_id");
            entity.Property(e => e.DocumentAuthor).HasColumnName("document_author");
            entity.Property(e => e.DocumentEdited)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("document_edited");
            entity.Property(e => e.DocumentEvent).HasColumnName("document_event");
            entity.Property(e => e.DocumentField).HasColumnName("document_field");
            entity.Property(e => e.DocumentMade)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("document_made");
            entity.Property(e => e.DocumentName)
                .HasColumnType("character varying")
                .HasColumnName("document_name");
            entity.Property(e => e.DocumentStatus).HasColumnName("document_status");
            entity.Property(e => e.DocumentType).HasColumnName("document_type");

            entity.HasOne(d => d.DocumentAuthorNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentAuthor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_employee_fk");

            entity.HasOne(d => d.DocumentEventNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_event_fk");

            entity.HasOne(d => d.DocumentFieldNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentField)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_document_field_fk");

            entity.HasOne(d => d.DocumentStatusNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_document_status_fk");

            entity.HasOne(d => d.DocumentTypeNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("document_document_type_fk");
        });

        modelBuilder.Entity<DocumentField>(entity =>
        {
            entity.HasKey(e => e.DocumentFieldId).HasName("document_field_pk");

            entity.ToTable("document_field", "calendar");

            entity.Property(e => e.DocumentFieldId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("document_field_id");
            entity.Property(e => e.DocumentFieldName)
                .HasColumnType("character varying")
                .HasColumnName("document_field_name");
        });

        modelBuilder.Entity<DocumentStatus>(entity =>
        {
            entity.HasKey(e => e.DocumentStatusId).HasName("document_status_pk");

            entity.ToTable("document_status", "calendar");

            entity.Property(e => e.DocumentStatusId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("document_status_id");
            entity.Property(e => e.DocumentStatusName)
                .HasColumnType("character varying")
                .HasColumnName("document_status_name");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("document_type_pk");

            entity.ToTable("document_type", "calendar");

            entity.Property(e => e.DocumentTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("document_type_id");
            entity.Property(e => e.DocumentTypeName)
                .HasColumnType("character varying")
                .HasColumnName("document_type_name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pk");

            entity.ToTable("employee", "calendar");

            entity.Property(e => e.EmployeeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("employee_id");
            entity.Property(e => e.EmployeeBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("employee_birth");
            entity.Property(e => e.EmployeeCabinet).HasColumnName("employee_cabinet");
            entity.Property(e => e.EmployeeEmail)
                .HasColumnType("character varying")
                .HasColumnName("employee_email");
            entity.Property(e => e.EmployeeFired).HasColumnName("employee_fired");
            entity.Property(e => e.EmployeeFirst).HasColumnName("employee_first");
            entity.Property(e => e.EmployeeName)
                .HasColumnType("character varying")
                .HasColumnName("employee_name");
            entity.Property(e => e.EmployeePhone)
                .HasColumnType("character varying")
                .HasColumnName("employee_phone");
            entity.Property(e => e.EmployeePosition).HasColumnName("employee_position");
            entity.Property(e => e.EmployeeSecond).HasColumnName("employee_second");
            entity.Property(e => e.EmployeeThird).HasColumnName("employee_third");

            entity.HasOne(d => d.EmployeeCabinetNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeCabinet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_cabinet_fk");

            entity.HasOne(d => d.EmployeeFirstNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeFirst)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_first_level_branch_fk");

            entity.HasOne(d => d.EmployeePositionNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeePosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_position_fk");

            entity.HasOne(d => d.EmployeeSecondNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeSecond)
                .HasConstraintName("employee_second_level_branch_fk");

            entity.HasOne(d => d.EmployeeThirdNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeeThird)
                .HasConstraintName("employee_third_level_branch_fk");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("event_pk");

            entity.ToTable("event", "calendar");

            entity.Property(e => e.EventId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("event_id");
            entity.Property(e => e.EventDescription)
                .HasColumnType("character varying")
                .HasColumnName("event_description");
            entity.Property(e => e.EventName)
                .HasColumnType("character varying")
                .HasColumnName("event_name");
            entity.Property(e => e.EventType).HasColumnName("event_type");

            entity.HasOne(d => d.EventTypeNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("event_event_type_fk");
        });

        modelBuilder.Entity<EventStatus>(entity =>
        {
            entity.HasKey(e => e.EventStatusId).HasName("event_status_pk");

            entity.ToTable("event_status", "calendar");

            entity.Property(e => e.EventStatusId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("event_status_id");
            entity.Property(e => e.EventStatusName)
                .HasColumnType("character varying")
                .HasColumnName("event_status_name");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("event_type_pk");

            entity.ToTable("event_type", "calendar");

            entity.Property(e => e.EventTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("event_type_id");
            entity.Property(e => e.EventTypeName)
                .HasColumnType("character varying")
                .HasColumnName("event_type_name");
        });

        modelBuilder.Entity<FirstLevelBranch>(entity =>
        {
            entity.HasKey(e => e.FirstLevelBranchId).HasName("department_pk");

            entity.ToTable("first_level_branch", "calendar");

            entity.Property(e => e.FirstLevelBranchId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("first_level_branch_id");
            entity.Property(e => e.FirstLevelBranchDescription)
                .HasColumnType("character varying")
                .HasColumnName("first_level_branch_description");
            entity.Property(e => e.FirstLevelBranchName)
                .HasColumnType("character varying")
                .HasColumnName("first_level_branch_name");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("position_pk");

            entity.ToTable("position", "calendar");

            entity.Property(e => e.PositionId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("position_id");
            entity.Property(e => e.PositionName)
                .HasColumnType("character varying")
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<SecondLevelBranch>(entity =>
        {
            entity.HasKey(e => e.SecondLevelBranchId).HasName("second_level_branch_pk");

            entity.ToTable("second_level_branch", "calendar");

            entity.Property(e => e.SecondLevelBranchId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("second_level_branch_id");
            entity.Property(e => e.SecondLevelBranchDescription)
                .HasColumnType("character varying")
                .HasColumnName("second_level_branch_description");
            entity.Property(e => e.SecondLevelBranchName)
                .HasColumnType("character varying")
                .HasColumnName("second_level_branch_name");
            entity.Property(e => e.SecondLevelBranchParent).HasColumnName("second_level_branch_parent");

            entity.HasOne(d => d.SecondLevelBranchParentNavigation).WithMany(p => p.SecondLevelBranches)
                .HasForeignKey(d => d.SecondLevelBranchParent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("second_level_branch_first_level_branch_fk");
        });

        modelBuilder.Entity<ThirdLevelBranch>(entity =>
        {
            entity.HasKey(e => e.ThirdLevelBranchId).HasName("third_level_branch_pk");

            entity.ToTable("third_level_branch", "calendar");

            entity.Property(e => e.ThirdLevelBranchId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("third_level_branch_id");
            entity.Property(e => e.ThirdLevelBranchDescription)
                .HasColumnType("character varying")
                .HasColumnName("third_level_branch_description");
            entity.Property(e => e.ThirdLevelBranchName)
                .HasColumnType("character varying")
                .HasColumnName("third_level_branch_name");
            entity.Property(e => e.ThirdLevelBranchParent).HasColumnName("third_level_branch_parent");

            entity.HasOne(d => d.ThirdLevelBranchParentNavigation).WithMany(p => p.ThirdLevelBranches)
                .HasForeignKey(d => d.ThirdLevelBranchParent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("third_level_branch_second_level_branch_fk");
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(e => e.TimetableId).HasName("timetable_pk");

            entity.ToTable("timetable", "calendar");

            entity.Property(e => e.TimetableId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("timetable_id");
            entity.Property(e => e.TimetableCharge).HasColumnName("timetable_charge");
            entity.Property(e => e.TimetableEmployee).HasColumnName("timetable_employee");
            entity.Property(e => e.TimetableEvent).HasColumnName("timetable_event");
            entity.Property(e => e.TimetableShiftman).HasColumnName("timetable_shiftman");

            entity.HasOne(d => d.TimetableEmployeeNavigation).WithMany(p => p.TimetableTimetableEmployeeNavigations)
                .HasForeignKey(d => d.TimetableEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timetable_employee_fk");

            entity.HasOne(d => d.TimetableEventNavigation).WithMany(p => p.Timetables)
                .HasForeignKey(d => d.TimetableEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timetable_event_fk");

            entity.HasOne(d => d.TimetableShiftmanNavigation).WithMany(p => p.TimetableTimetableShiftmanNavigations)
                .HasForeignKey(d => d.TimetableShiftman)
                .HasConstraintName("timetable_employee_fk2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
