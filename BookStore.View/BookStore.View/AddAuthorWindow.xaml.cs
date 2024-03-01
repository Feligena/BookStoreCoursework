using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddauthorWindow.xaml
    /// </summary>
    public partial class AddauthorWindow : Window
    {
        private author _currentauthor = new author() { human = new human() };
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public AddauthorWindow()
        {
            InitializeComponent();

            AddEditAuthor.Text = "Добавить автора";
            DataContext = _currentauthor;
        }

        private void SaveNewauthor_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentauthor.human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentauthor.human.last_name))
                errors.AppendLine("Укажите фамилию");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentauthor.id == 0)
            {
                if (_db.authors.Any(a => a.human.last_name == _currentauthor.human.last_name
                                    && a.human.first_name == _currentauthor.human.first_name
                                    && a.human.patronymic == _currentauthor.human.patronymic
                                    && a.is_deleted == false))
                {
                    MessageBox.Show("Такой автор уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else _db.authors.Add(_currentauthor);
            }
                
            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый автор", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
