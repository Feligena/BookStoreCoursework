using bookstore.View;
using BookStore.View.MVVM.Models;
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

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для AddPromotionWindow.xaml
    /// </summary>
    public partial class AddPromotionWindow : Window
    {

        private promotion _currentPromo = new promotion() { start_of_stock = DateTime.Now, end_of_stock = DateTime.Now };
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public AddPromotionWindow(promotion selectedPromo)
        {
            InitializeComponent();

            if(selectedPromo != null)
            {
                _currentPromo = selectedPromo;
                _currentPromo.promotion_on_books = _db.promotion_on_books.Where(p => p.id_promotion == _currentPromo.id).ToList();

                AddEditPromo.Text = "Редактировать акцию";
            }
            else AddEditPromo.Text = "Добавить акцию";

            DataContext = _currentPromo;

            BooksListBox.ItemsSource = _db.books.Where(b => b.is_deleted == false).ToList();
        }

        private void SavePromoBtn_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPromo.name_promotion))
                errors.AppendLine("Укажите название акции");

            if (_currentPromo.discount_percentage == 0)
                errors.AppendLine("Укажите процент скидки");

            if (string.IsNullOrWhiteSpace(_currentPromo.description))
                errors.AppendLine("Укажите описание акции");

            if (_currentPromo.start_of_stock == null)
                errors.AppendLine("Укажите дату начала акции");

            if (string.IsNullOrWhiteSpace(_currentPromo.end_of_stock.ToString()))
                errors.AppendLine("Укажите дату окончания акции");

            if (_currentPromo.promotion_on_books.Count == 0 && BooksListBox.SelectedItems.Count == 0) //BooksListBox.SelectedItems.Count == 0 && 
                errors.AppendLine("Выберите книги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var listBooks = BooksListBox.SelectedItems.Cast<book>().ToList();
            int itdexPromo = _currentPromo.id;
            var tmpArr = new List<book>();

            if (_currentPromo.id == 0)
            {
                if (_db.promotions.Any(p => p.name_promotion == _currentPromo.name_promotion
                                        && p.discount_percentage == _currentPromo.discount_percentage
                                        && p.start_of_stock == _currentPromo.start_of_stock
                                        && p.end_of_stock == _currentPromo.end_of_stock
                                        && p.is_deleted == false))
                {
                    MessageBox.Show("Такая акция уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _db.promotions.Add(_currentPromo);

                try 
                { 
                    _db.SaveChanges();
                    itdexPromo = _db.promotions.ToList().Last().id;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }   

            if (_currentPromo.promotion_on_books.Count > 0)
            {
                var tmp = _db.promotion_on_books.Where(p => p.id_promotion == _currentPromo.id).ToList();

                foreach (var promo in tmp)
                    tmpArr.Add(promo.book);
            }

            foreach (var book in listBooks)
            {
                if (tmpArr.Any(t => t.id == book.id)) break;

                var promoOnBooks = new promotion_on_books() { id_book = book.id, id_promotion = itdexPromo };
                _db.promotion_on_books.Add(promoOnBooks);
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
