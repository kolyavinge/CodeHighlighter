using System.Linq;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

internal interface IHistoryAction
{
    bool Do();
    void Undo();
    void Redo();
}

internal abstract class HistoryAction : IHistoryAction
{
    public abstract bool Do();
    public abstract void Undo();
    public abstract void Redo();
}

public interface IHistory
{
    bool CanRedo { get; }
    bool CanUndo { get; }
    void Redo();
    void Undo();
}

internal interface IHistoryInternal : IHistory
{
    void AddAndDo(IHistoryAction action);
}

internal class History : IHistoryInternal
{
    public const int ActionsLimit = 1000;

    public readonly LimitedCollection<IHistoryAction> _actions = new(ActionsLimit);
    public int _activeActionIndex = -1;

    public bool CanRedo => _actions.Any() && !IsLastActive();

    public bool CanUndo => _activeActionIndex >= 0;

    public void AddAndDo(IHistoryAction action)
    {
        if (!action.Do())
        {
            return;
        }
        if (!IsLastActive())
        {
            _actions.RemoveRange(_activeActionIndex + 1, _actions.Count - _activeActionIndex - 1);
        }
        if (!_actions.HasLimit)
        {
            _activeActionIndex++;
        }
        _actions.Add(action);
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
