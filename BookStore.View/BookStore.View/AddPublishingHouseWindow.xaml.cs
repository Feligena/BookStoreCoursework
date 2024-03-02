using BookStore.View.MVVM.Models;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddPublishingHouseWindow.xaml
    /// </summary>
    public partial class AddPublishingHouseWindow : Window
    {
        private publishing_house _currentPubHouse = new publishing_house();
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public AddPublishingHouseWindow()
        {
            InitializeComponent();

            AddEditPubHouse.Text = "Добавить издательство";
            DataContext = _currentPubHouse;
        }

        private void SaveNewPubHouse_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPubHouse.name_pub_house))
                errors.AppendLine("Укажите название издательства");

            if (string.IsNullOrWhiteSpace(_currentPubHouse.address))
                errors.AppendLine("Укажите юр.адрес организации");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentPubHouse.id == 0)
            {
                if (_db.publishing_house.Any(p => p.name_pub_house == _currentPubHouse.name_pub_house && p.is_deleted == false))
                {
                    MessageBox.Show("Такое издательство уже существует", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else _db.publishing_house.Add(_currentPubHouse);
            }
                

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлено новое издательство", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    this.Close();
            }
            catch(Exception ex)
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
