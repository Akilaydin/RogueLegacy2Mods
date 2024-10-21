namespace OriGames.SoulStonesForAll
{
    using System.Collections.Generic;

    public class Constants
    {
        public const string SETTINGS_BOSS_SOULS_GAIN = "BossBonus";
        public const string SETTINGS_MINI_BOSS_BONUS = "MiniBossBonus";
        public const string SETTINGS_TIER_1_BONUS = "Tier1Bonus";
        public const string SETTINGS_TIER_2_BONUS = "Tier2Bonus";
        public const string SETTINGS_TIER_3_BONUS = "Tier3Bonus";
        public const string SETTINGS_GOLD_CHEST_BONUS = "GoldChestBonus";
        public const string SETTINGS_ORE_CHEST_BONUS = "OreChestBonus";
        public const string SETTINGS_FAIRY_CHEST_BONUS = "FairyChestBonus";
        public const string SETTINGS_EVERYTHING_BONUS = "EverythingBonus";
        
        readonly public static HashSet<EnemyType> Bosses = new HashSet<EnemyType> {
            /* Lamech  */ EnemyType.SpellswordBoss,
            /* Pirates */ EnemyType.SkeletonBossA, EnemyType.SkeletonBossB,
            /* Naamah  */ EnemyType.DancingBoss,
            /* Enoch   */ EnemyType.StudyBoss, EnemyType.MimicChestBoss,
            /* Irad    */ EnemyType.EyeballBoss_Left, EnemyType.EyeballBoss_Right, EnemyType.EyeballBoss_Bottom, EnemyType.EyeballBoss_Middle,
            /* Tubal   */ EnemyType.CaveBoss,
            /* Jonah   */ EnemyType.TraitorBoss,
            /* Cain    */ EnemyType.FinalBoss,
        };
    }
}