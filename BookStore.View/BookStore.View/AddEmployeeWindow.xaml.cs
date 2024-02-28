using bookstore.View;
using bookstore.View.MVVM.View;
using bookstore.View;
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

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private employees _currentEmployee = new employees() { human = new human(), job_titles = new job_titles()};
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public AddEmployeeWindow() //employees selectedemployees
        {
            //if(selectedemployees != null)
            //    _currentEmployee = selectedemployees;

            InitializeComponent();
            DataContext = _currentEmployee;
            JobTitleComboBox.ItemsSource = _db.job_titles.ToList();
        }

        private void ButtonSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentEmployee.human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentEmployee.human.last_name))
                errors.AppendLine("Укажите фамилию");

            if (_currentEmployee.job_titles == null)
                errors.AppendLine("Выберите должность");

            if (string.IsNullOrWhiteSpace(textBoxLogin.Text))
                errors.AppendLine("Укажите логин");

            if (string.IsNullOrWhiteSpace(textBoxPassword.Password))
                errors.AppendLine("Укажите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentEmployee.id == 0)
            {
                if (_db.authorization.Any(a => a.login == textBoxLogin.Text))
                {
                    var tmp = _db.authorization.First(a => a.login == textBoxLogin.Text && a.is_deleted == false);

                    if (tmp != null)
                    {
                        var empl = _db.employees.Any(a => a.human.last_name == _currentEmployee.human.last_name
                                        && a.human.first_name == _currentEmployee.human.first_name
                                        && a.human.patronymic == _currentEmployee.human.patronymic
                                        && tmp.id_employee == a.id
                                        && a.is_deleted == false);

                        if (empl)
                        {
                            MessageBox.Show("Такой сотрудник уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        MessageBox.Show("Такой логин уже занят", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    var authorization = new authorization() { login = textBoxLogin.Text.Trim(), password = textBoxPassword.Password.Trim() };
                    _currentEmployee.authorization.Add(authorization);
                    _db.employees.Add(_currentEmployee);
                }
            }

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый сотрудник", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
