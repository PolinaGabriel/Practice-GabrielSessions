using Avalonia.Controls;
using Avalonia.Interactivity;
using GabrielCalendar.Models;
using System.Collections.Generic;
using System.Linq;

namespace GabrielCalendar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<FirstLevelBranch> firsts = [new FirstLevelBranch() { FirstLevelBranchName = "Высшее звено" }, .. Helper.Database.FirstLevelBranches.OrderBy(x => x.FirstLevelBranchId).ToList()];
            firstBox.ItemsSource = firsts;
            firstBox.SelectedIndex = 0;
            FillEmployees();
        }

        public void FirstSelect(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => ForFirstSelect();

        public void SecondSelect(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => ForSecondSelect();

        public void ThirdSelect(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => FillEmployees();

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
            FillEmployees();
        }

        /// <summary>
        /// Заполнение списка сотрудников
        /// </summary>
        public void FillEmployees()
        {
            List<Employee> employees = new List<Employee>();
            if (firstBox.SelectedIndex != 0)
            {
                if (secondBox.SelectedIndex != 0)
                {
                    if (thirdBox.SelectedIndex != 0)
                    {
                        employees = Helper.Database.Employees.Where(x => x.EmployeeFirst == firstBox.SelectedIndex && x.EmployeeSecond == secondBox.SelectedIndex && x.EmployeeThird == thirdBox.SelectedIndex).OrderBy(x => x.EmployeeId).ToList();
                    }
                    else
                    {
                        employees = Helper.Database.Employees.Where(x => x.EmployeeFirst == firstBox.SelectedIndex && x.EmployeeSecond == secondBox.SelectedIndex).OrderBy(x => x.EmployeeId).ToList();
                    }
                }
                else
                {
                    employees = Helper.Database.Employees.Where(x => x.EmployeeFirst == firstBox.SelectedIndex).OrderBy(x => x.EmployeeId).ToList();
                }
            }
            else
            {
                employees = Helper.Database.Employees.OrderBy(x => x.EmployeeId).ToList();
            }
            employeeList.ItemsSource = employees.Select(x => new
            {
                x.EmployeeId,
                FirstBranch = x.EmployeeFirstNavigation.FirstLevelBranchName,
                SecondBranch = (x.EmployeeSecond != null) ? " - " + x.EmployeeSecondNavigation.SecondLevelBranchName : "",
                ThirdBranch = (x.EmployeeThird != null) ? " - " + x.EmployeeThirdNavigation.ThirdLevelBranchName : "",
                SecondLine = x.EmployeeName,
                ThirdLine = x.EmployeePhone + "    " + x.EmployeeEmail,
                FourthLine = x.EmployeeCabinetNavigation.CabinetName,
                Color = (x.EmployeeFired == true) ? "DarkGray" : "LightGreen"
            });
        }

        /// <summary>
        /// Редактирование сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        public void Edit(object sender, RoutedEventArgs routedEventArgs)
        {
            EmployeeWindow employeeWindow = new EmployeeWindow(Helper.Database.Employees.FirstOrDefault(x => x.EmployeeId == (int)(sender as Border).Tag));
            employeeWindow.Title = "Редактирование сотрудника";
            employeeWindow.firstBox.SelectedIndex = Helper.Database.Employees.FirstOrDefault(x => x.EmployeeId == (int)(sender as Border).Tag).EmployeeFirst;
            employeeWindow.positionBox.SelectedIndex = Helper.Database.Employees.FirstOrDefault(x => x.EmployeeId == (int)(sender as Border).Tag).EmployeePosition;
            employeeWindow.cabinetBox.SelectedIndex = Helper.Database.Employees.FirstOrDefault(x => x.EmployeeId == (int)(sender as Border).Tag).EmployeeCabinet;
            employeeWindow.editPanel.IsVisible = false;
            employeeWindow.cancelButton.IsVisible = false;
            employeeWindow.saveButton.IsVisible = false;
            employeeWindow.fireButton.IsVisible = false;
            employeeWindow.currentToggle.IsChecked = true;
            employeeWindow.ForEvents();
            employeeWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        public void Add(object sender, RoutedEventArgs routedEventArgs)
        {
            Employee employee = new Employee();
            EmployeeWindow employeeWindow = new EmployeeWindow(employee);
            employeeWindow.Title = "Новый сотрудник";
            employeeWindow.firstBox.SelectedIndex = 0;
            employeeWindow.positionBox.SelectedIndex = 0;
            employeeWindow.cabinetBox.SelectedIndex = 0;
            employeeWindow.showPanel.IsVisible = false;
            employeeWindow.timePanel.IsVisible = false;
            employeeWindow.vacationTitle.IsVisible = false;
            employeeWindow.vacationList.IsVisible = false;
            employeeWindow.tripTitle.IsVisible = false;
            employeeWindow.tripList.IsVisible = false;
            employeeWindow.illTitle.IsVisible = false;
            employeeWindow.illList.IsVisible = false;
            employeeWindow.studyTitle.IsVisible = false;
            employeeWindow.studyList.IsVisible = false;
            employeeWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Переход к окну событий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        public void GoToEvents(object sender, RoutedEventArgs routedEventArgs)
        {
            EventWindow eventWindow = new EventWindow();
            eventWindow.Show();
            this.Close();
        }
    }
}