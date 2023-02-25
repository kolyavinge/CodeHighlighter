﻿using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TextEventsTest
{
    private Text _text;
    private TextEvents _events;
    private int _linesCount;

    [SetUp]
    public void Setup()
    {
        _text = new Text();
        _events = new TextEvents(_text, new TextChangedEventArgsFactory(new TextLinesChangingLogic()));
        _events.LinesCountChanged += (s, e) => _linesCount = e.LinesCount;
        _linesCount = _text.LinesCount;
    }

    [Test]
    public void SetText()
    {
        _text = new Text();
        _text.TextContent = "123\n123";
        _events = new TextEvents(_text, new TextChangedEventArgsFactory(new TextLinesChangingLogic()));
        _events.LinesCountChanged += (s, e) => _linesCount = e.LinesCount;
        _events.RaiseTextSet();
        Assert.AreEqual(2, _linesCount);

        _text.TextContent = "";
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void SetText_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _events.RaiseTextChanged();
        Assert.AreEqual(2, _linesCount);

        _text.TextContent = "123";
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void NewLine_RaiseTextChanged()
    {
        _text.AppendNewLine(new(0, 0));
        _events.RaiseTextChanged();
        Assert.AreEqual(2, _linesCount);

        _text.AppendNewLine(new(0, 0));
        _events.RaiseTextChanged();
        Assert.AreEqual(3, _linesCount);
    }

    [Test]
    public void AppendChar_RaiseTextChanged()
    {
        _text.AppendChar(new(0, 0), '1');
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void Insert_TwoLines_RaiseTextChanged()
    {
        _text.Insert(new(0, 0), new Text("123"));
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);

        _text.Insert(new(0, 0), new Text("\n123\n123"));
        _events.RaiseTextChanged();
        Assert.AreEqual(3, _linesCount);
    }

    [Test]
    public void LeftDelete_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _text.LeftDelete(new(1, 0));
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void RightDelete_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _text.RightDelete(new(0, 3));
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void DeleteSelection_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _text.DeleteSelection(new TextSelection(_text).Set(new(0, 0), new(1, 3)).GetSelectedLines());
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void DeleteLine_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";

        _text.DeleteLine(0);
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);

        _text.DeleteLine(0);
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void DeleteLines_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _text.DeleteLines(0, 1);
        _events.RaiseTextChanged();
        Assert.AreEqual(1, _linesCount);
    }

    [Test]
    public void ReplaceLines_RaiseTextChanged()
    {
        _text.TextContent = "123\n123";
        _text.ReplaceLines(0, 1);
        _events.RaiseTextChanged();
        Assert.AreEqual(2, _linesCount);
    }
}
