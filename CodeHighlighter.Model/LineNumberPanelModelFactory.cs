using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter;

public static class LineNumberPanelModelFactory
{
    public static ILineNumberPanelModel MakeModel(ICodeTextBoxModel? codeTextBoxModel = null)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new LineNumberPanelModelInjectModule(codeTextBoxModel));

        var model = container.Resolve<ILineNumberPanelModel>();

        return model;
    }
}
