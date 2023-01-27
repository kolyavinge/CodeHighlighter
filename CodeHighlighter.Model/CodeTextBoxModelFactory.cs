using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Controllers;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class CodeTextBoxModelFactory
{
    public static ICodeTextBoxModel MakeModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        var container = new DependencyContainer();
        container.BindSingleton<ICodeProvider>(codeProvider);
        container.BindSingleton<CodeTextBoxModelAdditionalParams>(additionalParams ?? new());

        var model = container.Resolve<ICodeTextBoxModel>();

        return model;
    }

    public static IKeyboardController MakeKeyboardController(ICodeTextBoxModel model)
    {
        return new KeyboardController(model);
    }

    public static IMouseController MakeMouseController(ICodeTextBox codeTextBox, ICodeTextBoxModel model)
    {
        return new MouseController(codeTextBox, model, new PointInTextSelection(model.TextSelection));
    }
}
