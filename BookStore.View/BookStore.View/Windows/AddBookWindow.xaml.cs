using bookstore.View;
using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        private book _currentBook = new book();
       
        public AddBook(book selectedBook)
        {
            InitializeComponent();

            if(selectedBook != null)
            {
                _currentBook = selectedBook;
                AddEditBook.Text = "Редактировать книгу";
            }
            else AddEditBook.Text = "Добавить книгу";

            DataContext = _currentBook;

            PublishingComboBox.ItemsSource = _db.publishing_house.Where(a => a.is_deleted == false).ToList();
            GenresComboBox.ItemsSource = _db.genres.Where(a => a.is_deleted == false).ToList();
            AuthorComboBox.ItemsSource = _db.authors.Where(a => a.is_deleted == false).ToList();
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

            if (_currentBook.genre == null)
                errors.AppendLine("Выберите жанр");

            if (_currentBook.cost_price == 0)
                errors.AppendLine("Укажите себестоимость");

            if (_currentBook.selling_price == 0)
                errors.AppendLine("Укажите стоимость продажи");

            if (_currentBook.amount == 0)
                errors.AppendLine("Укажите количество");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentBook.id == 0)
            {
                if (_db.books.Any(b => b.name_book == _currentBook.name_book
                                && b.author.human.last_name == _currentBook.author.human.last_name
                                && b.author.human.first_name == _currentBook.author.human.first_name
                                && b.author.human.patronymic == _currentBook.author.human.patronymic
                                && b.publishing_house.name_pub_house == _currentBook.publishing_house.name_pub_house
                                && b.year_publishing == _currentBook.year_publishing
                                && b.number_pages == _currentBook.number_pages
                                && b.genre.name_genre == _currentBook.genre.name_genre
                                && b.is_deleted == false))
                {
                    MessageBox.Show("Такая книга уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                    _db.books.Add(_currentBook);
            }

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Информация сохранена", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void AddGenreBtn_Click(object sender, RoutedEventArgs e)
        {
            var addGenre = new AddgenresWindow();
            addGenre.ShowDialog();
            GenresComboBox.ItemsSource = _db.genres.Where(a => a.is_deleted == false).ToList();
        }

        private void AddPubHouseBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPubHouse = new AddPublishingHouseWindow();
            addPubHouse.ShowDialog();
            PublishingComboBox.ItemsSource = _db.publishing_house.Where(a => a.is_deleted == false).ToList();
        }

        private void AddAuthorBtn_Click(object sender, RoutedEventArgs e)
        {
            var addauthor = new AddauthorWindow();
            addauthor.ShowDialog();
            AuthorComboBox.ItemsSource = _db.authors.Where(a => a.is_deleted == false).ToList();
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
