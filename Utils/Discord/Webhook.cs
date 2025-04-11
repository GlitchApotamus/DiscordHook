using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

namespace DiscordHook.Utils.Discord;
public class DiscordWebhook : MonoBehaviour
{

    public IEnumerator SendWebhook(IDiscordWebhook webhook)
    {
        if (string.IsNullOrEmpty(webhook.Message) && webhook.Embed == null)
        {
            Debug.LogError("Error: Either a message or an embed must be provided.");
            yield break;
        }

        if (webhook.Embed != null)
        {
            webhook.Embed.Image ??= new EmbedImage();

            webhook.Embed.Image.Url = webhook.ImageUrl;
        }

        var payload = new
        {
            username = webhook.Username,
            avatar_url = webhook.AvatarUrl,
            content = webhook.Message,
            embeds = webhook.Embed != null ? new[] { webhook.Embed } : null
        };

        var jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);

        using UnityWebRequest request = new(DiscordHook.BoundConfig?.WebhookUrl.Value, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Discord Hook Error: {request.downloadHandler.text}");
        }
    }
    public class IDiscordWebhook
    {
        public string? Username { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = string.Empty;
        public DiscordEmbed? Embed { get; set; } = null!;
        public string? Message { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
    }
}
