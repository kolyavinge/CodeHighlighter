using CodeHighlighter.Core;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Infrastructure;

internal interface IHistoryActionsFactory
{
    THistoryAction Make<THistoryAction>() where THistoryAction : IHistoryAction;
}

internal class HistoryActionsFactory : IHistoryActionsFactory
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private readonly IInputActionContext _context;

    public HistoryActionsFactory(IInputActionsFactory inputActionsFactory, IInputActionContext context)
    {
        _inputActionsFactory = inputActionsFactory;
        _context = context;
    }

    public THistoryAction Make<THistoryAction>() where THistoryAction : IHistoryAction
    {
        return (THistoryAction)(IHistoryAction)Make(typeof(THistoryAction));
    }

    private object Make(Type t)
    {
        if (t == typeof(IAppendCharHistoryAction)) return new AppendCharHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IAppendNewLineHistoryAction)) return new AppendNewLineHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IDeleteLeftTokenHistoryAction)) return new DeleteLeftTokenHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IDeleteRightTokenHistoryAction)) return new DeleteRightTokenHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IDeleteSelectedLinesHistoryAction)) return new DeleteSelectedLinesHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IInsertTextHistoryAction)) return new InsertTextHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(ILeftDeleteHistoryAction)) return new LeftDeleteHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IMoveSelectedLinesDownHistoryAction)) return new MoveSelectedLinesDownHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IMoveSelectedLinesUpHistoryAction)) return new MoveSelectedLinesUpHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(IRightDeleteHistoryAction)) return new RightDeleteHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(ISetTextCaseHistoryAction)) return new SetTextCaseHistoryAction(_inputActionsFactory, _context);
        if (t == typeof(ISetTextHistoryAction)) return new SetTextHistoryAction(_inputActionsFactory, _context);

        throw new ArgumentException();
    }
}
