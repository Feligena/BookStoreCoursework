using bookstore.View.Core;

namespace bookstore.View.MVVM.ViewModel
{
    class AdminViewModel : ObservableObject
    {
        public RelayCommand LibraryViewCommand { get; set; }
        public RelayCommand SelectionViewCommand { get; set; }
        public RelayCommand ShareManagementViewCommand { get; set; }
        public RelayCommand BayersViewCommand { get; set; }
        public RelayCommand EmployeeViewCommand { get; set; }
        public RelayCommand WriteOffsViewCommand { get; set; }
        

        public LibraryViewModel LibraryVM { get; set; }
        public SelectionViewModel SelectionVM { get; set; }
        public ShareManagementViewModel ShareManagementVM { get; set; }
        public BayersViewModel BayersVM { get; set; }
        public EmployeeViewModel EmployeeVM { get; set; }
        public WriteOffsViewModel WriteOffsVM { get; set; }
        

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public AdminViewModel() 
        {
            LibraryVM = new LibraryViewModel();
            SelectionVM = new SelectionViewModel();
            ShareManagementVM= new ShareManagementViewModel();
            BayersVM = new BayersViewModel();
            EmployeeVM = new EmployeeViewModel();
            WriteOffsVM = new WriteOffsViewModel();
            

            CurrentView = LibraryVM;

            LibraryViewCommand = new RelayCommand(c => { CurrentView = LibraryVM; });

            SelectionViewCommand = new RelayCommand(c =>{ CurrentView = SelectionVM; });

            ShareManagementViewCommand = new RelayCommand(c => { CurrentView = ShareManagementVM; });

            BayersViewCommand = new RelayCommand(c => { CurrentView = BayersVM; });

            EmployeeViewCommand = new RelayCommand(c => { CurrentView = EmployeeVM; });

            WriteOffsViewCommand = new RelayCommand(c => { CurrentView = WriteOffsVM; });

            
        }
    }
}
