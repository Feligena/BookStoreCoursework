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
        private DbbookstoreEntities _db = DbbookstoreEntities.GetContext();
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
