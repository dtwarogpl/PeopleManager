using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace PeopleManager.ViewModel.Commands;


public class AsyncCommand : AsyncCommandBase
{
    private readonly Func<Task> _command;
    public AsyncCommand(Func<Task> command)
    {
        _command = command;
    }
    public override bool CanExecute(object parameter)
    {
        return true;
    }
    public override Task ExecuteAsync(object parameter)
    {
        return _command();
    }
}