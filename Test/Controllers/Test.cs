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
    public async Task<ControllerResult> TestCmd()
    {
        return new Embed()
            .SetTitle("Hello, world!")
            .SetDescription("And fuck you init!");
    }
}
