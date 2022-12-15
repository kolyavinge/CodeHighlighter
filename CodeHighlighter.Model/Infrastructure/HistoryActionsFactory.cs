using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

public interface IHistoryActionsFactory
{
    TInputAction Get<TInputAction>(); // where;
}

internal class HistoryActionsFactory : IHistoryActionsFactory
{
    private readonly IResolvingProvider _provider;

    public HistoryActionsFactory(IResolvingProvider provider)
    {
        _provider = provider;
    }

    public TInputAction Get<TInputAction>()
    {
        return _provider.Resolve<TInputAction>();
    }
}
