using DependencyCord.Controllers;
using DependencyCord.Services.Cache;
using DependencyCord.Utils;
using DependencyCord.Types;
using Microsoft.Extensions.Logging;

namespace Test.Controllers;

[Controller]
public class Test : BaseController
{
    private GuildCache _guildCache;
    private ILogger<Test> _logger;

    public Test(GuildCache guildCache, ILogger<Test> logger)
    {
        _guildCache = guildCache;
        _logger = logger;
    }

    [Command("test")]
    public ControllerResult TestCmd([FromOption("smth")] IUser smth)
    {
        return new Embed()
            .SetTitle("I got an interaction!")
            .SetDescription(smth is null ? "You didn't select anything." :$"You selected user {smth.Username}");
    }
}
