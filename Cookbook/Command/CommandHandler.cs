﻿using System;
using System.Windows.Input;

namespace Cookbook.Command;

public class CommandHandler : ICommand
{
    private Action _action;
    private Func<bool> _canExecute;
    
    public CommandHandler(Action action, Func<bool> canExecute)
    {
        _action = action;
        _canExecute = canExecute;
    }

    public CommandHandler(Action action)
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
        _action();
    }
}