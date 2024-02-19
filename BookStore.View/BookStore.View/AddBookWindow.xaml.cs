using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private DbBookstore _db = DbBookstore.GetContext();
        private Books _currentBook = new Books();
       
        public AddBook()
        {
            InitializeComponent();
            DataContext = _currentBook;

            PublishingComboBox.ItemsSource = _db.publishing_house.Where(a => a.is_deleted == false).ToList();
            GenresComboBox.ItemsSource = _db.Genres.Where(a => a.is_deleted == false).ToList();
            //AuthorComboBox.ItemsSource = _db.Author.Where(a => a.is_deleted == false).Join(_db.Human, a => a.id_Human, h => h.id,
            //                                                (a, h) => new
            //                                                {
            //                                                    a.id,
            //                                                    nameAuthor = h.last_name + " " + h.first_name + " " + h.patronymic,
            //                                                    a.id_Human
            //                                                }).ToList();
            AuthorComboBox.ItemsSource = _db.Author.Where(a => a.is_deleted == false).ToList();
        }

        private void SaveBookBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentBook.name_book))
                errors.AppendLine("Укажите название книги");

            if (AuthorComboBox.Text == null)
                errors.AppendLine("Укажите автора");

            if (_currentBook.publishing_house == null)
                errors.AppendLine("Выберите издательство");

            if (string.IsNullOrWhiteSpace(_currentBook.year_publishing.ToString()))
                errors.AppendLine("Укажите год издания");

            if (string.IsNullOrWhiteSpace(_currentBook.number_pages.ToString()))
                errors.AppendLine("Укажите количество страниц");

            if (_currentBook.Genres == null)
                errors.AppendLine("Выберите жанр");

            if (string.IsNullOrWhiteSpace(_currentBook.cost_price.ToString()))
                errors.AppendLine("Укажите себестоимость");

            if (string.IsNullOrWhiteSpace(_currentBook.selling_price.ToString()))
                errors.AppendLine("Укажите стоимость продажи");

            if (string.IsNullOrWhiteSpace(_currentBook.amount.ToString()))
                errors.AppendLine("Укажите количество");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!");
                return;
            }

            if (_currentBook.id == 0)
                _db.Books.Add(_currentBook);

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлена новая книга");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                this.Close();
            }
        }

        private void AddGenreBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddPubHouseBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAuthorBtn_Click(object sender, RoutedEventArgs e)
        {

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
