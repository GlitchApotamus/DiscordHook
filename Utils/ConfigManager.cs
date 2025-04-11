using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace DiscordHook.Utils;

class DiscordHookConfig
{
    public readonly ConfigEntry<bool> SendAsEmbed;
    public readonly ConfigEntry<string> WebhookUrl;
    public readonly ConfigEntry<string> WebhookUsername;
    public readonly ConfigEntry<string> WebhookAvatarUrl;
    public readonly ConfigEntry<string> WebhookImageUrl;
    public DiscordHookConfig(ConfigFile cfg)
    {
        cfg.SaveOnConfigSet = false;

        SendAsEmbed = cfg.Bind(
            "Settings",
            "SendAsEmbed",
            false,
            new ConfigDescription("Whether to send this message as an embed.")
        );

        WebhookUrl = cfg.Bind(
            "Settings",
            "WebhookUrl",
            string.Empty,
            new ConfigDescription("Where to send content to in discord. Should look like this: https://discord.com/api/webhooks/1349793699168518255/WLPhkQcHHH0DoFfVpmL-feS4TsfnfgiChEMJMWOXnVnfPtJgN8-mq7-dvx")
        );

        WebhookUsername = cfg.Bind(
            "Settings",
            "WebhookUsername",
            "Webhook Bot",
            new ConfigDescription("The username that will be displayed when the webhook sends a message.")
        );

        WebhookAvatarUrl = cfg.Bind(
            "Settings",
            "WebhookAvatarUrl",
            "https://example.com/avatar.png",
            new ConfigDescription("The avatar URL that will be displayed when the webhook sends a message. Don't want an image? Just leave it blank and discord will give a random default.")
        );

        WebhookImageUrl = cfg.Bind(
            "Settings",
            "WebhookImageUrl",
            "https://shared.cloudflare.steamstatic.com/store_item_assets/steam/apps/3241660/extras/00_title_repo.png?t=1740578354",
            new ConfigDescription("The image URL that will be displayed when the webhook sends a message. Don't want an image? Just leave it blank.")
        );
        ClearOrphanedEntries(cfg);
        cfg.Save();
        cfg.SaveOnConfigSet = true;
    }

    static void ClearOrphanedEntries(ConfigFile cfg)
    {
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
        orphanedEntries.Clear();
    }
}