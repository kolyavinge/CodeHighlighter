using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private readonly InputActionContext _inputActionContext;
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
        _inputActionContext = new InputActionContext(InputModel, Text, TextCursor, TextMeasures, TextSelection, Viewport, ViewportContext, RaiseTextChanged);
        SetCodeProvider(codeProvider);
    }

    internal void Init(ICodeTextBox? codeTextBox, IViewportContext viewportContext)
    {
        _codeTextBox = codeTextBox;
        ViewportContext = viewportContext;
        Viewport = new Viewport(ViewportContext, TextMeasures);
        _inputActionContext.CodeTextBox = _codeTextBox;
        _inputActionContext.Viewport = Viewport;
        _inputActionContext.ViewportContext = ViewportContext;
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

    public void MoveCursorLeft() => MoveCursorLeftInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorRight() => MoveCursorRightInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorUp() => MoveCursorUpInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorDown() => MoveCursorDownInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorStartLine() => MoveCursorStartLineInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorEndLine() => MoveCursorEndLineInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorTextBegin() => MoveCursorTextBeginInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorTextEnd() => MoveCursorTextEndInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorPageUp() => MoveCursorPageUpInputAction.Instance.Do(_inputActionContext);

    public void MoveCursorPageDown() => MoveCursorPageDownInputAction.Instance.Do(_inputActionContext);

    public void MoveSelectedLinesUp() => History.AddAndDo(new MoveSelectedLinesUpHistoryAction(_inputActionContext));

    public void MoveSelectedLinesDown() => History.AddAndDo(new MoveSelectedLinesDownHistoryAction(_inputActionContext));

    public void GotoLine(int lineIndex) => GotoLineInputAction.Instance.Do(_inputActionContext, lineIndex);

    public void ScrollLineUp() => ScrollLineUpInputAction.Instance.Do(_inputActionContext);

    public void ScrollLineDown() => ScrollLineDownInputAction.Instance.Do(_inputActionContext);

    public void MoveToPrevToken() => MoveToPrevTokenInputAction.Instance.Do(_inputActionContext);

    public void MoveToNextToken() => MoveToNextTokenInputAction.Instance.Do(_inputActionContext);

    public void DeleteLeftToken() => History.AddAndDo(new DeleteLeftTokenHistoryAction(_inputActionContext));

    public void DeleteRightToken() => History.AddAndDo(new DeleteRightTokenHistoryAction(_inputActionContext));

    public void SelectAll() => SelectAllInputAction.Instance.Do(_inputActionContext);

    public void AppendChar(char ch) => History.AddAndDo(new AppendCharHistoryAction(_inputActionContext, ch));

    public void AppendNewLine() => History.AddAndDo(new AppendNewLineHistoryAction(_inputActionContext));

    public void InsertText(string insertedText) => History.AddAndDo(new InsertTextHistoryAction(_inputActionContext, insertedText));

    public void DeleteSelectedLines() => History.AddAndDo(new DeleteSelectedLinesHistoryAction(_inputActionContext));

    public void LeftDelete() => History.AddAndDo(new LeftDeleteHistoryAction(_inputActionContext));

    public void RightDelete() => History.AddAndDo(new RightDeleteHistoryAction(_inputActionContext));

    public void ToUpperCase() => History.AddAndDo(new ToUpperCaseHistoryAction(_inputActionContext));

    public void ToLowerCase() => History.AddAndDo(new ToLowerCaseHistoryAction(_inputActionContext));

    private void RaiseTextChanged() => TextChanged?.Invoke(this, EventArgs.Empty);
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
}
