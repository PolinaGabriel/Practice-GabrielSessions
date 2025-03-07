using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GabrielCalendar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace GabrielCalendar;

public partial class EmployeeWindow : Window
{
    public Employee _employee = new Employee(); //выбранный сотрудник
    
    public EmployeeWindow(Employee employee)
    {
        InitializeComponent();
        _employee = employee;
        showPanel.DataContext = _employee;
        editPanel.DataContext = _employee;
        leftErrorBlock.IsVisible = false;
        List<FirstLevelBranch> firsts = [new FirstLevelBranch() { FirstLevelBranchName = "Высшее звено" }, .. Helper.Database.FirstLevelBranches.OrderBy(x => x.FirstLevelBranchId).ToList()];
        firstBox.ItemsSource = firsts;
        List<Position> positions = [new Position() { PositionName = "Должность" }, .. Helper.Database.Positions.OrderBy(x => x.PositionId).ToList()];
        positionBox.ItemsSource = positions;
        List<Cabinet> cabinets = [new Cabinet() { CabinetName = "Кабинет" }, .. Helper.Database.Cabinets.OrderBy(x => x.CabinetId).ToList()];
        cabinetBox.ItemsSource = cabinets;
    }

    public void FirstSelect(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => ForFirstSelect();

    public void SecondSelect(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => ForSecondSelect();

    /// <summary>
    /// Заполнение списков при выборе высшего звена
    /// </summary>
    public void ForFirstSelect()
    {
        List<SecondLevelBranch> seconds = new List<SecondLevelBranch>();
        switch (firstBox.SelectedIndex)
        {
            case 0:
                secondBox.IsEnabled = false;
                seconds.Add(new SecondLevelBranch() { SecondLevelBranchName = "Среднее звено" });
                break;

            default:
                secondBox.IsEnabled = true;
                seconds = [new SecondLevelBranch() { SecondLevelBranchName = "Среднее звено" }, .. Helper.Database.SecondLevelBranches.Where(x => x.SecondLevelBranchParent == firstBox.SelectedIndex).OrderBy(x => x.SecondLevelBranchId).ToList()];
                break;
        }
        if (seconds.Count() == 1)
        {
            secondBox.IsEnabled = false;
        }
        secondBox.ItemsSource = seconds;
        secondBox.SelectedIndex = 0;
        ForSecondSelect();
    }

    /// <summary>
    /// Заполнение списков при выборе среднего звена
    /// </summary>
    public void ForSecondSelect()
    {
        List<ThirdLevelBranch> thirds = new List<ThirdLevelBranch>();
        switch (secondBox.SelectedIndex)
        {
            case 0:
                thirdBox.IsEnabled = false;
                thirds.Add(new ThirdLevelBranch() { ThirdLevelBranchName = "Низшее звено" });
                break;

            default:
                thirdBox.IsEnabled = true;
                thirds = [new ThirdLevelBranch() { ThirdLevelBranchName = "Низшее звено" }, .. Helper.Database.ThirdLevelBranches.Where(x => x.ThirdLevelBranchParent == secondBox.SelectedIndex).OrderBy(x => x.ThirdLevelBranchId).ToList()];
                break;
        }
        if (thirds.Count() == 1)
        {
            thirdBox.IsEnabled = false;
        }
        thirdBox.ItemsSource = thirds;
        thirdBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Возвращение к окну списка сотрудников
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Close(object sender, RoutedEventArgs routedEventArgs)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    /// <summary>
    /// Редактирование сотрудника
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Edit(object sender, RoutedEventArgs routedEventArgs)
    {
        showPanel.IsVisible = false;
        editPanel.IsVisible = true;
        cancelButton.IsVisible = true;
        saveButton.IsVisible = true;
        fireButton.IsVisible = true;
        editButton.IsEnabled = false;
    }

    /// <summary>
    /// Закрытие панели редактирования и открытие панели просмотра
    /// </summary>
    public void ShowShowPanel()
    {
        showPanel.IsVisible = true;
        editPanel.IsVisible = false;
        cancelButton.IsVisible = false;
        saveButton.IsVisible = false;
        fireButton.IsVisible = false;
        editButton.IsEnabled = true;
    }

    /// <summary>
    /// Отмена изменений
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Cancel(object sender, RoutedEventArgs routedEventArgs) => ShowShowPanel();

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Save(object sender, RoutedEventArgs routedEventArgs)
    {
        if (string.IsNullOrEmpty(nameBox.Text) == true || birthBox.SelectedDate == null || string.IsNullOrEmpty(phoneBox.Text) == true || string.IsNullOrEmpty(emailBox.Text) == true || firstBox.SelectedIndex == 0 || positionBox.SelectedIndex == 0 || cabinetBox.SelectedIndex == 0)
        {
            leftErrorBlock.IsVisible = true;
        }
        else
        {
            leftErrorBlock.IsVisible = false;
            _employee.EmployeePosition = positionBox.SelectedIndex;
            _employee.EmployeeCabinet = cabinetBox.SelectedIndex;
            _employee.EmployeeFirst = firstBox.SelectedIndex;
            _employee.EmployeeSecond = (secondBox.SelectedIndex == 0) ? null : secondBox.SelectedIndex;
            _employee.EmployeeThird = (thirdBox.SelectedIndex == 0) ? null : thirdBox.SelectedIndex;
            if (_employee.EmployeeId == 0)
            {
                Helper.Database.Employees.Add(_employee);
            }
            else
            {
                Helper.Database.Employees.Update(_employee);
            }
            ShowShowPanel();
        }
    }

    /// <summary>
    /// Увольнение сотрудника
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void Fire(object sender, RoutedEventArgs routedEventArgs)
    {
        DeleteWindow deleteWindow = new DeleteWindow(_employee);
        deleteWindow.Show();
        this.Close();
    }

    public void Events(object sender, RoutedEventArgs routedEventArgs) => ForEvents();

    /// <summary>
    /// Вывод расписания по времени
    /// </summary>
    public void ForEvents()
    {
        if (previousToggle.IsChecked == true || currentToggle.IsChecked == true || futureToggle.IsChecked == true)
        {
            List<Timetable> vacations = new List<Timetable>();
            List<Timetable> trips = new List<Timetable>();
            List<Timetable> ills = new List<Timetable>();
            List<Timetable> studies = new List<Timetable>();
            if (previousToggle.IsChecked == true)
            {
                foreach (Timetable timetable in Helper.Database.Timetables.Include(x => x.TimetableEventNavigation))
                {
                    if (timetable.TimetableEmployee == _employee.EmployeeId && timetable.TimetableEnd < DateTime.Now)
                    {
                        DiffTimetable(timetable, vacations, trips, ills, studies);
                    }
                }
            }
            if (currentToggle.IsChecked == true)
            {
                foreach (Timetable timetable in Helper.Database.Timetables.Include(x => x.TimetableEventNavigation))
                {
                    if (timetable.TimetableEmployee == _employee.EmployeeId && timetable.TimetableStart <= DateTime.Now && timetable.TimetableEnd >= DateTime.Now)
                    {
                        DiffTimetable(timetable, vacations, trips, ills, studies);
                    }
                }
            }
            if (futureToggle.IsChecked == true)
            {
                foreach (Timetable timetable in Helper.Database.Timetables.Include(x => x.TimetableEventNavigation))
                {
                    if (timetable.TimetableEmployee == _employee.EmployeeId && timetable.TimetableStart >= DateTime.Now)
                    {
                        DiffTimetable(timetable, vacations, trips, ills, studies);
                    }
                }
            }
            vacationList.ItemsSource = vacations.OrderBy(x => x.TimetableId);
            tripList.ItemsSource = trips.OrderBy(x => x.TimetableId);
            illList.ItemsSource = ills.OrderBy(x => x.TimetableId);
            studyList.ItemsSource = studies.OrderBy(x => x.TimetableId);
        }
        else
        {
            vacationList.ItemsSource = null;
            tripList.ItemsSource = null;
            illList.ItemsSource = null;
            studyList.ItemsSource = null;
        }
    }

    /// <summary>
    /// Заполнение календарей
    /// </summary>
    /// <param name="timetable"></param>
    /// <param name="vacations"></param>
    /// <param name="trips"></param>
    /// <param name="ills"></param>
    /// <param name="studies"></param>
    public void DiffTimetable(Timetable timetable, List<Timetable> vacations, List<Timetable> trips, List<Timetable> ills, List<Timetable> studies)
    {
        switch (timetable.TimetableEventNavigation.EventType)
        {
            case 1:
                vacations.Add(timetable);
                break;
            case 2:
                trips.Add(timetable);
                break;
            case 3:
                ills.Add(timetable);
                break;
            case 4:
                studies.Add(timetable);
                break;
        }
    }
}