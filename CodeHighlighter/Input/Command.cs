using System;
using System.Windows.Input;

namespace CodeHighlighter.Input;

public abstract class Command : ICommand
{
    public virtual bool CanExecute(object parameter)
    {
        return true;
    }

    public abstract void Execute(object parameter);

    public void Execute()
    {
        Execute(null!);
    }

    public event EventHandler? CanExecuteChanged;
}
