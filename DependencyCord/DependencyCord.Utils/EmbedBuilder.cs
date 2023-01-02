using DependencyCord.Types;

namespace DependencyCord.Utils;

public static class EmbedBuilder
{
    public static Embed SetTitle(this Embed embed, string title)
    {
        embed.Title = title;
        return embed;
    }
    public static Embed SetDescription(this Embed embed, string description)
    {
        embed.Description = description;
        return embed;
    }

    public static Embed AddField(this Embed embed, string key, string value, bool inline = false)
    {
        embed.Fields?.Add(new EmbedField { Name = key, Value = value, Inline = inline });
        return embed;
    }
}