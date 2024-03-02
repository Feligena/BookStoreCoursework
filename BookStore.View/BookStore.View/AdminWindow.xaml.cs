using BookStore.View.MVVM.Models;
using System.Windows;
using System.Windows.Input;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public static AdminWindow _adminWindow;
        public static bool _accessRights {  get; set; }

        public AdminWindow(bool accesses)
        {
            InitializeComponent();
            _adminWindow = this;


            _accessRights = accesses;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            // Application.Current.Shutdown();

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
