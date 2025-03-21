using System;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;

namespace DiscordHook
{
    public static class LobbyPatchTypesHolder
    {
        public static Type[] LobbyPatches = new Type[]
        {
            typeof(OnLobbyLocked),
            typeof(OnLobbyJoined),
            typeof(OnLobbyLeft),
            typeof(OnLobbyUnlocked),
            typeof(OnDestroy),
            typeof(OnLeaveLobby),
            typeof(OnJoinLobby),
            typeof(OnHostLobby)
        };
    }

    public static class SharedData
    {
        public static int currentLevelNumber = RunManager.instance.levelsCompleted;
        public static Lobby lobby = SteamManager.instance.currentLobby;
        public static Friend owner = lobby.Owner;
        public static bool lobbyClosed = false;
        public static string GetLobbyLink()
        {
            var userId = SteamClient.SteamId;
            var gameId = SteamClient.AppId;
            return $"https://linkoid.github.io/steamuri/joinlobby/{gameId}/{lobby.Id}/{userId}";

        }

    }

    [HarmonyPatch(typeof(SteamManager), "LockLobby")]
    public class OnLobbyLocked
    {
        private static void Postfix()
        {
            DiscordHook.Logger.LogInfo("Lobby is now locked");
            DiscordHook.Instance.PostDiscordMessage(message: $"The lobby is now locked.", embed: new DiscordEmbed
            {
                Description = "The lobby is now locked.",
                Color = 16711680,
            });
        }
    }

    [HarmonyPatch(typeof(SteamManager), "UnlockLobby")]
    public class OnLobbyUnlocked
    {
        private static void Postfix()
        {
            DiscordHook.Instance.PostDiscordMessage(message: $"[{SharedData.owner.Name}'s Lobby]({SharedData.GetLobbyLink()}) is now open. We will be moving to level number {SharedData.currentLevelNumber + 1}!",
                embed: new DiscordEmbed
                {
                    Description = $"[{SharedData.owner.Name}'s Lobby]({SharedData.GetLobbyLink()}) is now open. We will be moving to level number {SharedData.currentLevelNumber + 1}!",
                    Color = 65280,
                });
        }
    }

    [HarmonyPatch(typeof(SteamManager), "OnLobbyMemberJoined")]
    public class OnLobbyJoined
    {
        private static void Postfix(Lobby _lobby, Friend _friend)
        {
            DiscordHook.Instance.PostDiscordMessage(message: $"{_friend.Name} has joined their lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}", embed: new DiscordEmbed
            {
                Description = $"{_friend.Name} has joined their lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}",
                Color = 65280,
            });
        }
    }

    [HarmonyPatch(typeof(SteamManager), "OnLobbyMemberLeft")]
    public class OnLobbyLeft
    {
        private static void Postfix(Lobby _lobby, Friend _friend)
        {
            DiscordHook.Instance.PostDiscordMessage(message: $"{_friend.Name} has left {SharedData.owner.Name}'s Lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}", embed: new DiscordEmbed
            {
                Description = $"{_friend.Name} has left {SharedData.owner.Name}'s Lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}",
                Color = 16711680,
            });

        }
    }

    [HarmonyPatch(typeof(SteamManager), "OnDestroy")]
    public class OnDestroy
    {
        private static void Postfix()
        {
            if (SharedData.currentLevelNumber == 0) return;
            if (!SharedData.lobbyClosed)
            {
                DiscordHook.Instance.PostDiscordMessage(message: "The Lobby has taken a wild turn into CrashMania!", embed: new DiscordEmbed
                {
                    Description = "The Lobby has taken a wild turn into CrashMania!",
                    Color = 16711680,
                });
            }
        }
    }


    [HarmonyPatch(typeof(SteamManager), "LeaveLobby")]
    public class OnLeaveLobby
    {
        private static void Postfix()
        {
            if (SteamClient.Name == SharedData.owner.Name) 
            {
                DiscordHook.Instance.PostDiscordMessage(message: $"{SteamClient.Name} has closed their lobby.", embed: new DiscordEmbed
                {
                    Description = $"{SteamClient.Name} has closed their lobby.",
                    Color = 16711680,
                });
                SharedData.lobbyClosed = true;
                return;
            }
            DiscordHook.Instance.PostDiscordMessage(message: $"{SteamClient.Name} has left {SharedData.owner.Name}'s Lobby.", embed: new DiscordEmbed
            {
                Description = $"{SteamClient.Name} has left {SharedData.owner.Name}'s Lobby.",
                Color = 16711680,
            });
        }
    }

    [HarmonyPatch(typeof(SteamManager), "OnLobbyEntered")]
    public class OnJoinLobby
    {
        private static void Postfix()
        {
            DiscordHook.Instance.PostDiscordMessage(message: $"{SteamClient.Name} has joined their lobby.", embed: new DiscordEmbed
            {
                Description = $"{SteamClient.Name} has joined their lobby.",
                Color = 65280,
            });
        }
    }

    [HarmonyPatch(typeof(SteamManager), "HostLobby")]
    public class OnHostLobby
    {
        private static void Postfix()
        {
            DiscordHook.Instance.PostDiscordMessage(message: $"{SteamClient.Name} is now hosting a lobby, please stand by for the invite link....", embed: new DiscordEmbed
            {
                Description = $"{SteamClient.Name} is now hosting a lobby, please stand by for the invite link....",
                Color = 65280,
            });
        }
    }
}