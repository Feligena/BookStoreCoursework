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
using System.Windows.Shapes;

namespace BookStore.View
{
    /// <summary>
    /// Логика взаимодействия для AddPublishingHouseWindow.xaml
    /// </summary>
    public partial class AddPublishingHouseWindow : Window
    {
        private publishing_house _currentPubHouse = new publishing_house();
        private DbBookstore _db = DbBookstore.GetContext();
        public AddPublishingHouseWindow()
        {
            InitializeComponent();
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
                MessageBox.Show(errors.ToString(), "Ошибка!");
                return;
            }

            if (_db.publishing_house.Any(p => p.name_pub_house == _currentPubHouse.name_pub_house && p.is_deleted == false))
            {
                MessageBox.Show("Такое издательство уже существует");
                return;
            }

            if (_currentPubHouse.id == 0)
                _db.publishing_house.Add(_currentPubHouse);

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлено новое издательство");
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
