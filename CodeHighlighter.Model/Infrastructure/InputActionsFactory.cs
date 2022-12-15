using CodeHighlighter.InputActions;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

public interface IInputActionsFactory
{
    TInputAction Get<TInputAction>();//where TInputAction : InputAction;
}

internal class InputActionsFactory : IInputActionsFactory
{
    private readonly IResolvingProvider _provider;

    public InputActionsFactory(IResolvingProvider provider)
    {
        _provider = provider;
    }

    public TInputAction Get<TInputAction>() //where TInputAction : InputAction
    {
        return _provider.Resolve<TInputAction>();
    }
}
