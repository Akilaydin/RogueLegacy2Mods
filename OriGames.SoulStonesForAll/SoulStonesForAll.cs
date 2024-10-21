using BepInEx;

using Wob_Common;

namespace OriGames.SoulStonesForAll
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;

    [BepInPlugin("OriGames.SoulStonesForAll", "Soul Stones For All Mod", "1.0.0")]
    public partial class SoulStonesForAll : BaseUnityPlugin
    {
        readonly public static Dictionary<EnemyRank, int> EnemyBonusByRank = new Dictionary<EnemyRank, int>();
        readonly public static Dictionary<SpecialItemType, int> ChestBonusByType = new Dictionary<SpecialItemType, int>();
        
        protected void Awake()
        {
            WobPlugin.Initialise(this, Logger);
            
            try
            {
                InitializeSettings();
                
                InitializeDictionaries();

                WobPlugin.Patch();
                
                Log("WobPlugin patched");

                SetBossesSoulsGain(WobSettings.Get(Constants.SETTINGS_BOSS_SOULS_GAIN, 100));
            
                Log("Mod fully initialized");
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }

        public static void GivePlayerSouls(int amount, Vector3 position)
        {
            try
            {
                if (amount <= 0)
                {
                    return;
                }
                
                Log($"Giving player souls {amount}. Souls collected before: {SaveManager.ModeSaveData.MiscSoulCollected}");
                SoulDrop.FakeSoulCounter_STATIC += amount;
                SaveManager.ModeSaveData.MiscSoulCollected += amount;

                var itemDropManagerType = typeof(ItemDropManager);
                
                var internalDropItemMethod = itemDropManagerType.GetMethod(nameof(ItemDropManager.Internal_DropItem), BindingFlags.NonPublic | BindingFlags.Instance);

                object[] parameters = { ItemDropType.Soul, amount, position, false, true, false, true };

                internalDropItemMethod.Invoke(ItemDropManager_Patches.ItemDropManagerInstance, parameters);
                
                Log($"Gave player souls {amount}. Souls collected after: {SaveManager.ModeSaveData.MiscSoulCollected}");
                
                Log($"=========EXIT GivePlayerSouls");

            }
            catch (Exception e)
            {
                Log($"Exception in GivePlayerSouls: " + e.ToString());
            }
        }
        
        private static void SetBossesSoulsGain(int newValue)
        {
            Log($"Set Bosses Souls Gain to {newValue}");
            
            var keys = new List<BossID>(Souls_EV.BOSS_SOUL_DROP_TABLE.Keys);

            foreach (var bossID in keys)
            {
                Souls_EV.BOSS_SOUL_DROP_TABLE[bossID] = new Vector2Int((int)newValue, (int)newValue);
            }
        }
        
        private static void InitializeDictionaries()
        {
            EnemyBonusByRank.Add(EnemyRank.Basic, WobSettings.Get(Constants.SETTINGS_TIER_1_BONUS, 0));
            EnemyBonusByRank.Add(EnemyRank.Advanced, WobSettings.Get(Constants.SETTINGS_TIER_2_BONUS, 0));
            EnemyBonusByRank.Add(EnemyRank.Expert, WobSettings.Get(Constants.SETTINGS_TIER_3_BONUS, 0));
            EnemyBonusByRank.Add(EnemyRank.Miniboss, WobSettings.Get(Constants.SETTINGS_MINI_BOSS_BONUS, 0));
                
            ChestBonusByType.Add(SpecialItemType.Gold, WobSettings.Get(Constants.SETTINGS_GOLD_CHEST_BONUS, 0));
            ChestBonusByType.Add(SpecialItemType.Ore, WobSettings.Get(Constants.SETTINGS_ORE_CHEST_BONUS, 0));
            ChestBonusByType.Add(SpecialItemType.Rune, WobSettings.Get(Constants.SETTINGS_FAIRY_CHEST_BONUS, 0));
        }

        private static void InitializeSettings()
        {
            WobSettings.Add(new WobSettings.Entry[]
            {
                new WobSettings.Num<int>(Constants.SETTINGS_TIER_1_BONUS, "Get this amount of soul stones from tier 1 (basic) variant enemies", 0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_TIER_2_BONUS, "Get this amount of soul stones from tier 2 (advanced) variant enemies", 0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_TIER_3_BONUS, "Get this amount of soul stones from tier 3 (commander) variant enemies",0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_MINI_BOSS_BONUS, "Get this amount of soul stones from mini bosses", 0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_BOSS_SOULS_GAIN, "Get this amount of soul stones from bosses", 100, 1),
                    
                new WobSettings.Num<int>(Constants.SETTINGS_GOLD_CHEST_BONUS, "Get this amount of soul stones for every opened gold chest", 0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_FAIRY_CHEST_BONUS, "Get this amount of soul stones for every opened fairy chest", 0, 1),
                new WobSettings.Num<int>(Constants.SETTINGS_ORE_CHEST_BONUS, "Get this amount of soul stones for every opened ore chest", 0, 1),
                    
                new WobSettings.Num<int>(Constants.SETTINGS_EVERYTHING_BONUS, "Increases amount of soul stones by this value every time they drop", 0, 1),
            });
        }
                
        public static void Log(string message, bool error = true)
        {
            WobPlugin.Log($"{DateTime.Now}: " + message, error);
        }
    }

}