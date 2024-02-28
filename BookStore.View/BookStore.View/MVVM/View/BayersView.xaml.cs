using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using bookstore.View.MVVM.ViewModel;
using bookstore.View;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для BayersView.xaml
    /// </summary>
    public partial class BayersView : UserControl
    {
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public BayersView()
        {
            InitializeComponent();
            UpdateUsersDG();
        }

        private void UpdateUsersDG()
        {
            UserDataGrid.ItemsSource = _db.users.Where(u => u.is_deleted == false).ToList();
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow(null);
            addUserWindow.ShowDialog();
            UpdateUsersDG();
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow((sender as Button).DataContext as users);
            addUserWindow.ShowDialog();
            UpdateUsersDG();
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить покупателя?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var tmp = (sender as Button).DataContext as users;
                tmp.is_deleted = true;
                tmp.human.is_deleted = true;

                try
                {
                    _db.SaveChanges();
                    MessageBox.Show("Покупатель удален", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            UpdateUsersDG();
        }

        private void SearchBayersBtn_CLick(object sender, RoutedEventArgs e)
        {
            UserDataGrid.ItemsSource = _db.users.Where(u => u.is_deleted == false
                                                            && (u.human.first_name.Contains(SearchBayersText.Text)
                                                            || u.human.last_name.Contains(SearchBayersText.Text)
                                                            || u.human.patronymic.Contains(SearchBayersText.Text)
                                                            || u.phone.ToString().Contains(SearchBayersText.Text))).ToList();
        }
    }
}
