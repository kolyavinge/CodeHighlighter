using System;
using System.Collections.Generic;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Tests.Model;

internal class TestInputActionsFactory : IInputActionsFactory
{
    private readonly Dictionary<Type, object> _actions = new();

    public TestInputActionsFactory()
    {
        _actions.Add(typeof(IAppendCharInputAction), new AppendCharInputAction());
        _actions.Add(typeof(IAppendNewLineInputAction), new AppendNewLineInputAction());
        _actions.Add(typeof(IDeleteLeftTokenInputAction), new DeleteLeftTokenInputAction(this));
        _actions.Add(typeof(IDeleteRightTokenInputAction), new DeleteRightTokenInputAction(this));
        _actions.Add(typeof(IDeleteSelectedLinesInputAction), new DeleteSelectedLinesInputAction());
        _actions.Add(typeof(IGotoLineInputAction), new GotoLineInputAction(this));
        _actions.Add(typeof(IInsertTextInputAction), new InsertTextInputAction());
        _actions.Add(typeof(ILeftDeleteInputAction), new LeftDeleteInputAction());
        _actions.Add(typeof(IMoveCursorDownInputAction), new MoveCursorDownInputAction());
        _actions.Add(typeof(IMoveCursorEndLineInputAction), new MoveCursorEndLineInputAction());
        _actions.Add(typeof(IMoveCursorLeftInputAction), new MoveCursorLeftInputAction());
        _actions.Add(typeof(IMoveCursorPageDownInputAction), new MoveCursorPageDownInputAction());
        _actions.Add(typeof(IMoveCursorPageUpInputAction), new MoveCursorPageUpInputAction());
        _actions.Add(typeof(IMoveCursorRightInputAction), new MoveCursorRightInputAction());
        _actions.Add(typeof(IMoveCursorStartLineInputAction), new MoveCursorStartLineInputAction());
        _actions.Add(typeof(IMoveCursorTextBeginInputAction), new MoveCursorTextBeginInputAction());
        _actions.Add(typeof(IMoveCursorTextEndInputAction), new MoveCursorTextEndInputAction());
        _actions.Add(typeof(IMoveCursorToInputAction), new MoveCursorToInputAction());
        _actions.Add(typeof(IMoveCursorUpInputAction), new MoveCursorUpInputAction());
        _actions.Add(typeof(IMoveSelectedLinesDownInputAction), new MoveSelectedLinesDownInputAction());
        _actions.Add(typeof(IMoveSelectedLinesUpInputAction), new MoveSelectedLinesUpInputAction());
        _actions.Add(typeof(IMoveToNextTokenInputAction), new MoveToNextTokenInputAction());
        _actions.Add(typeof(IMoveToPrevTokenInputAction), new MoveToPrevTokenInputAction());
        _actions.Add(typeof(IRightDeleteInputAction), new RightDeleteInputAction());
        _actions.Add(typeof(IScrollLineDownInputAction), new ScrollLineDownInputAction());
        _actions.Add(typeof(IScrollLineUpInputAction), new ScrollLineUpInputAction());
        _actions.Add(typeof(ISelectAllInputAction), new SelectAllInputAction());
        _actions.Add(typeof(ISelectTokenInputAction), new SelectTokenInputAction());
        _actions.Add(typeof(ISetTextCaseInputAction), new SetTextCaseInputAction());
        _actions.Add(typeof(ISetTextInputAction), new SetTextInputAction());
    }

    public TInputAction Get<TInputAction>()
    {
        return (TInputAction)_actions[typeof(TInputAction)];
    }
}
