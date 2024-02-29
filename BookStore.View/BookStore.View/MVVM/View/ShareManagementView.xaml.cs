using BookStore.View;
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
    /// Логика взаимодействия для ShareManagementView.xaml
    /// </summary>
    public partial class ShareManagementView : UserControl
    {
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public ShareManagementView()
        {
            InitializeComponent();

            UpdatePromotionsDG();
        }

        private void UpdatePromotionsDG()
        {
            PromoDataGrid.ItemsSource = _db.promotions.Where(p => p.is_deleted == false).ToList();
        }

        private void AddPromotionBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPromoWindow = new AddPromotionWindow(null);
            addPromoWindow.ShowDialog();
            UpdatePromotionsDG();
        }

        private void GetBooksListBtn_Click(object sender, RoutedEventArgs e)
        {
            var getBooksOnPromoWindow = new GetPromoOnBooksWindow((sender as Button).DataContext as promotions);
            getBooksOnPromoWindow.ShowDialog();
        }

        private void EditPromoBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPromoWindow = new AddPromotionWindow((sender as Button).DataContext as promotions);
            addPromoWindow.ShowDialog();
            UpdatePromotionsDG();
        }

        private void DeletePromoBtn_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
