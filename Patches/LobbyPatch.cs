using DiscordHook.Utils.Discord;
using HarmonyLib;
using Steamworks;
using Steamworks.Data;

namespace DiscordHook.Patches;

[HarmonyPatch(typeof(SteamManager))]
public class SteamManagerPatches
{
    private static Lobby currentLobby;
    private static bool lobbyClosed = false;
    private static bool awakeCalled = false;
    private static string GetLobbyLink(SteamId lobbyId, SteamId ownerId)
    {
        var gameId = SteamClient.AppId;
        return $"https://steamuri.com/joinlobby/{gameId}/{lobbyId}/{ownerId}";
    }


    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    public static void OnAwake()
    {
        currentLobby = SteamManager.instance.currentLobby;
        if (!awakeCalled)
        {
            new DiscordMessage().PostDiscordMessage(message: $"{SteamClient.Name} has started playing REPO!",
                embed: new DiscordEmbed
                {
                    Description = $"{SteamClient.Name} has started playing REPO!",
                    Color = 65280,
                });
            awakeCalled = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("LockLobby")]
    public static void OnLobbyLocked()
    {
        new DiscordMessage().PostDiscordMessage(message: $"The lobby is now locked.", embed: new DiscordEmbed
        {
            Description = "The lobby is now locked.",
            Color = 16711680,
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch("UnlockLobby")]
    public static void OnLobbyUnlocked()
    {
        var owner = currentLobby.Owner;
        int currentLevelNumber = RunManager.instance.levelsCompleted;
        new DiscordMessage().PostDiscordMessage(message: $"[{owner.Name}'s Lobby]({GetLobbyLink(currentLobby.Id, owner.Id)}) is now open. We will be moving to level number {currentLevelNumber + 1}!",
            embed: new DiscordEmbed
            {
                Description = $"[{owner.Name}'s Lobby]({GetLobbyLink(currentLobby.Id, owner.Id)}) is now open. We will be moving to level number {currentLevelNumber + 1}!",
                Color = 65280,
            });
    }

    [HarmonyPostfix]
    [HarmonyPatch("OnLobbyMemberJoined")]
    public static void OnLobbyJoined(Lobby _lobby, Friend _friend)
    {
        if (_friend.Name == _lobby.Owner.Name) return;
        new DiscordMessage().PostDiscordMessage(message: $"{_friend.Name} has joined {_lobby.Owner.Name}'s lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}", embed: new DiscordEmbed
        {
            Description = $"{_friend.Name} has joined {_lobby.Owner.Name}'s lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}",
            Color = 65280,
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch("OnLobbyMemberLeft")]
    public static void OnLobbyLeft(Lobby _lobby, Friend _friend)
    {
        if (_friend.Name == _lobby.Owner.Name) return;
        new DiscordMessage().PostDiscordMessage(message: $"{_friend.Name} has left {_lobby.Owner.Name}'s Lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}", embed: new DiscordEmbed
        {
            Description = $"{_friend.Name} has left {_lobby.Owner.Name}'s Lobby. Lobby count: {_lobby.MemberCount}/{_lobby.MaxMembers}",
            Color = 16711680,
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch("OnDestroy")]
    public static void OnDestroy()
    {
        int currentLevelNumber = RunManager.instance.levelsCompleted;
        if (currentLevelNumber == 0) return;
        if (!lobbyClosed)
        {
            new DiscordMessage().PostDiscordMessage(message: "The Lobby has taken a wild turn into CrashMania!", embed: new DiscordEmbed
            {
                Description = "The Lobby has taken a wild turn into CrashMania!",
                Color = 16711680,
            });
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("LeaveLobby")]
    public static void OnLeaveLobby()
    {
        var lobby = SteamManager.instance.currentLobby;
        var owner = lobby.Owner;
        if (SteamClient.Name == owner.Name)
        {
            new DiscordMessage().PostDiscordMessage(message: $"{SteamClient.Name} has closed their lobby.", embed: new DiscordEmbed
            {
                Description = $"{SteamClient.Name} has closed their lobby.",
                Color = 16711680,
            });
            lobbyClosed = true;
            return;
        }
        new DiscordMessage().PostDiscordMessage(message: $"{SteamClient.Name} has left {owner.Name}'s Lobby.", embed: new DiscordEmbed
        {
            Description = $"{SteamClient.Name} has left {owner.Name}'s Lobby.",
            Color = 16711680,
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch("OnLobbyEntered")]
    public static void OnJoinLobby(Lobby _lobby)
    {
        if (_lobby.Owner.Name == SteamClient.Name) return;
        new DiscordMessage().PostDiscordMessage(message: $"{SteamClient.Name} has joined {_lobby.Owner.Name}'s lobby.", embed: new DiscordEmbed
        {
            Description = $"{SteamClient.Name} has joined {_lobby.Owner.Name}'s lobby.",
            Color = 65280,
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch("HostLobby")]
    public static void OnHostLobby()
    {
        new DiscordMessage().PostDiscordMessage(message: $"{SteamClient.Name} is now hosting a lobby, please stand by for the invite link....", embed: new DiscordEmbed
        {
            Description = $"{SteamClient.Name} is now hosting a lobby, please stand by for the invite link....",
            Color = 65280,
        });
    }

}
