using System.Linq;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class HistoryTest
{
    private Mock<IHistoryAction> _firstAction;
    private Mock<IHistoryAction> _secondAction;
    private History _history;

    [SetUp]
    public void Setup()
    {
        _firstAction = new Mock<IHistoryAction>();
        _secondAction = new Mock<IHistoryAction>();
        _firstAction.Setup(x => x.Do()).Returns(true);
        _secondAction.Setup(x => x.Do()).Returns(true);
        _history = new History();
    }

    [Test]
    public void Init()
    {
        Assert.AreEqual(0, _history._actions.Count);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void DoFalse_NotAdd()
    {
        _firstAction.Setup(x => x.Do()).Returns(false);
        _history.AddAndDo(_firstAction.Object);

        _firstAction.Verify(x => x.Do(), Times.Once());
        Assert.AreEqual(0, _history._actions.Count);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void AddFirst()
    {
        _history.AddAndDo(_firstAction.Object);

        _firstAction.Verify(x => x.Do(), Times.Once());
        Assert.AreEqual(1, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(0, _history._activeActionIndex);
    }

    [Test]
    public void Add_AllActive_AddNew()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.AddAndDo(_secondAction.Object);

        _firstAction.Verify(x => x.Do(), Times.Once());
        _secondAction.Verify(x => x.Do(), Times.Once());
        Assert.AreEqual(2, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(_secondAction.Object, _history._actions[1]);
        Assert.AreEqual(1, _history._activeActionIndex);
    }

    [Test]
    public void Add_LastUndo_RemoveLastAndAddNew()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Undo();
        _history.AddAndDo(_secondAction.Object);

        _firstAction.Verify(x => x.Undo(), Times.Once());
        Assert.AreEqual(1, _history._actions.Count);
        Assert.AreEqual(_secondAction.Object, _history._actions[0]);
        Assert.AreEqual(0, _history._activeActionIndex);
    }

    [Test]
    public void Add_LastUndo_AddDoFalse_NotRemoveLast()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Undo();
        _secondAction.Setup(x => x.Do()).Returns(false);
        _history.AddAndDo(_secondAction.Object);

        _secondAction.Verify(x => x.Do(), Times.Once());
        Assert.AreEqual(1, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void Redo_EmptyHistory_NoResult()
    {
        _history.Redo();

        Assert.AreEqual(0, _history._actions.Count);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void Redo_AllActive_NoResult()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Redo();

        _firstAction.Verify(x => x.Redo(), Times.Never());
        Assert.AreEqual(1, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(0, _history._activeActionIndex);
    }

    [Test]
    public void Redo_LastUndo_LastActive()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.AddAndDo(_secondAction.Object);
        _history.Undo();
        _history.Redo();

        _secondAction.Verify(x => x.Undo(), Times.Once());
        _secondAction.Verify(x => x.Redo(), Times.Once());
        Assert.AreEqual(2, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(_secondAction.Object, _history._actions[1]);
        Assert.AreEqual(1, _history._activeActionIndex);
    }

    [Test]
    public void Redo_AllUndo_AllRedo()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.AddAndDo(_secondAction.Object);
        _history.Undo();
        _history.Undo();
        _history.Redo();
        _history.Redo();

        Assert.AreEqual(2, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(_secondAction.Object, _history._actions[1]);
        Assert.AreEqual(1, _history._activeActionIndex);
    }

    [Test]
    public void Undo_EmptyHistory_NoResult()
    {
        _history.Undo();

        Assert.AreEqual(0, _history._actions.Count);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void Undo()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Undo();

        _firstAction.Verify(x => x.Undo(), Times.Once());
        Assert.AreEqual(1, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void Undo_AllUndo_NoResult()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.AddAndDo(_secondAction.Object);
        _history.Undo();
        _history.Undo();
        _history.Undo();

        _secondAction.Verify(x => x.Undo(), Times.Once());
        _firstAction.Verify(x => x.Undo(), Times.Once());
        Assert.AreEqual(2, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(_secondAction.Object, _history._actions[1]);
        Assert.AreEqual(-1, _history._activeActionIndex);
    }

    [Test]
    public void Undo_LastActive_LastUndo()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.AddAndDo(_secondAction.Object);
        _history.Undo();

        _secondAction.Verify(x => x.Undo(), Times.Once());
        Assert.AreEqual(2, _history._actions.Count);
        Assert.AreEqual(_firstAction.Object, _history._actions[0]);
        Assert.AreEqual(_secondAction.Object, _history._actions[1]);
        Assert.AreEqual(0, _history._activeActionIndex);
    }

    [Test]
    public void CanRedoUndo_Init_False()
    {
        Assert.False(_history.CanRedo);
        Assert.False(_history.CanUndo);
    }

    [Test]
    public void CanRedo_AddNew_False()
    {
        _history.AddAndDo(_firstAction.Object);

        Assert.False(_history.CanRedo);
    }

    [Test]
    public void CanRedo_Undo_True()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Undo();

        Assert.True(_history.CanRedo);
    }

    [Test]
    public void CanUndo_AddNew_True()
    {
        _history.AddAndDo(_firstAction.Object);

        Assert.True(_history.CanUndo);
    }

    [Test]
    public void CanUndo_LastUndo_False()
    {
        _history.AddAndDo(_firstAction.Object);
        _history.Undo();

        Assert.False(_history.CanUndo);
    }

    [Test]
    public void AddToLimit()
    {
        _history.AddAndDo(_firstAction.Object);
        for (int i = 1; i < History.ActionsLimit; i++)
        {
            _history.AddAndDo(_secondAction.Object);
        }
        _history.AddAndDo(_secondAction.Object);
        // _firstAction removed

        Assert.That(_history._actions.First(), Is.EqualTo(_secondAction.Object));
        Assert.That(_history._actions.Last(), Is.EqualTo(_secondAction.Object));
        _firstAction.Verify(x => x.Undo(), Times.Never());
    }

    [Test]
    public void AddToLimit_2()
    {
        for (int i = 0; i < History.ActionsLimit; i++)
        {
            _history.AddAndDo(_firstAction.Object);
        }

        for (int i = 0; i < History.ActionsLimit; i++)
        {
            _history.AddAndDo(_secondAction.Object);
        }

        Assert.That(_history._actions.First(), Is.EqualTo(_secondAction.Object));
        Assert.That(_history._actions.Last(), Is.EqualTo(_secondAction.Object));
        _firstAction.Verify(x => x.Undo(), Times.Never());
    }

    [Test]
    public void ActiveActionIndexWithLimit()
    {
        for (int i = 0; i < 2 * History.ActionsLimit; i++)
        {
            _history.AddAndDo(_firstAction.Object);
        }

        Assert.That(_history._activeActionIndex, Is.EqualTo(History.ActionsLimit - 1));
    }
}
