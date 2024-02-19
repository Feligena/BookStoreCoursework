using BookStore.View;
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

            if (_currentBook.year_publishing == 0)
                errors.AppendLine("Укажите год издания");

            if (_currentBook.number_pages == 0)
                errors.AppendLine("Укажите количество страниц");

            if (_currentBook.Genres == null)
                errors.AppendLine("Выберите жанр");

            if (_currentBook.cost_price == 0)
                errors.AppendLine("Укажите себестоимость");

            if (_currentBook.selling_price == 0)
                errors.AppendLine("Укажите стоимость продажи");

            if (_currentBook.amount == 0)
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
            var addGenre = new AddGenresWindow();
            addGenre.ShowDialog();
        }

        private void AddPubHouseBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPubHouse = new AddPublishingHouseWindow();
            addPubHouse.ShowDialog();
        }

        private void AddAuthorBtn_Click(object sender, RoutedEventArgs e)
        {
            var addAuthor = new AddAuthorWindow();
            addAuthor.ShowDialog();
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
