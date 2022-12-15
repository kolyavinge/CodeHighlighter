using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class SetTextInputActionIntegration : BaseInputActionIntegration
{
    private SetTextInputAction _action;

    [SetUp]
    public void Setup()
    {
        Init();
        _action = new SetTextInputAction();
    }

    [Test]
    public void SetText_ResetCursor_1()
    {
        _action.Do(_context, "123");
        MoveCursorTextEnd();

        _action.Do(_context, "");

        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }

    [Test]
    public void SetText_ResetCursor_2()
    {
        _action.Do(_context, "123");
        MoveCursorTextEnd();

        _action.Do(_context, "1");

        Assert.AreEqual(new CursorPosition(0, 0), _textCursor.Position);
    }
}
