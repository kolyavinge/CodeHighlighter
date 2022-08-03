using System;

namespace CodeHighlighter.Model;

internal abstract class EditTextResult
{
    public readonly CursorPosition OldCursorPosition;
    public readonly CursorPosition NewCursorPosition;
    public readonly CursorPosition SelectionStart;
    public readonly CursorPosition SelectionEnd;
    public readonly string DeletedSelectedText;

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
    public readonly string InsertedText;

    public InsertTextResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText, string insertedText)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        InsertedText = insertedText;
    }
}

internal class DeleteResult : EditTextResult
{
    public readonly Text.CharDeleteResult CharCharDeleteResult;
    public bool NoDeletion => String.IsNullOrWhiteSpace(DeletedSelectedText) && CharCharDeleteResult.NoDeletion;

    public DeleteResult(
        CursorPosition oldCursorPosition,
        CursorPosition newCursorPosition,
        CursorPosition selectionStart,
        CursorPosition selectionEnd,
        string deletedSelectedText,
        Text.CharDeleteResult charCharDeleteResult)
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
    public readonly bool NoDelection;

    public DeleteTokenResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText, bool noDelection)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
        NoDelection = noDelection;
    }
}

internal class MoveSelectedLinesResult : EditTextResult
{
    public bool NoMoving => OldCursorPosition.Equals(NewCursorPosition);

    public MoveSelectedLinesResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, "")
    {
    }
}

internal class DeleteSelectedLinesResult : EditTextResult
{
    public bool NoDeletion => String.IsNullOrWhiteSpace(DeletedSelectedText);

    public DeleteSelectedLinesResult(
        CursorPosition oldCursorPosition, CursorPosition newCursorPosition, CursorPosition selectionStart, CursorPosition selectionEnd, string deletedSelectedText)
        : base(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText)
    {
    }
}
