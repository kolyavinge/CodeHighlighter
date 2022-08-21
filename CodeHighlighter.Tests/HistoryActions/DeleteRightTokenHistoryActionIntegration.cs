﻿using CodeHighlighter.HistoryActions;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class DeleteRightTokenHistoryActionIntegration : BaseHistoryActionIntegration
{
    private DeleteRightTokenHistoryAction _action;

    [SetUp]
    public void Setup()
    {
        MakeContext();
        _context.InputModel.SetText("text\r\n123");
        _action = new DeleteRightTokenHistoryAction(_context);
    }

    [Test]
    public void NoDelete()
    {
        _context.InputModel.MoveCursorTo(new(1, 3));
        Assert.False(_action.Do());
        InvalidateVisualCallNever();
    }

    [Test]
    public void NoSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 0));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("\r\n123", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 0));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("\r\n123", _text.ToString());

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void NoSelection_DeleteReturn()
    {
        _context.InputModel.MoveCursorTo(new(0, 4));
        MakeInactiveSelection();

        _action.Do();
        Assert.AreEqual("text123", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(0, 4));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("text123", _text.ToString());

        InvalidateVisualCallThreeTimes();
    }

    [Test]
    public void WithSelection()
    {
        _context.InputModel.MoveCursorTo(new(0, 3));
        _context.InputModel.ActivateSelection();
        _context.InputModel.MoveCursorTo(new(1, 1));
        _context.InputModel.CompleteSelection();

        _action.Do();
        Assert.AreEqual("tex23", _text.ToString());

        MakeUncompleteSelection();
        _action.Undo();
        Assert.AreEqual("text\r\n123", _text.ToString());
        AssertCursorPosition(new(1, 1));

        MakeUncompleteSelection();
        _action.Redo();
        Assert.AreEqual("tex23", _text.ToString());

        InvalidateVisualCallThreeTimes();
    }
}