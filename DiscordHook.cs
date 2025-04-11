using BepInEx;
using BepInEx.Logging;
using DiscordHook.Utils;
using HarmonyLib;
using System;
using UnityEngine;

namespace DiscordHook;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
internal sealed class DiscordHook : BaseUnityPlugin
{
    internal static DiscordHook Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }
    internal static DiscordHookConfig? BoundConfig { get; private set; }
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
        Patch();
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");

    }

    internal void Patch()
    {
        if (Harmony == null)
        {
            Harmony val = new Harmony(MyPluginInfo.PLUGIN_GUID);
            Harmony val2 = val;
            Harmony = val;
        }
        Harmony.PatchAll();
    }
    internal void Unpatch()
    {
        Harmony? harmony = Harmony;
        if (harmony != null)
        {
            harmony.UnpatchSelf();
        }
    }
    private void Update()
    {

    }
    private void OnDestroy()
    {
        Unpatch();
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} has been unloaded!");
    }

    
}



