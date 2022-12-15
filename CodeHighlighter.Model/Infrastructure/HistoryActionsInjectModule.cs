using System.Linq;
using System.Reflection;
using CodeHighlighter.Model;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class HistoryActionsInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        var actionTypes = Assembly
            .GetAssembly(typeof(IHistoryActionsFactory))
            .GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttribute<HistoryActionAttribute>() != null);

        foreach (var actionType in actionTypes)
        {
            bindingProvider.Bind(actionType.GetInterfaces().Last(), actionType).ToSingleton();
        }

        bindingProvider.Bind<IHistoryActionsFactory>().ToMethod(provider => new HistoryActionsFactory(provider)).ToSingleton();
    }
}
