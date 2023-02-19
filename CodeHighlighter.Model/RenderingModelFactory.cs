using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public static class RenderingModelFactory
{
    public static ICodeTextBoxRenderingModel MakeModel(ICodeTextBoxModel model, ICodeTextBoxRenderingContext renderingContext)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new RenderingInjectModule());
        container.BindSingleton<ICodeTextBoxModel>(model);
        container.BindSingleton<ILineGapCollection>(model.Gaps);
        container.BindSingleton<ICodeTextBoxRenderingContext>(renderingContext);

        var renderingModel = container.Resolve<ICodeTextBoxRenderingModel>();

        return renderingModel;
    }
}
