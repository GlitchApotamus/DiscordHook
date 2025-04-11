using HarmonyLib;
using UnityEngine;

namespace DiscordHook.Patches;

[HarmonyPatch(typeof(PlayerAvatar))]
public static class PlayerAvatarPatches
{
    private static PlayerAvatar Player { get; set; } = null!;
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void Awake_Postfix() => Player = PlayerAvatar.instance;

    [HarmonyPatch("PlayerDeathRPC")]
    [HarmonyPrefix]
    static void PlayerDeath_Prefix(PlayerAvatar __instance) => Debug.Log($"Player {__instance.playerName} is about to die.");
    
    [HarmonyPatch("PlayerDeathRPC")]
    [HarmonyPostfix]
    static void PlayerDeath_Postfix(PlayerAvatar __instance) => Debug.Log($"Player {__instance.playerName} has died.");

    [HarmonyPatch("ReviveRPC")]
    [HarmonyPrefix]
    static void PlayerRevive_Prefix(PlayerAvatar __instance) => Debug.Log($"Player {__instance.playerName} is about to be revived.");
    
    [HarmonyPatch("ReviveRPC")]
    [HarmonyPostfix]
    static void PlayerRevive_Postfix(PlayerAvatar __instance) => Debug.Log($"Player {__instance.playerName} has been revived.");

    // [HarmonyPatch("Update")]
    // [HarmonyPrefix]
    // static void Update_Prefix() => Debug.Log($"Update method is about to be called for {Player.playerName}.");
    
    // [HarmonyPatch("Update")]
    // [HarmonyPostfix]
    // static void Update_Postfix() => Debug.Log($"Update method has been called for {Player.playerName}.");
}


