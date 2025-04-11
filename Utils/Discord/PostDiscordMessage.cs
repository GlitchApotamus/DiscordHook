namespace DiscordHook.Utils.Discord;

internal sealed class DiscordMessage
{
    private static readonly DiscordHookConfig? BoundConfig = DiscordHook.BoundConfig;
    internal void PostDiscordMessage(string message, DiscordEmbed embed)
    {

        var username = BoundConfig?.WebhookUsername.Value!;
        var avatarUrl = BoundConfig?.WebhookAvatarUrl.Value!;
        var imageUrl = BoundConfig?.WebhookImageUrl.Value!;
        if (BoundConfig?.SendAsEmbed.Value == true && embed != null)
        {
            DiscordHook.Instance.StartCoroutine(new DiscordWebhook().SendWebhook(new DiscordWebhook.IDiscordWebhook
            {
                Username = username,
                AvatarUrl = avatarUrl,
                ImageUrl = imageUrl,
                Embed = embed
            }));
        }
        else
        {
            DiscordHook.Instance.StartCoroutine(new DiscordWebhook().SendWebhook(new DiscordWebhook.IDiscordWebhook
            {
                Username = username,
                AvatarUrl = avatarUrl,
                ImageUrl = imageUrl,
                Message = message
            }));
        }
    }
}
