using BookStore.View.MVVM.Models;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для GetPromoOnBooksWindow.xaml
    /// </summary>
    public partial class GetPromoOnBooksWindow : Window
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public GetPromoOnBooksWindow(promotion selectedPromo)
        {
            InitializeComponent();

            if(selectedPromo != null)
            {
                PromoOnBooksDataGrid.ItemsSource = _db.promotion_on_books.Include(p => p.book)
                                                                         .Where(p => p.id_promotion == selectedPromo.id && p.book.is_deleted == false)
                                                                         .ToList();
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
