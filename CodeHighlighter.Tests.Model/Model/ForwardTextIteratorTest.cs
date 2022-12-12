using System;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

class ForwardTextIteratorTest
{
    private ForwardTextIterator _iterator;

    [Test]
    public void NegativeStartLineIndex_Error()
    {
        try
        {
            new ForwardTextIterator(new Text(), -1, 10);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("startLineIndex", e.Message);
        }
    }

    [Test]
    public void NegativeEndLineIndex_Error()
    {
        try
        {
            new ForwardTextIterator(new Text(), 0, -1);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("endLineIndex", e.Message);
        }
    }

    [Test]
    public void NegativeStartLineIndexGreaterThenEndLineIndex_Error()
    {
        try
        {
            new ForwardTextIterator(new Text(), 10, 2);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual("endLineIndex must be greater then startLineIndex", e.Message);
        }
    }

    [Test]
    public void Empty()
    {
        SetText("");

        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(0, _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(0, _iterator.ColumnIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void Return()
    {
        SetText("\n");

        Assert.AreEqual('\n', _iterator.Char);
        Assert.AreEqual(0, _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(0, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(0, _iterator.NextChar);
        Assert.AreEqual(1, _iterator.LineIndex);
        Assert.AreEqual(0, _iterator.ColumnIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void OneLine()
    {
        SetText("123");

        Assert.AreEqual('1', _iterator.Char);
        Assert.AreEqual('2', _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(0, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);
        Assert.AreEqual('3', _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(1, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);
        Assert.AreEqual(0, _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(2, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(3, _iterator.ColumnIndex);
        Assert.AreEqual(0, _iterator.Char);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void OneLineAndReturn()
    {
        SetText("123\n");

        Assert.AreEqual('1', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);
        Assert.AreEqual('\n', _iterator.NextChar);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);
        Assert.AreEqual(0, _iterator.NextChar);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(3, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void TwoLines()
    {
        SetText("123\n456");

        Assert.AreEqual('1', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);
        Assert.AreEqual('\n', _iterator.NextChar);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);
        Assert.AreEqual('4', _iterator.NextChar);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('4', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('5', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('6', _iterator.Char);
        Assert.AreEqual(1, _iterator.LineIndex);
        Assert.AreEqual(2, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(1, _iterator.LineIndex);
        Assert.AreEqual(3, _iterator.ColumnIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void TwoLinesAndReturn()
    {
        SetText("123\n456\n");

        Assert.AreEqual('1', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('4', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('5', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('6', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);
        Assert.AreEqual(1, _iterator.LineIndex);
        Assert.AreEqual(3, _iterator.ColumnIndex);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(2, _iterator.LineIndex);
        Assert.AreEqual(0, _iterator.ColumnIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void OnlyFirstLine()
    {
        SetText("123\n456", 0, 0);

        Assert.AreEqual('1', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);
        Assert.False(_iterator.Eof);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(0, _iterator.LineIndex);
        Assert.AreEqual(3, _iterator.ColumnIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void OnlyFirstTwoLines()
    {
        SetText("1\n2\n3", 0, 1);

        Assert.AreEqual('1', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual('2', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(1, _iterator.LineIndex);
        Assert.True(_iterator.Eof);
    }

    [Test]
    public void OnlyLastTwoLines()
    {
        SetText("1\n2\n3", 1, 2);

        Assert.AreEqual('2', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual('\n', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual('3', _iterator.Char);

        _iterator.MoveNext();
        Assert.AreEqual(0, _iterator.Char);
        Assert.AreEqual(2, _iterator.LineIndex);
        Assert.True(_iterator.Eof);
    }

    private void SetText(string textString)
    {
        var text = new Text();
        text.TextContent = textString;
        _iterator = new ForwardTextIterator(text, 0, text.LinesCount - 1);
    }

    private void SetText(string textString, int startLineIndex, int endLineIndex)
    {
        var text = new Text();
        text.TextContent = textString;
        _iterator = new ForwardTextIterator(text, startLineIndex, endLineIndex);
    }
}
