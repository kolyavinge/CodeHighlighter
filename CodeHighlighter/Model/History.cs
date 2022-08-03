using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal interface IHistoryAction
{
    abstract bool Do();
    abstract void Undo();
    abstract void Redo();
}

internal abstract class HistoryAction : IHistoryAction
{
    public abstract bool Do();
    public abstract void Undo();
    public abstract void Redo();
}

public class History
{
    internal readonly List<IHistoryAction> _actions = new();
    internal int _activeActionIndex = -1;

    public bool CanRedo => _actions.Any() && !IsLastActive();

    public bool CanUndo => _activeActionIndex >= 0;

    internal void AddAndDo(IHistoryAction action)
    {
        if (!action.Do())
        {
            return;
        }
        if (!IsLastActive())
        {
            _actions.RemoveRange(_activeActionIndex + 1, _actions.Count - _activeActionIndex - 1);
        }
        _actions.Add(action);
        _activeActionIndex++;
    }

    public void Redo()
    {
        if (!_actions.Any()) return;
        if (IsLastActive()) return;
        _activeActionIndex++;
        _actions[_activeActionIndex].Redo();
    }

    public void Undo()
    {
        if (_activeActionIndex == -1) return;
        _actions[_activeActionIndex].Undo();
        _activeActionIndex--;
    }

    private bool IsLastActive()
    {
        return _activeActionIndex != -1 && _actions[_activeActionIndex] == _actions.Last();
    }
}
