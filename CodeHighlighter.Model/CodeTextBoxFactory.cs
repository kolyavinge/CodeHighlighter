using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class CodeTextBoxFactory
{
    public static ICodeTextBox MakeModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new CodeTextBoxInjectModule());
        container.BindSingleton<ICodeProvider>(codeProvider);
        container.BindSingleton<CodeTextBoxModelAdditionalParams>(additionalParams ?? new());

        var model = container.Resolve<ICodeTextBox>();

        return model;
    }
}
