using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;
using CodeHighlighter.Rendering;

namespace CodeHighlighter;

public static class RenderingModelFactory
{
    public static IRenderingModel MakeModel(ICodeTextBoxModel model, IRenderingContext renderingContext)
    {
        var container = new DependencyContainer();
        container.InitFromModules(new RenderingInjectModule());
        container.BindSingleton<ICodeTextBoxModel>(model);
        container.BindSingleton<ILineGapCollection>(model.Gaps);
        container.BindSingleton<IRenderingContext>(renderingContext);

        var renderingModel = container.Resolve<IRenderingModel>();

        return renderingModel;
    }
}
