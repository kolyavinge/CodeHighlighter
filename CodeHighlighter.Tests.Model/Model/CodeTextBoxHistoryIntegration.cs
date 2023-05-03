using CodeHighlighter.Model;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class CodeTextBoxHistoryIntegration : BaseCodeTextBoxIntegration
{
    private CodeTextBoxModel _model;

    [SetUp]
    public void Setup()
    {
        _model = MakeModel();
        _model.Text = "";
    }

    [Test]
    public void History()
    {
        _model.AppendChar('1');
        _model.AppendChar('2');
        _model.AppendChar('3');
        Assert.That(_model.Text, Is.EqualTo("123"));

        _model.History.Undo();
        Assert.That(_model.Text, Is.EqualTo("12"));

        _model.History.Undo();
        Assert.That(_model.Text, Is.EqualTo("1"));

        _model.History.Undo();
        Assert.That(_model.Text, Is.EqualTo(""));
    }
}
