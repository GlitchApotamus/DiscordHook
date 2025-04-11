using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordHook.Utils.Discord;
public class DiscordEmbed
{
    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("color")]
    public int? Color { get; set; }

    [JsonProperty("timestamp")]
    public string? Timestamp { get; set; }

    [JsonProperty("footer")]
    public EmbedFooter? Footer { get; set; }

    [JsonProperty("image")]
    public EmbedImage? Image { get; set; }

    [JsonProperty("thumbnail")]
    public EmbedThumbnail? Thumbnail { get; set; }

    [JsonProperty("author")]
    public EmbedAuthor? Author { get; set; }

    [JsonProperty("fields")]
    public List<EmbedField>? Fields { get; set; } = new();

    public DiscordEmbed() { }

    public DiscordEmbed(string title, string description, int color)
    {
        Title = title;
        Description = description;
        Color = color;
    }
}

public class EmbedFooter
{
    [JsonProperty("text")]
    public string? Text { get; set; }

    [JsonProperty("icon_url")]
    public string? IconUrl { get; set; }
}

public class EmbedImage
{
    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class EmbedThumbnail
{
    [JsonProperty("url")]
    public string? Url { get; set; }
}

public class EmbedAuthor
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("icon_url")]
    public string? IconUrl { get; set; }
}

public class EmbedField
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("inline")]
    public bool Inline { get; set; } = false;
}
