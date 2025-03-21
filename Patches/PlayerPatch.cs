using System;
using HarmonyLib;
using UnityEngine;

public static class PlayerPatchTypesHolder
{
    public static Type[] PlayerPatches = new Type[]
    {
        typeof(PlayerAvatar_PlayerDeath_Patch)
    };
}

[HarmonyPatch(typeof(PlayerAvatar))]
[HarmonyPatch("PlayerDeath")]
public static class PlayerAvatar_PlayerDeath_Patch
{
    static void Prefix(PlayerAvatar __instance)
    {
        Debug.Log($"Player {__instance.name} is about to die.");
    }

    static void Postfix(PlayerAvatar __instance)
    {
        Debug.Log($"Player {__instance.name} has died.");
    }
}
