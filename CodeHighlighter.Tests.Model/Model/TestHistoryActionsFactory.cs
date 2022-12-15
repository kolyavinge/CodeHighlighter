using System;
using System.Collections.Generic;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Tests.Model;

internal class TestHistoryActionsFactory : IHistoryActionsFactory
{
    private readonly Dictionary<Type, object> _actions = new();

    public TestHistoryActionsFactory(IInputActionsFactory inputActionsFactory, IInputActionContext context)
    {
        _actions.Add(typeof(IAppendCharHistoryAction), new AppendCharHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IAppendNewLineHistoryAction), new AppendNewLineHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IDeleteLeftTokenHistoryAction), new DeleteLeftTokenHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IDeleteRightTokenHistoryAction), new DeleteRightTokenHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IDeleteSelectedLinesHistoryAction), new DeleteSelectedLinesHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IInsertTextHistoryAction), new InsertTextHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(ILeftDeleteHistoryAction), new LeftDeleteHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IMoveSelectedLinesDownHistoryAction), new MoveSelectedLinesDownHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IMoveSelectedLinesUpHistoryAction), new MoveSelectedLinesUpHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(IRightDeleteHistoryAction), new RightDeleteHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(ISetTextCaseHistoryAction), new SetTextCaseHistoryAction(inputActionsFactory, context));
        _actions.Add(typeof(ISetTextHistoryAction), new SetTextHistoryAction(inputActionsFactory, context));
    }

    public TInputAction Get<TInputAction>()
    {
        return (TInputAction)_actions[typeof(TInputAction)];
    }
}
