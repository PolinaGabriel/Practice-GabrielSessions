using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GabrielCalendar.Models;
using System.Collections.Generic;
using System.Linq;

namespace GabrielCalendar;

public partial class DeleteWindow : Window
{
    public Employee _employee = new Employee(); //выбранный сотрудник

    public DeleteWindow(Employee employee)
    {
        InitializeComponent();
        _employee = employee;
        this.Title = _employee.EmployeeName;
    }

    /// <summary>
    /// Увольнение сотрудника
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Fire(object sender, RoutedEventArgs routedEventArgs)
    {
        _employee.EmployeeFired = true;
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Отмена увольнения сотрудника
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Cancel(object sender, RoutedEventArgs routedEventArgs)
    {
        EmployeeWindow employeeWindow = new EmployeeWindow(_employee);
        IEnumerable<Timetable> vacations =
            from x in Helper.Database.Timetables
            where x.TimetableEmployee == (int)(sender as Border).Tag && x.TimetableEventNavigation.EventType == 1
            orderby x.TimetableId
            select x;
        employeeWindow.vacationList.ItemsSource = vacations.ToList();
        IEnumerable<Timetable> trips =
            from x in Helper.Database.Timetables
            where x.TimetableEmployee == (int)(sender as Border).Tag && x.TimetableEventNavigation.EventType == 2
            orderby x.TimetableId
            select x;
        employeeWindow.tripList.ItemsSource = trips.ToList();
        IEnumerable<Timetable> ills =
            from x in Helper.Database.Timetables
            where x.TimetableEmployee == (int)(sender as Border).Tag && x.TimetableEventNavigation.EventType == 3
            orderby x.TimetableId
            select x;
        employeeWindow.illList.ItemsSource = ills.ToList();
        IEnumerable<Timetable> studies =
            from x in Helper.Database.Timetables
            where x.TimetableEmployee == (int)(sender as Border).Tag && x.TimetableEventNavigation.EventType == 4
            orderby x.TimetableId
            select x;
        employeeWindow.studyList.ItemsSource = studies.ToList();
        employeeWindow.Show();
        this.Close();
    }
}