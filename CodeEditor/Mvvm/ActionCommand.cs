using System;
using System.Windows.Input;

namespace CodeEditor.Mvvm;

public class ActionCommand : ICommand
{
    private readonly Action _action;

    public event EventHandler? CanExecuteChanged;

    public ActionCommand(Action action)
    {
        _action = action;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _action();
    }
}

public class ActionCommand<TParameter> : ICommand
{
    private readonly Action<TParameter> _action;

    public event EventHandler? CanExecuteChanged;

    public ActionCommand(Action<TParameter> action)
    {
        _action = action;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _action((TParameter)parameter);
    }
}
