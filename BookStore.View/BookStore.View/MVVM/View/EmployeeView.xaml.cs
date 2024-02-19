using Bookstore.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        private DbBookstore _db = DbBookstore.GetContext();
        public EmployeeView()
        {
            InitializeComponent();
            //EmployeeDataGrid.ItemsSource = DbBookstore.GetContext().Employees.Where(em => em.is_deleted == false).ToList();
            //EmployeeDataGrid.ItemsSource = _db.Employees.ToList();
        }

        private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                //EmployeeDataGrid.ItemsSource = _db.Employees.Where(em => em.is_deleted == false).ToList();
                //EmployeeDataGrid.ItemsSource = _db.Authorization.Where(em => em.is_deleted == false).ToList();
                EmployeeDataGrid.ItemsSource = _db.Employees.Where(em => em.is_deleted == false)
                                                            .Join(_db.Authorization, p => p.id, a => a.id_employee,
                                                                 (p, a) => new
                                                                 {
                                                                     a.id,
                                                                     fname = p.Human.first_name,
                                                                     lname = p.Human.last_name,
                                                                     p.Human.patronymic,
                                                                     title = p.job_titles.name_title,
                                                                     a.login
                                                                 })
                                                            .ToList();
            }
        }

        private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            //var addEmployeeWindow = new AddEmployeeWindow((sender as Button).DataContext as Employees);
            //addEmployeeWindow.ShowDialog();
        }

        private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
