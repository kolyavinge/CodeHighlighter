using System.Linq;
using System.Reflection;
using CodeHighlighter.InputActions;
using DependencyInjection;

namespace CodeHighlighter.Infrastructure;

internal class InputAndHistoryActionsInjectModule : InjectModule
{
    public override void Init(IBindingProvider bindingProvider)
    {
        var actionTypes = Assembly
            .GetAssembly(typeof(IInputActionsFactory))
            .GetTypes()
            .Where(type => type.IsClass && type.GetCustomAttribute<InputActionAttribute>() != null);

        foreach (var actionType in actionTypes)
        {
            bindingProvider.Bind(actionType.GetInterfaces().First(), actionType);
        }

        bindingProvider.Bind<IInputActionsFactory>().ToMethod(provider => new InputActionsFactory(provider)).ToSingleton();
    }
}
