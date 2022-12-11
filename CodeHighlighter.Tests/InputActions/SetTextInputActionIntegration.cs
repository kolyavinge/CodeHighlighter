using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.InputActions;

internal class SetTextInputActionIntegration : BaseInputActionIntegration
{
    [SetUp]
    public void Setup()
    {
        Init();
    }

    [Test]
    public void SetText_ResetCursor_1()
    {
        SetTextInputAction.Instance.Do(_context, "123");
        MoveCursorTextEnd();

        SetTextInputAction.Instance.Do(_context, "");

        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }

    [Test]
    public void SetText_ResetCursor_2()
    {
        SetTextInputAction.Instance.Do(_context, "123");
        MoveCursorTextEnd();

        SetTextInputAction.Instance.Do(_context, "1");

        Assert.AreEqual(new CursorPosition(0, 0), _model.TextCursor.Position);
    }
}
