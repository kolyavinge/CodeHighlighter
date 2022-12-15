using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Infrastructure;

namespace CodeHighlighter.Model;

public static class CodeTextBoxModelFactory
{
    public static CodeTextBoxModel MakeModel(ICodeProvider codeProvider, CodeTextBoxModelAdditionalParams? additionalParams = null)
    {
        var container = new DependencyContainer();
        container.BindSingleton<ICodeProvider>(codeProvider);
        container.BindSingleton<CodeTextBoxModelAdditionalParams>(additionalParams ?? new());

        var model = container.Resolve<CodeTextBoxModel>();

        return model;
    }
}
