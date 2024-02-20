using bookstore.View;
using bookstore.View;
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

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        private DbbookstoreEntities _db = DbbookstoreEntities.GetContext();
        public EmployeeView()
        {
            InitializeComponent();
            //EmployeeDataGrid.ItemsSource = Dbbookstore.GetContext().employees.Where(em => em.is_deleted == false).ToList();
            //EmployeeDataGrid.ItemsSource = _db.employees.ToList();
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
                //EmployeeDataGrid.ItemsSource = _db.employees.Where(em => em.is_deleted == false).ToList();
                //EmployeeDataGrid.ItemsSource = _db.authorization.Where(em => em.is_deleted == false).ToList();
                EmployeeDataGrid.ItemsSource = _db.employees.Where(em => em.is_deleted == false)
                                                            .Join(_db.authorization, p => p.id, a => a.id_employee,
                                                                 (p, a) => new
                                                                 {
                                                                     a.id,
                                                                     fname = p.human.first_name,
                                                                     lname = p.human.last_name,
                                                                     p.human.patronymic,
                                                                     title = p.job_titles.name_title,
                                                                     a.login
                                                                 })
                                                            .ToList();
            }
        }

        private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            //var addEmployeeWindow = new AddEmployeeWindow((sender as Button).DataContext as employees);
            //addEmployeeWindow.ShowDialog();
        }

        private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchEmployeeBtn_CLick(object sender, RoutedEventArgs e)
        {
            //SearchEmployeeText
            EmployeeDataGrid.ItemsSource = _db.employees.Where(em => em.is_deleted == false 
                                                                     && (em.human.first_name.Contains(SearchEmployeeText.Text)
                                                                     || em.human.last_name.Contains(SearchEmployeeText.Text)
                                                                     || em.human.patronymic.Contains(SearchEmployeeText.Text)))
                                                        .Join(_db.authorization, p => p.id, a => a.id_employee,
                                                             (p, a) => new
                                                             {
                                                                 a.id,
                                                                 fname = p.human.first_name,
                                                                 lname = p.human.last_name,
                                                                 p.human.patronymic,
                                                                 title = p.job_titles.name_title,
                                                                 a.login
                                                             })
                                                        .ToList();
        }
    }
}
