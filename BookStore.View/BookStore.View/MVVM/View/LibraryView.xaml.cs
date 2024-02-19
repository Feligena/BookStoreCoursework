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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LibraryView.xaml
    /// </summary>
    public partial class LibraryView : UserControl
    {
        private DbBookstore _db = DbBookstore.GetContext();
        public LibraryView()
        {
            InitializeComponent();
            LibraryDataGrid.ItemsSource = _db.Books.Where(b => b.is_deleted == false)
                                                   .Join(_db.Author,
                                                         b => b.id_Author, au => au.id, (b, au) => new
                                                         {
                                                             b.id,
                                                             b.name_book,
                                                             au.id_Human,
                                                             id_author = au.id,
                                                             nameAuthor = au.Human.last_name + " " + au.Human.first_name + " " + au.Human.patronymic,
                                                             pubHouse = b.publishing_house.name_pub_house,
                                                             b.year_publishing,
                                                             genre = b.Genres.name_genre,
                                                             b.number_pages,
                                                             b.cost_price,
                                                             b.selling_price,
                                                             b.amount
                                                         }).ToList();
        }

        private void EditBookBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteOffBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBookBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddBook_CLick(object sender, RoutedEventArgs e)
        {
            var addBook = new AddBook();
            addBook.ShowDialog();
        }

        private void SearchBookBtn_CLick(object sender, RoutedEventArgs e)
        {
            LibraryDataGrid.ItemsSource = _db.Books.Where(b => b.is_deleted == false 
                                                            && (b.name_book.Contains(SearchBookText.Text) 
                                                                || b.Author.Human.first_name.Contains(SearchBookText.Text) 
                                                                || b.Author.Human.last_name.Contains(SearchBookText.Text) 
                                                                || b.Genres.name_genre.Contains(SearchBookText.Text) 
                                                                || b.publishing_house.name_pub_house.Contains(SearchBookText.Text)))
                                                   .Join(_db.Author,
                                                         b => b.id_Author, au => au.id, (b, au) => new
                                                         {
                                                             b.id,
                                                             b.name_book,
                                                             au.id_Human,
                                                             id_author = au.id,
                                                             nameAuthor = au.Human.last_name + " " + au.Human.first_name + " " + au.Human.patronymic,
                                                             pubHouse = b.publishing_house.name_pub_house,
                                                             b.year_publishing,
                                                             genre = b.Genres.name_genre,
                                                             b.number_pages,
                                                             b.cost_price,
                                                             b.selling_price,
                                                             b.amount
                                                         }).ToList();
        }
    }
}
