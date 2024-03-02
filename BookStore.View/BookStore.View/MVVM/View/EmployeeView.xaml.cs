using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
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
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addEmployeeWindow = new AddEmployeeWindow(null);
            addEmployeeWindow.ShowDialog();
            UpdateEmployeeDG();
        }

        private void EditEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addEmployeeWindow = new AddEmployeeWindow((sender as Button).DataContext as employee);
            addEmployeeWindow.ShowDialog();
            UpdateEmployeeDG();
        }

        private void DeleteEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmpEmployee = (sender as Button).DataContext as employee;
                tmpEmployee.is_deleted = true;
                tmpEmployee.human.is_deleted = true;

                var tmpAuthorization = _db.authorizations.First(a => a.id_employee == tmpEmployee.id);
                tmpAuthorization.is_deleted = true;

                try
                {
                    _db.SaveChanges();
                    MessageBox.Show("Сотрудник удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateEmployeeDG();
            }
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
