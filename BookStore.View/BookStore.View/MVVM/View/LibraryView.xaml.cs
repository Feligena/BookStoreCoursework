using BookStore.View;
using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LibraryView.xaml
    /// </summary>
    public partial class LibraryView : UserControl
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public LibraryView()
        {
            InitializeComponent();

            UbdateDGLibrary();
        }

        private void UbdateDGLibrary()
        {
            LibraryDataGrid.ItemsSource = _db.books.Where(b => b.is_deleted == false).ToList();
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addBook = new AddBook((sender as Button).DataContext as book);
            addBook.ShowDialog();
            UbdateDGLibrary();
        }

        private void WriteOffBtn_Click(object sender, RoutedEventArgs e)
        {
            var addWriteOff = new AddWriteOffWindow((sender as Button).DataContext as book);
            addWriteOff.ShowDialog();
            UbdateDGLibrary();
        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(MessageBox.Show("Вы действительно хотите удалить эту книгу?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmp = (sender as Button).DataContext as book;

                tmp.is_deleted = true;

                try
                {
                    _db.SaveChanges();
                    MessageBox.Show("Книга удалена", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UbdateDGLibrary();
            }
        }

        private void BtnAddBook_CLick(object sender, RoutedEventArgs e)
        {
            var addBook = new AddBook(null);
            addBook.ShowDialog();
            UbdateDGLibrary();
        }

        private void SearchBookBtn_CLick(object sender, RoutedEventArgs e)
        {
            LibraryDataGrid.ItemsSource = _db.books.Where(b => b.is_deleted == false
                                                            && (b.name_book.Contains(SearchBookText.Text)
                                                            || b.author.human.first_name.Contains(SearchBookText.Text)
                                                            || b.author.human.last_name.Contains(SearchBookText.Text)
                                                            || b.genre.name_genre.Contains(SearchBookText.Text)
                                                            || b.publishing_house.name_pub_house.Contains(SearchBookText.Text))).ToList();
        }
    }
}
