using System.Threading.Tasks;
using System.Windows;
using PeopleManager.ViewModel.Abstractions;

 namespace PeopleManager.View
{
    public partial class MainWindow : Window
    {
        private readonly IViewModel _viewModel;

        public MainWindow(IViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = viewModel;
        }

      
    }
}
