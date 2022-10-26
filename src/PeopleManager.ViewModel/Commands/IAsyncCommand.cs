using System.Threading.Tasks;
using System.Windows.Input;

namespace PeopleManager.ViewModel.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object parameter);
}