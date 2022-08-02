using System;
using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering;

namespace CodeHighlighter.Model;

internal class InputModel
{
    private ICodeProvider _codeProvider;

    public Text Text { get; }
    public TextCursor TextCursor { get; }
    public TextSelection TextSelection { get; }
    public Tokens Tokens { get; }
    public TokensColors TokenColors { get; }

    public InputModel(Text text, TextCursor textCursor, TextSelection textSelection, Tokens tokens)
    {
        Text = text;
        TextCursor = textCursor;
        TextSelection = textSelection;
        Tokens = tokens;
        TokenColors = new();
        _codeProvider = new EmptyCodeProvider();
    }

    internal InputModel()
    {
        Text = new();
        TextCursor = new(Text);
        TextSelection = new(0, 0, 0, 0);
        Tokens = new();
        TokenColors = new();
        _codeProvider = new EmptyCodeProvider();
    }

    public void SetCodeProvider(ICodeProvider codeProvider)
    {
        _codeProvider = codeProvider;
        SetTokens();
    }

    public void SetText(string text)
    {
        Text.TextContent = text;
        SetTokens();
    }

    public void MoveCursorTo(CursorPosition position)
    {
        TextCursor.MoveTo(position);
        SetSelection();
    }

    public void MoveCursorUp()
    {
        TextCursor.MoveUp();
        SetSelection();
    }

    public void MoveCursorDown()
    {
        TextCursor.MoveDown();
        SetSelection();
    }

    public void MoveCursorLeft()
    {
        TextCursor.MoveLeft();
        SetSelection();
    }

    public void MoveCursorRight()
    {
        TextCursor.MoveRight();
        SetSelection();
    }

    public void MoveCursorStartLine()
    {
        TextCursor.MoveStartLine();
        SetSelection();
    }

    public void MoveCursorTextBegin()
    {
        TextCursor.MoveTextBegin();
        SetSelection();
    }

    public void MoveCursorEndLine()
    {
        TextCursor.MoveEndLine();
        SetSelection();
    }

    public void MoveCursorTextEnd()
    {
        TextCursor.MoveTextEnd();
        SetSelection();
    }

    public void MoveCursorPageUp(int pageSize)
    {
        TextCursor.MovePageUp(pageSize);
        SetSelection();
    }

    public void MoveCursorPageDown(int pageSize)
    {
        TextCursor.MovePageDown(pageSize);
        SetSelection();
    }

    public void SelectAll()
    {
        TextSelection.InProgress = false;
        TextSelection.StartCursorLineIndex = 0;
        TextSelection.StartCursorColumnIndex = 0;
        TextSelection.EndCursorLineIndex = Text.LinesCount - 1;
        TextSelection.EndCursorColumnIndex = Text.GetLine(TextSelection.EndCursorLineIndex).Length;
        TextCursor.MoveTextEnd();
    }

    public void ActivateSelection()
    {
        if (!TextSelection.InProgress)
        {
            TextSelection.InProgress = true;
            TextSelection.StartCursorLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
    }

    public void CompleteSelection()
    {
        TextSelection.InProgress = false;
    }

    private void SetSelection()
    {
        if (TextSelection.InProgress)
        {
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
        else
        {
            TextSelection.StartCursorLineIndex = TextCursor.LineIndex;
            TextSelection.StartCursorColumnIndex = TextCursor.ColumnIndex;
            TextSelection.EndCursorLineIndex = TextCursor.LineIndex;
            TextSelection.EndCursorColumnIndex = TextCursor.ColumnIndex;
        }
    }

    public void SelectToken(CursorPosition position)
    {
        var selector = new TokenSelector();
        var range = selector.GetSelection(Tokens, position);
        TextSelection.Reset();
        TextSelection.StartCursorLineIndex = position.LineIndex;
        TextSelection.StartCursorColumnIndex = range.StartCursorColumnIndex;
        TextSelection.EndCursorLineIndex = position.LineIndex;
        TextSelection.EndCursorColumnIndex = range.EndCursorColumnIndex;
    }

    public void MoveToNextToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos);
        SetSelection();
    }

    public void MoveToPrevToken()
    {
        var navigator = new TokenNavigator();
        var pos = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
        TextCursor.MoveTo(pos);
        SetSelection();
    }

    public void DeleteLeftToken()
    {
        if (TextSelection.IsExist)
        {
            LeftDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveLeft(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void DeleteRightToken()
    {
        if (TextSelection.IsExist)
        {
            RightDelete();
        }
        else
        {
            var navigator = new TokenNavigator();
            var pos = navigator.MoveRight(Text, Tokens, TextCursor.LineIndex, TextCursor.ColumnIndex);
            ActivateSelection();
            MoveCursorTo(pos);
            CompleteSelection();
            LeftDelete();
        }
    }

    public void NewLine()
    {
        if (TextSelection.IsExist) DeleteSelection();
        Text.NewLine(TextCursor.Position);
        Tokens.InsertEmptyLine(TextCursor.LineIndex + 1);
        TextCursor.MoveDown();
        TextCursor.MoveStartLine();
        UpdateTokensForLines(TextCursor.LineIndex - 1, 2);
    }

    public void AppendChar(char ch)
    {
        if (TextSelection.IsExist) DeleteSelection();
        Text.AppendChar(TextCursor.Position, ch);
        TextCursor.MoveRight();
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    public string GetSelectedText()
    {
        if (!TextSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in TextSelection.GetSelectedLines(Text))
        {
            selectedLines.Add(Text.GetLine(line.LineIndex).GetSubstring(line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }

    public void InsertText(string text)
    {
        var insertedText = new Text(text);
        if (TextSelection.IsExist) DeleteSelection();
        Text.Insert(TextCursor.Position, insertedText);
        if (insertedText.LinesCount == 1)
        {
            TextCursor.MoveTo(new(TextCursor.LineIndex, TextCursor.ColumnIndex + insertedText.Lines.Last().Length));
        }
        else
        {
            TextCursor.MoveTo(new(TextCursor.LineIndex + insertedText.LinesCount - 1, insertedText.Lines.Last().Length));
        }
        UpdateTokensForLines(0, Text.LinesCount);
    }

    public void LeftDelete()
    {
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            var newPosition = Text.GetCursorPositionAfterLeftDelete(TextCursor.Position);
            var deleteResult = Text.LeftDelete(TextCursor.Position);
            if (deleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex);
            }
            TextCursor.MoveTo(newPosition);
        }
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    public void RightDelete()
    {
        if (TextSelection.IsExist)
        {
            DeleteSelection();
        }
        else
        {
            var deleteResult = Text.RightDelete(TextCursor.Position);
            if (deleteResult.IsLineDeleted)
            {
                Tokens.DeleteLine(TextCursor.LineIndex + 1);
            }
        }
        UpdateTokensForLines(TextCursor.LineIndex, 1);
    }

    private void DeleteSelection()
    {
        var deleteResult = Text.DeleteSelection(TextSelection);
        Tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var startCursorPosition = TextSelection.GetSortedPositions().Item1;
        TextCursor.MoveTo(startCursorPosition);
        TextSelection.Reset();
    }

    public void DeleteSelectedLines()
    {
        CursorPosition start, end;
        if (TextSelection.IsExist)
        {
            (start, end) = TextSelection.GetSortedPositions();
            TextSelection.Reset();
        }
        else
        {
            start = end = new CursorPosition(TextCursor.LineIndex, TextCursor.ColumnIndex);
        }
        for (int i = start.LineIndex; i <= end.LineIndex; i++)
        {
            if (start.LineIndex < Text.LinesCount - 1)
            {
                Text.DeleteLine(start.LineIndex);
                Tokens.DeleteLine(start.LineIndex);
            }
            else
            {
                Text.GetLine(start.LineIndex).Clear();
                Tokens.GetTokens(start.LineIndex).Clear();
            }
        }
        TextCursor.MoveTo(start);
    }

    public void MoveSelectedLinesUp()
    {
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            (var start, var end) = TextSelection.GetSortedPositions();
            sourceIndex = start.LineIndex - 1;
            if (sourceIndex < 0) return;
            destinationIndex = end.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex - 1;
            if (destinationIndex < 0) return;
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveUp();
        TextSelection.StartCursorLineIndex--;
        TextSelection.EndCursorLineIndex--;
    }

    public void MoveSelectedLinesDown()
    {
        int sourceIndex, destinationIndex;
        if (TextSelection.IsExist)
        {
            (var start, var end) = TextSelection.GetSortedPositions();
            sourceIndex = end.LineIndex + 1;
            if (sourceIndex >= Text.LinesCount) return;
            destinationIndex = start.LineIndex;
        }
        else
        {
            sourceIndex = TextCursor.LineIndex;
            destinationIndex = sourceIndex + 1;
            if (destinationIndex >= Text.LinesCount) return;
        }
        Text.ReplaceLines(sourceIndex, destinationIndex);
        Tokens.ReplaceLines(sourceIndex, destinationIndex);
        TextCursor.MoveDown();
        TextSelection.StartCursorLineIndex++;
        TextSelection.EndCursorLineIndex++;
    }

    public void SetSelectedTextCase(TextCase textCase)
    {
        Text.SetSelectedTextCase(TextSelection, textCase);
        var (start, end) = TextSelection.GetSortedPositions();
        UpdateTokensForLines(start.LineIndex, end.LineIndex - start.LineIndex + 1);
    }

    private void SetTokens()
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text)).ToList();
        Tokens.SetTokens(codeProviderTokens, 0, Text.LinesCount);
        TokenColors.SetColors(_codeProvider.GetColors());
    }

    private void UpdateTokensForLines(int startLineIndex, int count)
    {
        var codeProviderTokens = _codeProvider.GetTokens(new ForwardTextIterator(Text, startLineIndex, startLineIndex + count - 1)).ToList();
        Tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}
