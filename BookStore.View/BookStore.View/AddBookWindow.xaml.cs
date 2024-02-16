using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace Bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private DbBookstore _db = DbBookstore.GetContext();
        public AddBook()
        {
            InitializeComponent();
            AuthorComboBox.ItemsSource = _db.Author.Where(a => a.is_deleted == false).Join(_db.Human, a => a.id_Human, h => h.id,
                                                            (a, h) => new
                                                            {
                                                                a.id,
                                                                nameAuthor = h.last_name + " " + h.first_name + " " + h.patronymic
                                                            }).ToList();
            PublishingComboBox.ItemsSource = _db.publishing_house.ToList();
            GenresComboBox.ItemsSource = _db.Genres.ToList();
        }

        private void SaveBookBtn_Click(object sender, RoutedEventArgs e)
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
