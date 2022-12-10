using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeHighlighter.Tests.InputActions;

internal class BaseInputActionIntegration
{
    protected InputModel _model;

    protected void Init()
    {
        _model = InputModel.MakeDefault();
        _model.SetCodeProvider(new SqlCodeProvider());
        _model.SetText("");
    }

    protected void SetText(string text)
    {
        _model.SetText(text);
    }

    protected void MoveCursorTo(CursorPosition position)
    {
        _model.MoveCursorTo(position);
    }

    protected void MoveCursorStartLine()
    {
        _model.MoveCursorStartLine();
    }

    protected void MoveCursorEndLine()
    {
        _model.MoveCursorEndLine();
    }

    protected void MoveCursorTextEnd()
    {
        _model.MoveCursorTextEnd();
    }

    protected void SelectAll()
    {
        _model.SelectAll();
    }

    protected void ActivateSelection()
    {
        _model.ActivateSelection();
    }

    protected void CompleteSelection()
    {
        _model.CompleteSelection();
    }

    protected void AppendNewLine()
    {
        _model.AppendNewLine();
    }
}
