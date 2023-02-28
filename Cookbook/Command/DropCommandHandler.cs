using System;
using System.Windows.Input;

namespace Cookbook.Command;

public class DropCommandHandler : ICommand
{
    private Action<object> _action;
    private Func<bool> _canExecute;
    
    public DropCommandHandler(Action<object> action, Func<bool> canExecute)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public DropCommandHandler(Action<object> action)
    {
        _action = action;
        _canExecute = () => true;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute.Invoke();
    }

    public void Execute(object? parameter)
    {
        _action(parameter);
    }
}