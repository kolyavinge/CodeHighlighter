using CodeHighlighter.CodeProvidering;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private readonly IInputActionContext _inputActionContext;
    private readonly IInputActionsFactory _inputActionsFactory;
    private ICodeTextBox _codeTextBox;

    public IText Text { get; }
    public ITokens Tokens { get; }
    public ITokensColors TokensColors { get; }
    public ITextCursor TextCursor { get; }
    public ITextMeasures TextMeasures { get; }
    public ITextSelection TextSelection { get; }
    internal ITextSelector TextSelector { get; }
    public IHistory History { get; }
    public ILinesDecorationCollection LinesDecoration { get; }
    public IViewport Viewport { get; private set; }
    public IBracketsHighlighter BracketsHighlighter { get; }
    public ITextEvents TextEvents { get; }
    public bool IsReadOnly { get; set; }

    public CodeTextBoxModel(
        IText text,
        ITextCursor textCursor,
        ITokens tokens,
        ITokensColors tokensColors,
        ITextMeasures textMeasures,
        IHistory history,
        ILinesDecorationCollection linesDecoration,
        ITextSelection textSelection,
        ITextSelector textSelector,
        IViewport viewport,
        IBracketsHighlighter bracketsHighlighter,
        ITextEvents textEvents,
        ICodeProvider codeProvider,
        IInputActionsFactory inputActionsFactory,
        IInputActionContext inputActionContext,
        CodeTextBoxModelAdditionalParams additionalParams)
    {
        _codeTextBox = DummyCodeTextBox.Instance;
        Text = text;
        TextCursor = textCursor;
        Tokens = tokens;
        TokensColors = tokensColors;
        TextMeasures = textMeasures;
        History = history;
        LinesDecoration = linesDecoration;
        TextSelection = textSelection;
        TextSelector = textSelector;
        Viewport = viewport;
        BracketsHighlighter = bracketsHighlighter;
        TextEvents = textEvents;
        _inputActionsFactory = inputActionsFactory;
        _inputActionContext = inputActionContext;
        IsReadOnly = additionalParams.IsReadOnly;
        SetCodeProvider(codeProvider);
    }

    private void SetCodeProvider(ICodeProvider codeProvider)
    {
        if (codeProvider is ITokenKindUpdatable tokenKindUpdatable)
        {
            var tokenKindUpdater = new TokenKindUpdater(Tokens);
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
        Viewport = new Viewport(Text, codeTextBox, TextCursor, TextMeasures);
        _inputActionContext.CodeTextBox = _codeTextBox;
        _inputActionContext.Viewport = Viewport;
    }

    public void SetText(string text)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new SetTextHistoryAction(_inputActionsFactory, _inputActionContext, text));
    }

    public string GetSelectedText()
    {
        return TextSelector.GetSelectedText();
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
        History.AddAndDo(new MoveSelectedLinesUpHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void MoveSelectedLinesDown()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new MoveSelectedLinesDownHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void ActivateSelection()
    {
        TextSelector.ActivateSelection();
    }

    public void CompleteSelection()
    {
        TextSelector.CompleteSelection();
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
        History.AddAndDo(new DeleteLeftTokenHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void DeleteRightToken()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new DeleteRightTokenHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void AppendChar(char ch)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new AppendCharHistoryAction(_inputActionsFactory, _inputActionContext, ch));
    }

    public void AppendNewLine()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new AppendNewLineHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void InsertText(string insertedText)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new InsertTextHistoryAction(_inputActionsFactory, _inputActionContext, insertedText));
    }

    public void DeleteSelectedLines()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new DeleteSelectedLinesHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void LeftDelete()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new LeftDeleteHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void RightDelete()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new RightDeleteHistoryAction(_inputActionsFactory, _inputActionContext));
    }

    public void SetTextCase(TextCase textCase)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new SetTextCaseHistoryAction(_inputActionsFactory, _inputActionContext, textCase));
    }
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
    public bool IsReadOnly { get; set; }
}
