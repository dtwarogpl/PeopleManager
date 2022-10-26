using System.Threading.Tasks;
using System.Windows;
using PeopleManager.ViewModel.Abstractions;

 namespace PeopleManager.View
{
    public partial class MainWindow : Window
    {
        public MainWindow(IViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
