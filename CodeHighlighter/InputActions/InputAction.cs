﻿using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputAction
{
    protected string GetSelectedText(InputActionContext context)
    {
        if (!context.TextSelection.IsExist) return "";
        var selectedLines = new List<string>();
        foreach (var line in context.TextSelection.GetSelectedLines(context.Text))
        {
            selectedLines.Add(context.Text.GetLine(line.LineIndex).GetSubstring(line.LeftColumnIndex, line.RightColumnIndex - line.LeftColumnIndex));
        }

        return String.Join(Environment.NewLine, selectedLines);
    }

    protected void SetSelection(InputActionContext context)
    {
        if (context.TextSelection.InProgress)
        {
            context.TextSelection.EndPosition = context.TextCursor.Position;
        }
        else
        {
            context.TextSelection.StartPosition = context.TextCursor.Position;
            context.TextSelection.EndPosition = context.TextCursor.Position;
        }
    }

    protected void DeleteSelection(InputActionContext context)
    {
        var deleteResult = context.Text.DeleteSelection(context.TextSelection);
        context.Tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var (startCursorPosition, _) = context.TextSelection.GetSortedPositions();
        context.TextCursor.MoveTo(startCursorPosition);
        context.TextSelection.Reset();
    }

    protected void SetTokens(InputActionContext context)
    {
        var codeProviderTokens = context.CodeProvider.GetTokens(new ForwardTextIterator(context.Text, 0, context.Text.LinesCount - 1)).ToList();
        context.Tokens.SetTokens(codeProviderTokens, 0, context.Text.LinesCount);
        context.TokenColors.SetColors(context.CodeProvider.GetColors());
    }

    protected void UpdateTokensForLines(InputActionContext context, int startLineIndex, int count)
    {
        var codeProviderTokens = context.CodeProvider.GetTokens(new ForwardTextIterator(context.Text, startLineIndex, startLineIndex + count - 1)).ToList();
        context.Tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}