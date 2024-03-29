﻿using CodeHighlighter.Core;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Core;

internal class EditTextResultToLinesChangeConverterTest
{
    private Mock<ITextLinesChangingLogic> _logic;
    private LinesChangeResult _linesChangeResult;
    private EditTextResultToLinesChangeConverter _converter;
    private LinesChangeResult _result;

    [SetUp]
    public void Setup()
    {
        _linesChangeResult = new LinesChangeResult(new(1, 2), new(3, 4));
        _logic = new Mock<ITextLinesChangingLogic>();
        _converter = new EditTextResultToLinesChangeConverter(_logic.Object);
    }

    [Test]
    public void MakeForAppendNewLine()
    {
        _logic.Setup(x => x.AppendNewLine(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForAppendNewLine(new(new(1, 0), default, new(2, 0), new(3, 0), ""));
        _logic.Verify(x => x.AppendNewLine(new(1, 0), new(2, 0), new(3, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForInsertText()
    {
        _logic.Setup(x => x.InsertText(new(1, 0), new(2, 0), new(3, 0), new(4, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForInsertText(new(default, default, new(3, 0), new(4, 0), "", new(1, 0), new(2, 0), "", true));
        _logic.Verify(x => x.InsertText(new(1, 0), new(2, 0), new(3, 0), new(4, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForInsertText_NoInsertion()
    {
        _logic.Setup(x => x.InsertText(new(1, 0), new(2, 0), new(3, 0), new(4, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForInsertText(new(default, default, new(3, 0), new(4, 0), "", new(1, 0), new(2, 0), "", false));
        _logic.Verify(x => x.InsertText(new(1, 0), new(2, 0), new(3, 0), new(4, 0)), Times.Never());
        AssertDefaultResult();
    }

    [Test]
    public void MakeForLeftDelete()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForLeftDelete(new(default, new(1, 0), new(2, 0), new(3, 0), "", new() { IsLineDeleted = true }));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForLeftDelete_NoDeletion()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForLeftDelete(new(new(1, 0), default, new(2, 0), new(3, 0), "", new() { IsLineDeleted = false }));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Never());
        AssertDefaultResult();
    }

    [Test]
    public void MakeForRightDelete()
    {
        _logic.Setup(x => x.RightDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForRightDelete(new(new(1, 0), default, new(2, 0), new(3, 0), "", new() { IsLineDeleted = true }));
        _logic.Verify(x => x.RightDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForRightDelete_NoDeletion()
    {
        _logic.Setup(x => x.RightDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForRightDelete(new(new(1, 0), default, new(2, 0), new(3, 0), "", new() { IsLineDeleted = false }));
        _logic.Verify(x => x.RightDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Never());
        AssertDefaultResult();
    }

    [Test]
    public void MakeForDeleteToken()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForDeleteToken(new(new(1, 0), default, new(2, 0), new(3, 0), "", true));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForDeleteToken_NoDeletion()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForDeleteToken(new(new(1, 0), default, new(2, 0), new(3, 0), "", false));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(3, 0)), Times.Never());
        AssertDefaultResult();
    }

    [Test]
    public void MakeForDeleteSelectedLines()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(4, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForDeleteSelectedLines(new(new(1, 0), default, new(2, 0), new(3, 0), "123"));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(4, 0)), Times.Once());
        AssertResult();
    }

    [Test]
    public void MakeForDeleteSelectedLines_NoDeletion()
    {
        _logic.Setup(x => x.LeftDelete(new(1, 0), new(2, 0), new(4, 0))).Returns(_linesChangeResult);
        _result = _converter.MakeForDeleteSelectedLines(new(new(1, 0), default, new(2, 2), new(3, 3), ""));
        _logic.Verify(x => x.LeftDelete(new(1, 0), new(2, 0), new(4, 0)), Times.Never());
        AssertDefaultResult();
    }

    private void AssertResult()
    {
        Assert.That(_result.AddedLines, Is.EqualTo(new LinesChange(1, 2)));
        Assert.That(_result.DeletedLines, Is.EqualTo(new LinesChange(3, 4)));
    }

    private void AssertDefaultResult()
    {
        Assert.That(_result, Is.EqualTo(new LinesChangeResult()));
    }
}
