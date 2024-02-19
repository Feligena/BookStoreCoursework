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

namespace Bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private users _currentUser = new users() { Human = new Human()};
        private DbBookstore _db = DbBookstore.GetContext();
        public AddUserWindow()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }

        private void SaveNewUserBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentUser.Human.last_name))
                errors.AppendLine("Укажите фамилию");

            if (_currentUser.phone == 0)
                errors.AppendLine("Укажите номер телефона");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_db.users.Any(a => a.Human.last_name == _currentUser.Human.last_name
                                    && a.Human.first_name == _currentUser.Human.first_name
                                    && a.Human.patronymic == _currentUser.Human.patronymic
                                    && a.phone == _currentUser.phone
                                    && a.is_deleted == false))
            {
                MessageBox.Show("Такой покупатель уже существует");
                return;
            }

            if (_currentUser.id == 0)
                _db.users.Add(_currentUser);

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый покупатель");
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
