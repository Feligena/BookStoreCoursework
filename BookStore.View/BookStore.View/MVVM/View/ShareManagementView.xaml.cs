using BookStore.View;
using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для ShareManagementView.xaml
    /// </summary>
    public partial class ShareManagementView : UserControl
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
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
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addPromoWindow = new AddPromotionWindow(null);
            addPromoWindow.ShowDialog();
            UpdatePromotionsDG();
        }

        private void GetBooksListBtn_Click(object sender, RoutedEventArgs e)
        {
            var getBooksOnPromoWindow = new GetPromoOnBooksWindow((sender as Button).DataContext as promotion);
            getBooksOnPromoWindow.ShowDialog();
        }

        private void EditPromoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var addPromoWindow = new AddPromotionWindow((sender as Button).DataContext as promotion);
            addPromoWindow.ShowDialog();
            UpdatePromotionsDG();
        }

        private void DeletePromoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminWindow._accessRights == false)
            {
                MessageBox.Show("У вас нет прав доступа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить акцию?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmpPromo = (sender as Button).DataContext as promotion;
                tmpPromo.is_deleted = true;

                var tmpPromoOnBooks = _db.promotion_on_books.Where(p => p.id_promotion == tmpPromo.id).ToList();
                _db.promotion_on_books.RemoveRange(tmpPromoOnBooks);

                try
                {
                    _db.SaveChanges();
                    MessageBox.Show("Акция удалена", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdatePromotionsDG();
            }
        }

        private void SearchPromoBtn_CLick(object sender, RoutedEventArgs e)
        {
            PromoDataGrid.ItemsSource = _db.promotions.Where(p => p.is_deleted == false
                                                            && (p.name_promotion.Contains(SearchPromoText.Text)
                                                            || p.description.Contains(SearchPromoText.Text)
                                                            || p.discount_percentage.ToString().Contains(SearchPromoText.Text)
                                                            || p.start_of_stock.ToString().Contains(SearchPromoText.Text)
                                                            || p.end_of_stock.ToString().Contains(SearchPromoText.Text))).ToList();
        }
    }
}
