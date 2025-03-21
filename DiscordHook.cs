using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
using System;
using System.Linq;
using Steamworks;

namespace DiscordHook;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
internal sealed class DiscordHook : BaseUnityPlugin
{
    internal static DiscordHook Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static DiscordHookConfig? BoundConfig { get; private set; }

    private static Type[] CombinedPatchTypes => 
    LobbyPatchTypesHolder.LobbyPatches.Concat(PlayerPatchTypesHolder.PlayerPatches).ToArray();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        BoundConfig = new DiscordHookConfig(base.Config);
        var webhookUrl = BoundConfig.WebhookUrl.Value;
        if (webhookUrl == string.Empty || !Uri.TryCreate(webhookUrl, UriKind.Absolute, out var uriResult) || uriResult.Scheme != Uri.UriSchemeHttps || !webhookUrl.Contains("discord.com/api/webhooks/"))
        {
            Logger.LogError($"Please set a valid Webhook URL in the config file.{Environment.NewLine}The plugin will not load until a valid Webhook URL is set.{Environment.NewLine}You can find the config file in the BepInEx/config folder.{Environment.NewLine}Disabling the plugin!");
            return;
        }

        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;


        foreach (var patchType in CombinedPatchTypes)
        {
            try
            {
                harmony.PatchAll(patchType);
            }
            catch (Exception)
            {
                Logger.LogError($"Failed to patch {patchType.Name}: {Environment.NewLine}{Environment.StackTrace}");
                return;
            }

        }

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");

        PostDiscordMessage(message: $"{SteamClient.Name} has started playing!", embed: new DiscordEmbed { Title = "Plugin Loaded", Description = $"{SteamClient.Name} has started playing!" });
    }

    internal void PostDiscordMessage(string message, DiscordEmbed embed)
    {
        var username = BoundConfig?.WebhookUsername.Value!;
        var avatarUrl = BoundConfig?.WebhookAvatarUrl.Value!;
        var bannerUrl = BoundConfig?.WebhookBannerUrl.Value!;
        if (BoundConfig?.SendAsEmbed.Value == true && embed != null)
        {
            StartCoroutine(new DiscordWebhook().SendWebhook(username, avatarUrl, bannerUrl, embed: embed));
        }
        else
        {
            StartCoroutine(new DiscordWebhook().SendWebhook(username, avatarUrl, bannerUrl, embed: null, message: message));
        }
    }
}


