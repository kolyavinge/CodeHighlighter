using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Common;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

internal class CodeTextBoxModel : ICodeTextBoxModel
{
    private ICodeTextBox _codeTextBox;
    private readonly IText _text;
    private readonly ITextCursor _textCursor;
    private readonly ITextSelector _textSelector;
    private readonly ITokens _tokens;
    private readonly IHistoryInternal _history;
    private readonly IInputActionContext _inputActionContext;
    private readonly IInputActionsFactory _inputActionsFactory;
    private readonly IHistoryActionsFactory _historyActionsFactory;

    public string Text
    {
        get => _text.TextContent;
        set
        {
            if (IsReadOnly) return;
            _history.AddAndDo(_historyActionsFactory.Get<ISetTextHistoryAction>().SetParams(value));
        }
    }

    public IEnumerable<string> TextLines => _text.Lines.Select(x => x.ToString()).ToList();

    public int TextLinesCount => _text.LinesCount;

    public CursorPosition CursorPosition => _textCursor.Position;

    public Point AbsoluteCursorPosition => _textCursor.GetAbsolutePosition(TextMeasures);

    public ITextSelection TextSelection { get; }

    public ITextMeasures TextMeasures { get; }

    public ITextEvents TextEvents { get; }

    public ITokenCollection Tokens => _tokens;

    public ITokensColorCollection TokensColors { get; }

    public IHistory History => _history;

    public ILinesDecorationCollection LinesDecoration { get; }

    public IViewport Viewport { get; private set; }

    public IBracketsHighlighter BracketsHighlighter { get; }

    public bool IsReadOnly { get; set; }

    public CodeTextBoxModel(
        ICodeProvider codeProvider,
        IText text,
        ITextCursor textCursor,
        ITextSelectionInternal textSelection,
        ITextSelector textSelector,
        ITextMeasures textMeasures,
        ITextEvents textEvents,
        ITokens tokens,
        ITokensColors tokensColors,
        IHistoryInternal history,
        ILinesDecorationCollection linesDecoration,
        IViewport viewport,
        IBracketsHighlighter bracketsHighlighter,
        IInputActionContext inputActionContext,
        IInputActionsFactory inputActionsFactory,
        IHistoryActionsFactory historyActionsFactory,
        CodeTextBoxModelAdditionalParams additionalParams)
    {
        _codeTextBox = DummyCodeTextBox.Instance;
        _text = text;
        _textCursor = textCursor;
        _textSelector = textSelector;
        _tokens = tokens;
        _history = history;
        _inputActionContext = inputActionContext;
        _inputActionsFactory = inputActionsFactory;
        _historyActionsFactory = historyActionsFactory;
        TextSelection = textSelection;
        TextMeasures = textMeasures;
        TextEvents = textEvents;
        TokensColors = tokensColors;
        LinesDecoration = linesDecoration;
        Viewport = viewport;
        BracketsHighlighter = bracketsHighlighter;
        IsReadOnly = additionalParams.IsReadOnly;
        SetCodeProvider(codeProvider);
    }

    private void SetCodeProvider(ICodeProvider codeProvider)
    {
        if (codeProvider is ITokenKindUpdatable tokenKindUpdatable)
        {
            var tokenKindUpdater = new TokenKindUpdater(_tokens);
            tokenKindUpdatable.TokenKindUpdated += (s, e) =>
            {
                tokenKindUpdater.UpdateTokenKinds(e.UpdatedTokenKinds);
                _codeTextBox.InvalidateVisual();
            };
        }
    }

    public void AttachCodeTextBox(ICodeTextBox codeTextBox)
    {
        _codeTextBox = codeTextBox;
        Viewport = new Viewport(_text, codeTextBox, _textCursor, TextMeasures);
        _inputActionContext.CodeTextBox = _codeTextBox;
        _inputActionContext.Viewport = Viewport;
    }

    public string GetSelectedText()
    {
        return _textSelector.GetSelectedText();
    }

    public void MoveCursorTo(CursorPosition position)
    {
        _inputActionsFactory.Get<IMoveCursorToInputAction>().Do(_inputActionContext, position);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorLeft()
    {
        _inputActionsFactory.Get<IMoveCursorLeftInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorRight()
    {
        _inputActionsFactory.Get<IMoveCursorRightInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorUp()
    {
        _inputActionsFactory.Get<IMoveCursorUpInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorDown()
    {
        _inputActionsFactory.Get<IMoveCursorDownInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorStartLine()
    {
        _inputActionsFactory.Get<IMoveCursorStartLineInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorEndLine()
    {
        _inputActionsFactory.Get<IMoveCursorEndLineInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorTextBegin()
    {
        _inputActionsFactory.Get<IMoveCursorTextBeginInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorTextEnd()
    {
        _inputActionsFactory.Get<IMoveCursorTextEndInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorPageUp()
    {
        _inputActionsFactory.Get<IMoveCursorPageUpInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorPageDown()
    {
        _inputActionsFactory.Get<IMoveCursorPageDownInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveSelectedLinesUp()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IMoveSelectedLinesUpHistoryAction>());
    }

    public void MoveSelectedLinesDown()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IMoveSelectedLinesDownHistoryAction>());
    }

    public void ActivateSelection()
    {
        _textSelector.ActivateSelection();
    }

    public void CompleteSelection()
    {
        _textSelector.CompleteSelection();
    }

    public void GotoLine(int lineIndex)
    {
        _inputActionsFactory.Get<IGotoLineInputAction>().Do(_inputActionContext, lineIndex);
        _codeTextBox.InvalidateVisual();
        _codeTextBox.Focus();
    }

    public void ScrollLineUp()
    {
        _inputActionsFactory.Get<IScrollLineUpInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void ScrollLineDown()
    {
        _inputActionsFactory.Get<IScrollLineDownInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveToPrevToken()
    {
        _inputActionsFactory.Get<IMoveToPrevTokenInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveToNextToken()
    {
        _inputActionsFactory.Get<IMoveToNextTokenInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void SelectAll()
    {
        _inputActionsFactory.Get<ISelectAllInputAction>().Do(_inputActionContext);
        _codeTextBox.InvalidateVisual();
    }

    public void SelectToken(CursorPosition position)
    {
        _inputActionsFactory.Get<ISelectTokenInputAction>().Do(_inputActionContext, position);
        _codeTextBox.InvalidateVisual();
    }

    public void DeleteLeftToken()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IDeleteLeftTokenHistoryAction>());
    }

    public void DeleteRightToken()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IDeleteRightTokenHistoryAction>());
    }

    public void AppendChar(char ch)
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IAppendCharHistoryAction>().SetParams(ch));
    }

    public void AppendNewLine()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IAppendNewLineHistoryAction>());
    }

    public void InsertText(string insertedText)
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IInsertTextHistoryAction>().SetParams(insertedText));
    }

    public void DeleteSelectedLines()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IDeleteSelectedLinesHistoryAction>());
    }

    public void LeftDelete()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<ILeftDeleteHistoryAction>());
    }

    public void RightDelete()
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<IRightDeleteHistoryAction>());
    }

    public void SetTextCase(TextCase textCase)
    {
        if (IsReadOnly) return;
        _history.AddAndDo(_historyActionsFactory.Get<ISetTextCaseHistoryAction>().SetParams(textCase));
    }
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
    public bool IsReadOnly { get; set; }
}
