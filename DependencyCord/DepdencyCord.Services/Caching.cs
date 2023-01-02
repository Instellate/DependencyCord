using DependencyCord.Types;
using DependencyCord.Internal;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DependencyCord.Services.Cache;

public class GuildCache
{
    private DCConfigs _config;
    private HttpClient _client;
    internal Dictionary<string, IGuild> _guilds = new Dictionary<string, IGuild>();

    internal GuildCache(DCConfigs config, IHttpClientFactory factory)
    {
        this._config = config;
        this._client = factory.CreateClient("Discord");
    }


    public async Task<IGuild> FetchGuild(string id)
    {
        if (_guilds.TryGetValue(id, out var dicGuild))
        {
            return dicGuild;
        }
        else
        {
            var request = await _client.GetAsync($"guilds/{id}");
            if (!request.IsSuccessStatusCode)
            {
                if (request.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new NotFoundException($"Couldn't find guild {id}.", NotFoundType.Guild);
                }
                throw new NotImplementedException($"Failed to fetch guild {id} with status code {request.StatusCode}");
            }
            else
            {
                IGuild guild = (await request.Content.ReadFromJsonAsync<JsonGuild>())!;
                _guilds.Add(id, guild);
                return guild;
            }
        }
    }
}

