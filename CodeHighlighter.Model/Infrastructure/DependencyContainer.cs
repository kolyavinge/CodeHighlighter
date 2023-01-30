using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class DependencyContainer
{
    private readonly IDependencyContainer _container;

    public DependencyContainer()
    {
        _container = DependencyContainerFactory.MakeLiteContainer();
    }

    public void InitFromModules(params InjectModule[] modules)
    {
        _container.InitFromModules(modules);
    }

    public void BindSingleton<TDependency, TImplementation>()
    {
        _container.Bind<TDependency, TImplementation>().ToSingleton();
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
