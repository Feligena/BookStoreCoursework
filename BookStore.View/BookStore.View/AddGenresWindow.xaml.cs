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
    /// Логика взаимодействия для AddgenresWindow.xaml
    /// </summary>
    public partial class AddgenresWindow : Window
    {
        private genres _currentGenre = new genres();
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public AddgenresWindow()
        {
            InitializeComponent();
            DataContext = _currentGenre;
        }

        private void SaveNewGenre_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentGenre.name_genre))
                errors.AppendLine("Укажите название жанра");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!");
                return;
            }

            if (_currentGenre.id == 0)
            {
                if (_db.genres.Any(g => g.name_genre == _currentGenre.name_genre && g.is_deleted == false))
                {
                    MessageBox.Show("Такой жанр уже существует");
                    return;
                }
                else _db.genres.Add(_currentGenre);
            }

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый жанр");
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
