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
using System.Windows.Shapes;

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для AddWriteOffWindow.xaml
    /// </summary>
    public partial class AddWriteOffWindow : Window
    {
        private write_offs _currentWriteOff = new write_offs() { books = new books(), employees = new employees() };
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public AddWriteOffWindow(books selectedBook)
        {
            InitializeComponent();

            if (selectedBook != null)
            {
                _currentWriteOff.books = selectedBook;
                _currentWriteOff.amount = selectedBook.amount;
                _currentWriteOff.id = 0;
                AddEditWriteOff.Text = "Списание книги";
            }

            DataContext = _currentWriteOff;

            EmployeeComboBox.ItemsSource = _db.employees.Where(em => em.is_deleted == false).ToList();
        }

        private void WriteOffBookBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentWriteOff.employees.id == 0)
                errors.AppendLine("Укажите сотрудника, который проводит списание");

            if(_currentWriteOff.amount == 0)
                errors.AppendLine("Укажите количество книг, подлежащих списанию");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(_currentWriteOff.amount > _currentWriteOff.books.amount)
            {
                MessageBox.Show("Списываемых книг не может быть больше, чем имеющихся в наличае", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentWriteOff.id == 0)
            {
                _currentWriteOff.books.amount -= _currentWriteOff.amount;

                if(_currentWriteOff.books.amount == 0)
                    _currentWriteOff.books.is_deleted = true;

                _currentWriteOff.date_write_offs = DateTime.Now;
                _db.write_offs.Add(_currentWriteOff);
            }

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Информация о списании сохранена", "Успешно!",MessageBoxButton.OK, MessageBoxImage.Asterisk);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
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
