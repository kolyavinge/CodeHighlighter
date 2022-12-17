using System.Linq;
using static CodeHighlighter.Model.IText;

namespace CodeHighlighter.Model;

internal abstract class EditTextResult
{
    public readonly CursorPosition OldCursorPosition;
    public readonly CursorPosition NewCursorPosition;
    public readonly CursorPosition SelectionStart;
    public readonly CursorPosition SelectionEnd;
    public readonly string DeletedSelectedText;
    public bool IsSelectionExist => !SelectionStart.Equals(SelectionEnd);

    protected EditTextResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText)
    {
        OldCursorPosition = oldCursorPosition;
        NewCursorPosition = newCursorPosition;
        SelectionStart = selectionStart;
        SelectionEnd = selectionEnd;
        DeletedSelectedText = deletedSelectedText;
    }
}

internal class SetTextResult : EditTextResult
{
    public readonly string Text;

    public SetTextResult(CursorPosition oldCursorPosition, string deletedSelectedText, string text)
        : base(oldCursorPosition, new(), new(), new(), deletedSelectedText)
    {
        Text = text;
    }
}

internal class AppendCharResult : EditTextResult
{
    public readonly char AppendedChar;

    public AppendCharResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText, char appendedChar)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        AppendedChar = appendedChar;
    }
}

internal class InsertTextResult : EditTextResult
{
    public readonly CursorPosition InsertStartPosition;
    public readonly CursorPosition InsertEndPosition;
    public readonly string InsertedText;

    public InsertTextResult(
        CursorPosition oldCursorPosition,
        CursorPosition newCursorPosition,
        CursorPosition selectionStart,
        CursorPosition selectionEnd,
        string deletedSelectedText,
        CursorPosition insertStartPosition,
        CursorPosition insertEndPosition,
        string insertedText,
        bool hasInserted)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        InsertStartPosition = insertStartPosition;
        InsertEndPosition = insertEndPosition;
        InsertedText = insertedText;
        HasInserted = hasInserted;
    }

    public bool HasInserted { get; }
}

internal class DeleteResult : EditTextResult
{
    public readonly CharDeleteResult CharCharDeleteResult;
    public bool HasDeleted => DeletedSelectedText.Any() || CharCharDeleteResult.HasDeleted;

    public DeleteResult(
        CursorPosition oldCursorPosition,
        CursorPosition newCursorPosition,
        CursorPosition selectionStart,
        CursorPosition selectionEnd,
        string deletedSelectedText,
        CharDeleteResult charCharDeleteResult)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        CharCharDeleteResult = charCharDeleteResult;
    }
}

internal class AppendNewLineResult : EditTextResult
{
    public AppendNewLineResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
    }
}

internal class DeleteTokenResult : EditTextResult
{
    public readonly bool HasDeleted;

    public DeleteTokenResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText, bool hasDeleted)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        HasDeleted = hasDeleted;
    }
}

internal class MoveSelectedLinesResult : EditTextResult
{
    public bool HasMoved => !OldCursorPosition.Equals(NewCursorPosition);

    public MoveSelectedLinesResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, "")
    {
    }
}

internal class DeleteSelectedLinesResult : EditTextResult
{
    public bool HasDeleted => DeletedSelectedText.Any();

    public DeleteSelectedLinesResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
    }
}

internal class CaseResult : EditTextResult
{
    public readonly string ChangedText;
    public bool HasChanged => !String.IsNullOrWhiteSpace(ChangedText);

    public CaseResult(
        CursorPosition cursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText, string changedText)
       : base(cursorPosition, cursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        ChangedText = changedText;
    }
}
