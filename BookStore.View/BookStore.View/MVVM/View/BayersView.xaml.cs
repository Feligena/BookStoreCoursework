using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bookstore.View.MVVM.ViewModel;

namespace Bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для BayersView.xaml
    /// </summary>
    public partial class BayersView : UserControl
    {
        private DbBookstore _db = DbBookstore.GetContext();
        public BayersView()
        {
            InitializeComponent();
            UserDataGrid.ItemsSource = _db.users.Where(u => u.is_deleted == false).ToList();
        }

        private void ButtonAddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow();
            //addUserWindow.Owner.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //addUserWindow.ShowDialog();
            addUserWindow.ShowDialog();
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
