using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class DependencyContainer
{
    private readonly IDependencyContainer _container;

    public DependencyContainer()
    {
        _container = DependencyContainerFactory.MakeLiteContainer();
        _container.InitFromModules(new CommonInjectModule(), new InputAndHistoryActionsInjectModule());
    }

    public void BindSingleton<TDependency>(object obj)
    {
        _container.Bind<TDependency>().ToMethod(_ => obj).ToSingleton();
    }

    public TDependency Resolve<TDependency>()
    {
        return _container.Resolve<TDependency>();
    }
}
