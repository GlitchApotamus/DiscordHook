using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class DiscordWebhook : MonoBehaviour
{

    public IEnumerator SendWebhook(string username, string avatar_url, string banner_url, DiscordEmbed? embed, string? message = null)
    {
        if (string.IsNullOrEmpty(message) && embed == null)
        {
            Debug.LogError("Error: Either a message or an embed must be provided.");
            yield break;
        }

        if (embed != null)
        {
            embed.Image ??= new EmbedImage();

            embed.Image.Url = banner_url;
        }

        var payload = new
        {
            username,
            avatar_url,
            content = message,
            embeds = embed != null ? new[] { embed } : null
        };

        var jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(DiscordHook.DiscordHook.BoundConfig?.WebhookUrl.Value, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Discord Hook Error: {request.downloadHandler.text}");
            }
        }
    }

}
