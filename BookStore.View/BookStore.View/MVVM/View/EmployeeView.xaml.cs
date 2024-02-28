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
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public EmployeeView()
        {
            InitializeComponent();
            UpdateEmployeeDG();
        }

        private void UpdateEmployeeDG()
        {
            EmployeeDataGrid.ItemsSource = _db.employees.Where(em => em.is_deleted == false).ToList();
        }

        private void ButtonAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow(null);
            addEmployeeWindow.ShowDialog();
            UpdateEmployeeDG();
        }

        private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow((sender as Button).DataContext as employees);
            addEmployeeWindow.ShowDialog();
            UpdateEmployeeDG();
        }

        private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmpEmployee = (sender as Button).DataContext as employees;
                tmpEmployee.is_deleted = true;
                tmpEmployee.human.is_deleted = true;

                var tmpAuthorization = _db.authorization.First(a => a.id_employee == tmpEmployee.id);
                tmpAuthorization.is_deleted = true;

                try
                {
                    _db.SaveChanges();
                    MessageBox.Show("Сотрудник удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            UpdateEmployeeDG();
        }

        private void SearchEmployeeBtn_CLick(object sender, RoutedEventArgs e)
        {
            EmployeeDataGrid.ItemsSource = _db.employees.Where(em => em.is_deleted == false 
                                                                     && (em.human.first_name.Contains(SearchEmployeeText.Text)
                                                                     || em.human.last_name.Contains(SearchEmployeeText.Text)
                                                                     || em.human.patronymic.Contains(SearchEmployeeText.Text))).ToList();
        }
    }
}
