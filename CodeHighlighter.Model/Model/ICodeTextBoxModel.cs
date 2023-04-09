using CodeHighlighter.Ancillary;
using CodeHighlighter.Common;
using CodeHighlighter.Core;

namespace CodeHighlighter.Model;

public interface ICodeTextBoxModel
{
    string Text { get; set; }
    public ITextLines TextLines { get; }
    CursorPosition CursorPosition { get; }
    Point AbsoluteCursorPosition { get; }
    ITextSelection TextSelection { get; }
    ITextMeasures TextMeasures { get; }
    ITextEvents TextEvents { get; }
    ITextMeasuresEvents TextMeasuresEvents { get; }
    ITokens Tokens { get; }
    ITokensColors TokensColors { get; }
    IHistory History { get; }
    ILinesDecorationCollection LinesDecoration { get; }
    ILineGapCollection Gaps { get; }
    ILineFolds Folds { get; }
    IViewport Viewport { get; }
    IBracketsHighlighter BracketsHighlighter { get; }
    ITextHighlighter TextHighlighter { get; }
    bool IsReadOnly { get; set; }
    ICodeTextBoxModelAdditionalInfo AdditionalInfo { get; }
    void AttachCodeTextBox(ICodeTextBox codeTextBox);
    string GetSelectedText();
    void MoveCursorTo(CursorPosition position);
    void MoveCursorLeft();
    void MoveCursorRight();
    void MoveCursorUp();
    void MoveCursorDown();
    void MoveCursorStartLine();
    void MoveCursorEndLine();
    void MoveCursorTextBegin();
    void MoveCursorTextEnd();
    void MoveCursorPageUp();
    void MoveCursorPageDown();
    void MoveSelectedLinesUp();
    void MoveSelectedLinesDown();
    void ActivateSelection();
    void CompleteSelection();
    void GotoLine(int lineIndex);
    void ScrollLineUp();
    void ScrollLineDown();
    void MoveToPrevToken();
    void MoveToNextToken();
    void SelectAll();
    void SelectToken(CursorPosition position);
    void DeleteLeftToken();
    void DeleteRightToken();
    void AppendChar(char ch);
    void AppendNewLine();
    void InsertText(string insertedText);
    void DeleteSelectedLines();
    void LeftDelete();
    void RightDelete();
    void SetTextCase(TextCase textCase);
    void Copy();
    void Paste();
    void Cut();
}
