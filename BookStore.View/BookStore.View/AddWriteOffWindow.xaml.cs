using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для AddWriteOffWindow.xaml
    /// </summary>
    public partial class AddWriteOffWindow : Window
    {
        private write_offs _currentWriteOff = new write_offs() { book = new book(), employee = new employee() };
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public AddWriteOffWindow(book selectedBook)
        {
            InitializeComponent();

            if (selectedBook != null)
            {
                _currentWriteOff.book = selectedBook;
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

            if (_currentWriteOff.employee.id == 0)
                errors.AppendLine("Укажите сотрудника, который проводит списание");

            if(_currentWriteOff.amount == 0)
                errors.AppendLine("Укажите количество книг, подлежащих списанию");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(_currentWriteOff.amount > _currentWriteOff.book.amount)
            {
                MessageBox.Show("Списываемых книг не может быть больше, чем имеющихся в наличае", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentWriteOff.id == 0)
            {
                _currentWriteOff.book.amount -= _currentWriteOff.amount;

                if(_currentWriteOff.book.amount == 0)
                    _currentWriteOff.book.is_deleted = true;

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
