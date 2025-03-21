using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using GabrielCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

namespace GabrielCalendar;

public partial class EventWindow : Window
{
    public List<DateTime> _easyDays = new List<DateTime>();
    public List<DateTime> _midDays = new List<DateTime>();
    public List<DateTime> _hardDays = new List<DateTime>();

    public EventWindow()
    {
        InitializeComponent();
        searchBox.Text = "";
        datePicker.DisplayDateStart = new DateTime(2000, 01, 01);
        datePicker.DisplayDateEnd = new DateTime(2100, 12, 31);
        datePicker.SelectedDate = DateTime.Now;
        findDates();
        Fill();
        datePicker.Loaded += OnCalendarLoaded;
        datePicker.DisplayDateChanged += CustomCalendar_DisplayDateChanged;
    }

    public void OnCalendarLoaded(object sender, EventArgs eventArgs) => DayColor();

    public void CustomCalendar_DisplayDateChanged(object? sender, CalendarDateChangedEventArgs calendarDateChangedEventArgs) => DayColor();

    public void SelectDate(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => Fill();

    public void Search(object sender, KeyEventArgs keyEventArgs) => Fill();

    /// <summary>
    /// Подсчёт числа мероприятий в дни
    /// </summary>
    public void findDates()
    {
        for (DateTime dt = (DateTime)datePicker.DisplayDateStart; dt <= (DateTime)datePicker.DisplayDateEnd; dt += new TimeSpan(1, 0, 0, 0))
        {
            int count = 0;
            foreach (Timetable timetable in Helper.Database.Timetables.Where(x => x.TimetableEventNavigation.EventType == 4 || x.TimetableEventNavigation.EventType == 5).OrderBy(x => x.TimetableId))
            {
                if (dt >= timetable.TimetableStart.Date && dt <= timetable.TimetableEnd.Date)
                {
                    count++;
                }
            }
            switch (count)
            {
                case < 2:
                    _easyDays.Add(dt);
                    break;
                case >= 5:
                    _hardDays.Add(dt);
                    break;
                default:
                    _midDays.Add(dt);
                    break;
            }
        }
    }

    /// <summary>
    /// Покраска дня в календаре определённым цветом
    /// </summary>
    public void DayColor()
    {
        foreach (var child in datePicker.GetVisualDescendants())
        {
            if (child is CalendarDayButton dayButton)
            {
                var date = datePicker.DisplayDate;
                string day = dayButton.Content!.ToString()!;
                try
                {
                    if (_easyDays.Contains(new DateTime(date.Year, date.Month, int.Parse(day))))
                    {
                        dayButton.Background = Brush.Parse("#89FC43");
                        dayButton.Foreground = Brushes.Black;
                    }
                    if (_midDays.Contains(new DateTime(date.Year, date.Month, int.Parse(day))))
                    {
                        dayButton.Background = Brush.Parse("#F8FC43");
                        dayButton.Foreground = Brushes.Black;
                    }
                    if (_hardDays.Contains(new DateTime(date.Year, date.Month, int.Parse(day))))
                    {
                        dayButton.Background = Brush.Parse("#FC4343");
                        dayButton.Foreground = Brushes.Black;
                    }
                }
                catch
                {
                    dayButton.Background = Brushes.LightGray;
                    dayButton.Foreground = Brushes.Black;
                }
            }
        }
    }

    /// <summary>
    /// Заполнение списков
    /// </summary>
    public void Fill()
    {
        List<Timetable> events = Helper.Database.Timetables.Where(x => (x.TimetableEventNavigation.EventType == 4 || x.TimetableEventNavigation.EventType == 5) && x.TimetableStart.Date <= datePicker.SelectedDate.Value.Date && x.TimetableEnd.Date >= datePicker.SelectedDate.Value.Date).OrderBy(x => x.TimetableId).ToList();
        events = EventSearch(events);
        eventList.ItemsSource = events.Select(x => new
        {
            x.TimetableId,
            x.TimetableEventNavigation.EventName,
            x.TimetableEventNavigation.EventDescription,
            start = Convert.ToString(x.TimetableStart).Substring(0, 16),
            x.TimetableEmployeeNavigation.EmployeeName
        });
        List<Employee> employees = new List<Employee>();
        foreach (Employee employee in Helper.Database.Employees.OrderBy(x => x.EmployeeId).ToList())
        {
            foreach (Timetable eve in events)
            {
                if (employee.EmployeeId == eve.TimetableEmployee || employee.EmployeeId == eve.TimetableShiftman)
                {
                    employees.Add(employee);
                }
            }
        }
        employees = EmployeeSearch(employees);
        employeeList.ItemsSource = employees.Select(x => new
        {
            x.EmployeeId,
            x.EmployeeName,
            x.EmployeePositionNavigation.PositionName,
            x.EmployeePhone,
            x.EmployeeEmail,
            birth = Convert.ToString(x.EmployeeBirth).Substring(0, 10),
            color = (x.EmployeeBirth.Day == datePicker.SelectedDate.Value.Day && x.EmployeeBirth.Month == datePicker.SelectedDate.Value.Month) ? "Red" : "Black",
            weight = (x.EmployeeBirth.Day == datePicker.SelectedDate.Value.Day && x.EmployeeBirth.Month == datePicker.SelectedDate.Value.Month) ? "Bold" : ""
        });
        List<News> news = new List<News>();
        var baseDirectory = AppContext.BaseDirectory;
        var jsonPath = Path.Combine(baseDirectory, "News", "news.JSON");
        var json = File.ReadAllText(jsonPath);
        news = JsonConvert.DeserializeObject<List<News>>(json).Where(x => x.date == datePicker.SelectedDate.Value.Date).ToList();
        news = NewsSearch(news);
        newstList.ItemsSource = news.Select(x => new
        {
            x.id,
            x.title,
            x.description,
            x.link,
            Date = Convert.ToString(x.date).Substring(1, 10)
        });
    }

    /// <summary>
    /// Поиск по сотрудникам
    /// </summary>
    /// <param name="employees"></param>
    /// <returns></returns>
    public List<Employee> EmployeeSearch(List<Employee> employees)
    {
        List<Employee> search = new List<Employee>();
        string[] words = searchBox.Text.Split(' ');
        foreach (Employee employee in employees)
        {
            foreach (string word in words)
            {
                if (employee.EmployeeName.ToLower().Contains(word.ToLower()) == true)
                {
                    search.Add(employee);
                    break;
                }
            }
        }
        return search;
    }

    /// <summary>
    /// Поиск по событиям
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public List<Timetable> EventSearch(List<Timetable> events)
    {
        List<Timetable> search = new List<Timetable>();
        string[] words = searchBox.Text.Split(' ');
        foreach (Timetable timetable in events)
        {
            foreach (string word in words)
            {
                if (timetable.TimetableEventNavigation.EventName.ToLower().Contains(word.ToLower()) == true)
                {
                    search.Add(timetable);
                    break;
                }
            }
        }
        return search;
    }

    /// <summary>
    /// Поиск по новостям
    /// </summary>
    /// <param name="news"></param>
    /// <returns></returns>
    public List<News> NewsSearch(List<News> news)
    {
        List<News> search = new List<News>();
        string[] words = searchBox.Text.Split(' ');
        foreach (News n in news)
        {
            foreach (string word in words)
            {
                if (n.title.ToLower().Contains(word.ToLower()) == true || n.description.ToLower().Contains(word.ToLower()) == true)
                {
                    search.Add(n);
                    break;
                }
            }
        }
        return search;
    }

    /// <summary>
    /// Переход к окну организационной структуры
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="routedEventArgs"></param>
    public void GoToStructure(object sender, RoutedEventArgs routedEventArgs)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}
