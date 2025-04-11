
using BepInEx.Logging;

namespace DiscordHook.Stats;

/// <summary>
/// TODO
/// This class is used to track various statistics in the game, such as total deaths, revives, kills, damage, etc.
/// It provides methods to increment these statistics and log the current values.
/// </summary>
public static class Tracker
{
    public static int TotalDeaths { get; set; } = 0;
    public static int TotalRevives { get; set; } = 0;
    public static int TotalKills { get; set; } = 0;
    public static int TotalDamage { get; set; } = 0;
    public static int TotalDamageTaken { get; set; } = 0;
    public static int TotalDamageDealt { get; set; } = 0;
    
    private static ManualLogSource _logger = DiscordHook.Logger;

    public static void IncrementDeaths()
    {
        _logger.LogInfo($"Total Deaths: {TotalDeaths}");
    }

    public static void IncrementRevives()
    {
        _logger.LogInfo($"Total Revives: {TotalRevives}");
    }
    public static void IncrementKills()
    {
        _logger.LogInfo($"Total Kills: {TotalKills}");
    }
    public static void IncrementDamage(int damage)
    {
        TotalDamage += damage;
        _logger.LogInfo($"Total Damage: {TotalDamage}");
    }
    public static void IncrementDamageTaken(int damage)
    {
        TotalDamageTaken += damage;
        _logger.LogInfo($"Total Damage Taken: {TotalDamageTaken}");
    }
    public static void IncrementDamageDealt(int damage)
    {
        TotalDamageDealt += damage;
        _logger.LogInfo($"Total Damage Dealt: {TotalDamageDealt}");
    }
}
