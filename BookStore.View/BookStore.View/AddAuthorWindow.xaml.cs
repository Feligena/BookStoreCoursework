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

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddauthorWindow.xaml
    /// </summary>
    public partial class AddauthorWindow : Window
    {
        private author _currentauthor = new author() { human = new human() };
        private DbBookstoreEntities _db = DbBookstoreEntities.GetContext();
        public AddauthorWindow()
        {
            InitializeComponent();
            DataContext = _currentauthor;
        }

        private void SaveNewauthor_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentauthor.human.first_name))
                errors.AppendLine("Укажите имя");

            if (string.IsNullOrWhiteSpace(_currentauthor.human.last_name))
                errors.AppendLine("Укажите фамилию");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentauthor.id == 0)
            {
                if (_db.author.Any(a => a.human.last_name == _currentauthor.human.last_name
                                    && a.human.first_name == _currentauthor.human.first_name
                                    && a.human.patronymic == _currentauthor.human.patronymic
                                    && a.is_deleted == false))
                {
                    MessageBox.Show("Такой автор уже существует");
                    return;
                }
                else _db.author.Add(_currentauthor);
            }
                
            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлен новый автор");
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
