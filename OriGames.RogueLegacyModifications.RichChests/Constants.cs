namespace OriGames.RogueLegacyModifications.RichChests
{
    /// <summary>
    /// Constants for settings keys and default values for multipliers and base amounts
    /// </summary>
    public static class Constants
    {
        // Key for gold multiplier setting
        public const string SETTINGS_GOLD_MULTIPLIER = "GOLD_MULTIPLIER";
        // Key for ore multiplier setting
        public const string SETTINGS_EVERY_ORE_MULTIPLIER = "EVERY_ORE_MULTIPLIER";

        // Key and default value for Aether Ore base drop amount setting
        public const string SETTINGS_AETHER_ORE_BASE_DROP_AMOUNT = "AETHER_ORE_BASE_DROP_AMOUNT";
        public const string SETTINGS_IRON_ORE_BASE_DROP_AMOUNT = "IRON_ORE_BASE_DROP_AMOUNT";

        // Default multiplier values
        public const float DEFAULT_EVERY_ORE_MULTIPLIER = 2f;
        public const float DEFAULT_GOLD_MULTIPLIER = 2f;

        // Default base drop values for ores
        public const float DEFAULT_AETHER_ORE_BASE_DROP_AMOUNT = 150f;
        public const float DEFAULT_IRON_ORE_BASE_DROP_AMOUNT = 175f;
    }
}