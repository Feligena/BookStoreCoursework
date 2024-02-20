using bookstore.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LibraryView.xaml
    /// </summary>
    public partial class LibraryView : UserControl
    {
        private DbbookstoreEntities _db = DbbookstoreEntities.GetContext();
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
            var addBook = new AddBook((sender as Button).DataContext as books);
            addBook.ShowDialog();
            UbdateDGLibrary();
        }

        private void WriteOffBtn_Click(object sender, RoutedEventArgs e)
        {

            UbdateDGLibrary();
        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {

            UbdateDGLibrary();
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
                                                                || b.genres.name_genre.Contains(SearchBookText.Text) 
                                                                || b.publishing_house.name_pub_house.Contains(SearchBookText.Text)))
                                                   .Join(_db.author,
                                                         b => b.id_author, au => au.id, (b, au) => new
                                                         {
                                                             b.id,
                                                             b.name_book,
                                                             au.id_human,
                                                             id_author = au.id,
                                                             nameauthor = au.human.last_name + " " + au.human.first_name + " " + au.human.patronymic,
                                                             pubHouse = b.publishing_house.name_pub_house,
                                                             b.year_publishing,
                                                             genre = b.genres.name_genre,
                                                             b.number_pages,
                                                             b.cost_price,
                                                             b.selling_price,
                                                             b.amount
                                                         }).ToList();
        }
    }
}
