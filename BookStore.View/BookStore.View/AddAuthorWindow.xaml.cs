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
using System.Windows.Shapes;

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        private Author _currentAuthor = new Author() { Human = new Human() };
        private DbBookstore _db = DbBookstore.GetContext();
        public AddAuthorWindow()
        {
            InitializeComponent();
            DataContext = _currentAuthor;
        }

        private void SaveNewAuthor_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentAuthor.Human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentAuthor.Human.last_name))
                errors.AppendLine("Укажите фамилию");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_db.Author.Any(a => a.Human.last_name == _currentAuthor.Human.last_name 
                                    && a.Human.first_name == _currentAuthor.Human.first_name 
                                    && a.Human.patronymic == _currentAuthor.Human.patronymic 
                                    && a.is_deleted == false))
            {
                MessageBox.Show("Такой автор уже существует");
                return;
            }

            if (_currentAuthor.id == 0)
                _db.Author.Add(_currentAuthor);
                
            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый автор");
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
