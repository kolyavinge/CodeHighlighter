using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private readonly HistoryActionContext _historyActionContext;
    private ICodeTextBox? _codeTextBox;

    public event EventHandler? TextChanged;

    public Text Text { get; }
    public Tokens Tokens { get; }
    public TextCursor TextCursor { get; }
    public TextMeasures TextMeasures { get; }
    public TextSelection TextSelection { get; }
    public History History { get; }

    internal IViewportContext ViewportContext { get; set; }
    internal Viewport Viewport { get; private set; }
    internal FontSettings FontSettings { get; }
    internal InputModel InputModel { get; }
    internal BracketsHighlighter BracketsHighlighter { get; }

    public CodeTextBoxModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        Text = new Text();
        TextCursor = new TextCursor(Text);
        Tokens = new Tokens();
        FontSettings = new FontSettings();
        TextMeasures = new TextMeasures(FontSettings);
        History = new History();
        TextSelection = new TextSelection(0, 0, 0, 0);
        InputModel = new InputModel(Text, TextCursor, TextSelection, Tokens);
        ViewportContext = new DummyViewportContext();
        Viewport = new Viewport(ViewportContext, TextMeasures);
        BracketsHighlighter = new BracketsHighlighter(additionalParams?.HighlighteredBrackets ?? "", Text, TextCursor);
        _historyActionContext = new HistoryActionContext(InputModel, Text, TextCursor, TextMeasures, TextSelection, Viewport, ViewportContext, RaiseTextChanged);
        SetCodeProvider(codeProvider);
    }

    internal void Init(ICodeTextBox? codeTextBox, IViewportContext viewportContext)
    {
        _codeTextBox = codeTextBox;
        ViewportContext = viewportContext;
        Viewport = new Viewport(ViewportContext, TextMeasures);
        _historyActionContext.CodeTextBox = _codeTextBox;
        _historyActionContext.Viewport = Viewport;
        _historyActionContext.ViewportContext = ViewportContext;
    }

    private void SetCodeProvider(ICodeProvider codeProvider)
    {
        InputModel.SetCodeProvider(codeProvider);
        if (codeProvider is ITokenKindUpdatable tokenKindUpdatable)
        {
            var tokenKindUpdater = new TokenKindUpdater(Tokens);
            tokenKindUpdatable.TokenKindUpdated += (s, e) =>
            {
                tokenKindUpdater.UpdateTokenKinds(e.UpdatedTokenKinds);
                _codeTextBox?.InvalidateVisual();
            };
        }
    }

    public void SetText(string text)
    {
        InputModel.SetText(text);
        Viewport.UpdateScrollbarsMaximumValues(Text);
        RaiseTextChanged();
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorLeft()
    {
        MoveCursorLeftInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorRight()
    {
        MoveCursorRightInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorUp()
    {
        MoveCursorUpInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorDown()
    {
        MoveCursorDownInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorStartLine()
    {
        MoveCursorStartLineInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorEndLine()
    {
        MoveCursorEndLineInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorTextBegin()
    {
        MoveCursorTextBeginInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorTextEnd()
    {
        MoveCursorTextEndInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorPageUp()
    {
        MoveCursorPageUpInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveCursorPageDown()
    {
        MoveCursorPageDownInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveSelectedLinesUp()
    {
        History.AddAndDo(new MoveSelectedLinesUpHistoryAction(_historyActionContext));
    }

    public void MoveSelectedLinesDown()
    {
        History.AddAndDo(new MoveSelectedLinesDownHistoryAction(_historyActionContext));
    }

    public void GotoLine(int lineIndex)
    {
        GotoLineInputAction.Instance.Do(_historyActionContext, lineIndex);
        _codeTextBox?.InvalidateVisual();
        _codeTextBox?.Focus();
    }

    public void ScrollLineUp()
    {
        ScrollLineUpInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void ScrollLineDown()
    {
        ScrollLineDownInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveToPrevToken()
    {
        MoveToPrevTokenInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void MoveToNextToken()
    {
        MoveToNextTokenInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void DeleteLeftToken()
    {
        History.AddAndDo(new DeleteLeftTokenHistoryAction(_historyActionContext));
    }

    public void DeleteRightToken()
    {
        History.AddAndDo(new DeleteRightTokenHistoryAction(_historyActionContext));
    }

    public void SelectAll()
    {
        SelectAllInputAction.Instance.Do(_historyActionContext);
        _codeTextBox?.InvalidateVisual();
    }

    public void AppendChar(char ch)
    {
        History.AddAndDo(new AppendCharHistoryAction(_historyActionContext, ch));
    }

    public void AppendNewLine()
    {
        History.AddAndDo(new AppendNewLineHistoryAction(_historyActionContext));
    }

    public void InsertText(string insertedText)
    {
        History.AddAndDo(new InsertTextHistoryAction(_historyActionContext, insertedText));
    }

    public void DeleteSelectedLines()
    {
        History.AddAndDo(new DeleteSelectedLinesHistoryAction(_historyActionContext));
    }

    public void LeftDelete()
    {
        History.AddAndDo(new LeftDeleteHistoryAction(_historyActionContext));
    }

    public void RightDelete()
    {
        History.AddAndDo(new RightDeleteHistoryAction(_historyActionContext));
    }

    public void ToUpperCase()
    {
        History.AddAndDo(new ToUpperCaseHistoryAction(_historyActionContext));
    }

    public void ToLowerCase()
    {
        History.AddAndDo(new ToLowerCaseHistoryAction(_historyActionContext));
    }

    private void RaiseTextChanged() => TextChanged?.Invoke(this, EventArgs.Empty);
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
}
