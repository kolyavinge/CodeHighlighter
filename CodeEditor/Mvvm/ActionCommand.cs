using System.Windows.Input;

namespace CodeEditor.Mvvm;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged;

    protected void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public abstract void Execute(object? parameter);
}

public class ActionCommand : Command
{
    private readonly Action _action;

    public ActionCommand(Action action)
    {
        _action = action;
    }

    public override void Execute(object? parameter)
    {
        _action();
    }
}

public class ActionCommand<TParameter> : Command
{
    private readonly Action<TParameter> _action;

    public ActionCommand(Action<TParameter> action)
    {
        _action = action;
    }

    public override void Execute(object? parameter)
    {
        _action((TParameter)parameter!);
    }
}
