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
using System.Windows.Shapes;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private users _currentUser = new users() { human = new human()};
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public AddUserWindow(users sendlerUser)
        {
            InitializeComponent();

            if(sendlerUser != null)
                _currentUser = sendlerUser;

            DataContext = _currentUser;
        }

        private void SaveNewUserBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentUser.human.last_name))
                errors.AppendLine("Укажите фамилию");

            if (string.IsNullOrWhiteSpace(_currentUser.phone))
                errors.AppendLine("Укажите номер телефона");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentUser.id == 0)
            {
                if (_db.users.Any(a => a.human.last_name == _currentUser.human.last_name
                                    && a.human.first_name == _currentUser.human.first_name
                                    && a.human.patronymic == _currentUser.human.patronymic
                                    && a.phone == _currentUser.phone
                                    && a.is_deleted == false))
                {
                    MessageBox.Show("Такой покупатель уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else _db.users.Add(_currentUser);
            }

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Информация о покупателе сохранена", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                this.Close();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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
        
    }
}
