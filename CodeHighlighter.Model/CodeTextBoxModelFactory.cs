using CodeHighlighter.CodeProvidering;
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
}
