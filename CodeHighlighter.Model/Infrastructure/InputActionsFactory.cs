using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

public interface IInputActionsFactory
{
    TInputAction Get<TInputAction>();
}

internal class InputActionsFactory : IInputActionsFactory
{
    private readonly IResolvingProvider _provider;

    public InputActionsFactory(IResolvingProvider provider)
    {
        _provider = provider;
    }

    public TInputAction Get<TInputAction>()
    {
        return _provider.Resolve<TInputAction>();
    }
}
