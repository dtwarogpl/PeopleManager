using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PeopleManager.ViewModel.Commands;


public class AsyncCommand : AsyncCommandBase, INotifyPropertyChanged
{
    private readonly Func<Task> _command;
    private readonly Func<bool>? _func;
    private bool _running;

    public AsyncCommand(Func<Task> command, Func<bool> func)
    {
        _command = command;
        _func = func;
    }

    public bool Running
    {
        get => _running;
        set => SetField(ref _running, value);
    }

    public AsyncCommand(Func<Task> command)
    {
        _command = command;
    }

   
    public override bool CanExecute(object parameter)
    {
        if (_func is null)
        {
            return !Running;
        }
        return _func() && !Running;
    }
    public override async Task ExecuteAsync(object parameter)
    {
        Running = true;
         await _command();
         Running = false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}



