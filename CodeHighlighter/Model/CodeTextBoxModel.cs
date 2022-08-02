using System;
using CodeHighlighter.CodeProvidering;
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

    internal IViewportContext ViewportContext { get; set; }
    internal Viewport Viewport { get; private set; }
    internal TextSelection TextSelection { get; }
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

    public void MoveSelectedLinesUp() => MoveSelectedLinesUpInputAction.Instance.Do(_inputActionContext);

    public void MoveSelectedLinesDown() => MoveSelectedLinesDownInputAction.Instance.Do(_inputActionContext);

    public void GotoLine(int lineIndex) => GotoLineInputAction.Instance.Do(_inputActionContext, lineIndex);

    public void ScrollLineUp() => ScrollLineUpInputAction.Instance.Do(_inputActionContext);

    public void ScrollLineDown() => ScrollLineDownInputAction.Instance.Do(_inputActionContext);

    public void MoveToPrevToken() => MoveToPrevTokenInputAction.Instance.Do(_inputActionContext);

    public void MoveToNextToken() => MoveToNextTokenInputAction.Instance.Do(_inputActionContext);

    public void DeleteLeftToken() => DeleteLeftTokenInputAction.Instance.Do(_inputActionContext);

    public void DeleteRightToken() => DeleteRightTokenInputAction.Instance.Do(_inputActionContext);

    public void SelectAll() => SelectAllInputAction.Instance.Do(_inputActionContext);

    public void TextInput(string inputText) => TextInputInputAction.Instance.Do(_inputActionContext, inputText);

    public void ReplaceText(CursorPosition start, CursorPosition end, string insertedText)
        => ReplaceTextInputAction.Instance.Do(_inputActionContext, start, end, insertedText);

    public void NewLine() => NewLineInputAction.Instance.Do(_inputActionContext);

    public void InsertText(string insertedText) => InsertTextInputAction.Instance.Do(_inputActionContext, insertedText);

    public void DeleteSelectedLines() => DeleteSelectedLinesInputAction.Instance.Do(_inputActionContext);

    public void LeftDelete() => LeftDeleteInputAction.Instance.Do(_inputActionContext);

    public void RightDelete() => RightDeleteInputAction.Instance.Do(_inputActionContext);

    public void ToUpperCase() => ToUpperCaseInputAction.Instance.Do(_inputActionContext);

    public void ToLowerCase() => ToLowerCaseInputAction.Instance.Do(_inputActionContext);

    private void RaiseTextChanged() => TextChanged?.Invoke(this, EventArgs.Empty);
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
}
