using System.Reflection;
using System.Net.Http.Json;
using DependencyCord.Types;
using DependencyCord.Internal;
using DependencyCord.Services.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyCord.Controllers;

internal record class ParameterInfo(string Name, ApplicationCommandOptionType Type, bool CanBeNull = false);
internal record class CommandInfo(Type ClassT, MethodInfo Method, List<ParameterInfo> Parameters, bool IsAsync = false);

internal class ControllerManager
{
    private IServiceProvider _services;
    private Assembly _asm;
    private Dictionary<string, CommandInfo> _commandInfos = new Dictionary<string, CommandInfo>();

    public ControllerManager(Assembly asm, IServiceProvider services)
    {
        _asm = asm;
        _services = services;
    }

    public void RegisterControllers()
    {
        var types = _asm.GetTypes();
        var orderedTypes = types
            .Where(t => t.IsClass)
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsSubclassOf(typeof(BaseController)));

        foreach (var type in orderedTypes)
        {
            if (type.GetCustomAttribute<ControllerAttribute>() is null) continue;
            foreach (var method in type.GetMethods())
            {
                var cnAttr = method.GetCustomAttribute<CommandAttribute>();
                if (cnAttr is null) continue;
                if (method.IsStatic) continue;
                bool isAsync = (method.ReturnType == typeof(Task<ControllerResult>));

                var parameters = new List<ParameterInfo>();
                foreach (var parameter in method.GetParameters())
                {
                    var foAttr = parameter.GetCustomAttribute<FromOptionAttribute>();
                    if (foAttr is null)
                        throw new Exception($"Parameter {parameter.Name} in method {method.Name} doesn't have FromOptionAttribute");
                    bool canBeNull = !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
                    if (parameter.ParameterType == typeof(IUser))
                    {
                        parameters.Add(new ParameterInfo(foAttr.OptionName,
                            ApplicationCommandOptionType.User, canBeNull));
                    }
                    else if (parameter.ParameterType == typeof(string))
                    {
                        parameters.Add(new ParameterInfo(foAttr.OptionName,
                            ApplicationCommandOptionType.String, canBeNull));
                    }
                    else if (parameter.ParameterType == typeof(int))
                    {
                        parameters.Add(new ParameterInfo(foAttr.OptionName,
                            ApplicationCommandOptionType.Integer, canBeNull));
                    }
                    else
                    {
                        throw new Exception("Parameter {parameter.Name} in method {method.Name} doesn't support any of the types needed.");
                    }
                }
                _commandInfos.Add(cnAttr.CommandName, new CommandInfo(type, method,
                    parameters, isAsync));
            }
        }
    }


    public async Task<bool> ExecuteCommand(InteractionObject interaction)
    {
        if (_commandInfos.TryGetValue(interaction.Data.Name, out var ci))
        {
            using (var scope = _services.CreateScope())
            {
                object?[]? objects;
                if (ci.Parameters.Count != 0)
                {
                    objects = new object?[ci.Parameters.Count];
                    for (int i = 0; i < ci.Parameters.Count; i++)
                    {
                        var parameter = ci.Parameters[i];
                        var value = interaction.Data.Options?
                            .Where(o => o.Type == parameter.Type).FirstOrDefault(o => o.Name == parameter.Name);
                        if (value is null)
                            if (parameter.CanBeNull)
                            {
                                objects[i] = null;
                            }
                            else
                                throw new NotImplementedException();
                        else if (value is not null)
                        {
                            switch (parameter.Type)
                            {
                                case ApplicationCommandOptionType.String:
                                    objects[i] = value.Value!.Value.GetString();
                                    break;
                                case ApplicationCommandOptionType.Integer:
                                    objects[i] = value.Value!.Value.GetInt32();
                                    break;
                                case ApplicationCommandOptionType.User:
                                    var id = value.Value!.Value.GetString();
                                    var user = interaction.Data.Resolved?.Users!.GetValueOrDefault(id);
                                    objects[i] = user;
                                    break;
                            }
                        }
                    }
                }
                else objects = null;
                var obj = scope.ServiceProvider.GetRequiredService(ci.ClassT) as BaseController;
                obj!.Data = interaction;
                IControllerResult result;
                if (ci.IsAsync)
                {
                    result = (await (ci.Method.Invoke(obj, objects) as Task<IControllerResult>)!);
                }
                else result = (ci.Method.Invoke(obj, objects) as IControllerResult)!;

                var client = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Discord");
                var interactionResponse = new InteractionResponseObject();
                interactionResponse.Type = InteractionResponseCallbackType.ChannelMessageWithSource;
                interactionResponse.Data = new InteractionResponseData();
                interactionResponse.Data.Content = result.Message;
                interactionResponse.Data.Embeds = result.Embeds;

                await client.PostAsJsonAsync($"interactions/{interaction.Id}/{interaction.Token}/callback",
                interactionResponse, JsonOptions.Op);
            }
            return true;
        }
        else return false;
    }
}


public abstract class BaseController
{
    /// <summary>
    /// Raw data provided by discord
    /// </summary>
    public InteractionObject Data { get; set; } = null!;
}

public interface IControllerResult
{
    public string? Message { get; set; }
    public List<Embed>? Embeds { get; set; }
}

public class ControllerResult : IControllerResult
{
    public static implicit operator ControllerResult(string message)
    {
        return new ControllerResult
        {
            Message = message
        };
    }

    public static implicit operator ControllerResult(Embed embed)
    {
        var result = new ControllerResult();
        result.Embeds = new List<Embed>();
        result.Embeds.Add(embed);
        return result;
    }

    public string? Message { get; set; }
    public List<Embed>? Embeds { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class CommandAttribute : Attribute
{
    public string CommandName { get; set; }

    public CommandAttribute(string name)
    {
        CommandName = name;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ControllerAttribute : Attribute
{
    public string? CommandGroupName { get; set; }

    public ControllerAttribute(string name)
    {
        CommandGroupName = name;
    }

    public ControllerAttribute()
    {

    }
}

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class FromOptionAttribute : Attribute
{
    public string OptionName { get; set; }

    public FromOptionAttribute(string name)
    {
        OptionName = name;
    }
}