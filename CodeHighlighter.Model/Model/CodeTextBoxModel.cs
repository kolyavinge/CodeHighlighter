﻿using CodeHighlighter.CodeProvidering;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.InputActions;

namespace CodeHighlighter.Model;

public class CodeTextBoxModel
{
    private readonly InputActionContext _context;
    private ICodeTextBox _codeTextBox;

    public event EventHandler? TextSet;
    public event EventHandler? TextChanged;

    public Text Text { get; }
    public Tokens Tokens { get; }
    public TokensColors TokensColors { get; }
    public TextCursor TextCursor { get; }
    public TextMeasures TextMeasures { get; }
    public TextSelection TextSelection { get; }
    internal TextSelector TextSelector { get; }
    public History History { get; }
    public LinesDecorationCollection LinesDecoration { get; }
    public bool IsReadOnly { get; set; }

    public Viewport Viewport { get; private set; }
    public BracketsHighlighter BracketsHighlighter { get; }

    public CodeTextBoxModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        _codeTextBox = DummyCodeTextBox.Instance;
        Text = new Text();
        TextCursor = new TextCursor(Text);
        Tokens = new Tokens();
        TokensColors = new TokensColors();
        TextMeasures = new TextMeasures();
        History = new History();
        LinesDecoration = new LinesDecorationCollection();
        TextSelection = new TextSelection();
        TextSelector = new TextSelector(Text, TextCursor, TextSelection);
        Viewport = new Viewport(Text, new DummyViewportContext(), TextMeasures);
        BracketsHighlighter = new BracketsHighlighter(additionalParams?.HighlighteredBrackets ?? "");
        IsReadOnly = additionalParams?.IsReadOnly ?? false;
        _context = new InputActionContext(
            codeProvider,
            Text,
            TextCursor,
            TextMeasures,
            TextSelection,
            TextSelector,
            Tokens,
            TokensColors,
            Viewport,
            () => TextChanged?.Invoke(this, EventArgs.Empty),
            () => TextSet?.Invoke(this, EventArgs.Empty));
        SetCodeProvider(codeProvider);
    }

    public CodeTextBoxModel() : this(new EmptyCodeProvider()) { }

    public void AttachCodeTextBox(ICodeTextBox codeTextBox)
    {
        _codeTextBox = codeTextBox;
        Viewport = new Viewport(Text, codeTextBox, TextMeasures);
        _context.CodeTextBox = _codeTextBox;
        _context.Viewport = Viewport;
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

    public void SetText(string text)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new SetTextHistoryAction(_context, text));
    }

    public string GetSelectedText()
    {
        return TextSelector.GetSelectedText();
    }

    public void MoveCursorTo(CursorPosition position)
    {
        MoveCursorToInputAction.Instance.Do(_context, position);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorLeft()
    {
        MoveCursorLeftInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorRight()
    {
        MoveCursorRightInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorUp()
    {
        MoveCursorUpInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorDown()
    {
        MoveCursorDownInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorStartLine()
    {
        MoveCursorStartLineInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorEndLine()
    {
        MoveCursorEndLineInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorTextBegin()
    {
        MoveCursorTextBeginInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorTextEnd()
    {
        MoveCursorTextEndInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorPageUp()
    {
        MoveCursorPageUpInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveCursorPageDown()
    {
        MoveCursorPageDownInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveSelectedLinesUp()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new MoveSelectedLinesUpHistoryAction(_context));
    }

    public void MoveSelectedLinesDown()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new MoveSelectedLinesDownHistoryAction(_context));
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
        GotoLineInputAction.Instance.Do(_context, lineIndex);
        _codeTextBox.InvalidateVisual();
        _codeTextBox.Focus();
    }

    public void ScrollLineUp()
    {
        ScrollLineUpInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void ScrollLineDown()
    {
        ScrollLineDownInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveToPrevToken()
    {
        MoveToPrevTokenInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void MoveToNextToken()
    {
        MoveToNextTokenInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void SelectAll()
    {
        SelectAllInputAction.Instance.Do(_context);
        _codeTextBox.InvalidateVisual();
    }

    public void SelectToken(CursorPosition position)
    {
        SelectTokenInputAction.Instance.Do(_context, position);
        _codeTextBox.InvalidateVisual();
    }

    public void DeleteLeftToken()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new DeleteLeftTokenHistoryAction(_context));
    }

    public void DeleteRightToken()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new DeleteRightTokenHistoryAction(_context));
    }

    public void AppendChar(char ch)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new AppendCharHistoryAction(_context, ch));
    }

    public void AppendNewLine()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new AppendNewLineHistoryAction(_context));
    }

    public void InsertText(string insertedText)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new InsertTextHistoryAction(_context, insertedText));
    }

    public void DeleteSelectedLines()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new DeleteSelectedLinesHistoryAction(_context));
    }

    public void LeftDelete()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new LeftDeleteHistoryAction(_context));
    }

    public void RightDelete()
    {
        if (IsReadOnly) return;
        History.AddAndDo(new RightDeleteHistoryAction(_context));
    }

    public void SetTextCase(TextCase textCase)
    {
        if (IsReadOnly) return;
        History.AddAndDo(new SetTextCaseHistoryAction(_context, textCase));
    }
}

public class CodeTextBoxModelAdditionalParams
{
    public string? HighlighteredBrackets { get; set; }
    public bool IsReadOnly { get; set; }
}