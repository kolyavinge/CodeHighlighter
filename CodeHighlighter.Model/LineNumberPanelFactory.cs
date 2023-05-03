using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class LineNumberPanelFactory
{
    public static ILineNumberPanel MakeModel(ICodeTextBox? codeTextBoxModel = null)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new LineNumberPanelModelInjectModule(codeTextBoxModel));

        var model = container.Resolve<ILineNumberPanel>();

        return model;
    }
}
