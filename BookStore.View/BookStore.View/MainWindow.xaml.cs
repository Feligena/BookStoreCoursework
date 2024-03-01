using BookStore.View.MVVM.Models;
using System.Linq;
using System.Windows;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EntranceBtn_Click(object sender, RoutedEventArgs e)
        {

            var authorizationCheck = _db.authorizations.FirstOrDefault(a => a.login == textBoxLoginEntry.Text && a.is_deleted == false);

            if (authorizationCheck != null)
            {
                if(authorizationCheck.password == passwordBoxEntry.Password)
                {
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Пользователя с таким логином нет", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
