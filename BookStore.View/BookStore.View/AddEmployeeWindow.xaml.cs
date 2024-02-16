using Bookstore.View;
using Bookstore.View.MVVM.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace Bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private Employees _currentEmployee = new Employees() { Human = new Human(), job_titles = new job_titles()};
        private DbBookstore _bookstore = DbBookstore.GetContext();
        public AddEmployeeWindow() //Employees selectedEmployees
        {
            //if(selectedEmployees != null)
            //    _currentEmployee = selectedEmployees;

            InitializeComponent();
            DataContext = _currentEmployee;
            JobTitleComboBox.ItemsSource = _bookstore.job_titles.ToList();
        }

        private void ButtonSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentEmployee.Human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentEmployee.Human.last_name))
                errors.AppendLine("Укажите фамилию");

            if (_currentEmployee.job_titles == null)
                errors.AppendLine("Выберите должность");

            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
                errors.AppendLine("Укажите логин");

            if (string.IsNullOrWhiteSpace(textBoxPassword.Password))
                errors.AppendLine("Укажите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentEmployee.id == 0)
            {
                var authorization = new Authorization() { login = textBoxLogin.Text.Trim(), password = textBoxPassword.Password.Trim() };
                _currentEmployee.Authorization.Add(authorization);
                _bookstore.Employees.Add(_currentEmployee);
            }
            try
            {
                _bookstore.SaveChanges();
                MessageBox.Show("Добавлен новый сотрудник");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                this.Close();
            }

        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
