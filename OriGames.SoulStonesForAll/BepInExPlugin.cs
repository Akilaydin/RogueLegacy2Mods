using BepInEx;

using HarmonyLib;

using Wob_Common;

namespace OriGames.SoulStonesForAll
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using BepInEx.Configuration;

    using GameEventTracking;

    using UnityEngine;

    [BepInPlugin("OriGames.SoulStonesForAll", "Soul Stones For All Mod", "1.0.0")]
    public partial class SoulStonesForAll : BaseUnityPlugin
    {
        private readonly static Dictionary<int, float> RankBonus = new Dictionary<int, float>();
        private const string SETTINGS_BOSS_SOULS_GAIN = "BossBonus";
        private const string SETTINGS_MINI_BOSS_BONUS = "MiniBossBonus";
        private const string SETTINGS_TIER_1_BONUS = "Tier1Bonus";
        private const string SETTINGS_TIER_2_BONUS = "Tier2Bonus";
        private const string SETTINGS_TIER_3_BONUS = "Tier1Bonus";
        private const string SETTINGS_CHEST_BONUS = "ChestBonus";
        private const string SETTINGS_EVERYTHING_BONUS = "EverythingBonus";

        protected void Awake()
        {
            // Set up the logger and basic config items
            WobPlugin.Initialise(this, Logger);
            
            Log("Initialising");
            Logger.LogError("Initialising");
            
            // Create/read the mod specific configuration options
            WobSettings.Add(new WobSettings.Entry[]
            {
                new WobSettings.Num<int>(SETTINGS_TIER_1_BONUS, "Get this amount of soul stones from tier 1 (basic) variant enemies", 0,
                    1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_TIER_2_BONUS, "Get this amount of soul stones from tier 2 (advanced) variant enemies", 0,
                    1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_TIER_3_BONUS, "Get this amount of soul stones from tier 3 (commander) variant enemies",0,
                    1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_MINI_BOSS_BONUS, "Get this amount of soul stones from mini bosses", 0,
                    1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_BOSS_SOULS_GAIN, "Get this amount of soul stones from bosses", 100,
                    1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_CHEST_BONUS, "Get this amount of soul stones for every opened chest", 0, 1, bounds: (0, 1000000)),
                new WobSettings.Num<int>(SETTINGS_EVERYTHING_BONUS, "Increases amount of soul stones by this value every time they drop", 0, 1, bounds: (0, 1000000)),
            });

            // Cache the settings into a dictionary based on the EnemyRank enum
            RankBonus.Add((int)EnemyRank.Basic, WobSettings.Get("Tier1Bonus", 0f));
            RankBonus.Add((int)EnemyRank.Advanced, WobSettings.Get("Tier2Bonus", 0f));
            RankBonus.Add((int)EnemyRank.Expert, WobSettings.Get("Tier3Bonus", 0f));
            RankBonus.Add((int)EnemyRank.Miniboss, WobSettings.Get("MiniBossBonus", 0f));
            
            //ItemEventTracker
            
            
            WobPlugin.Patch();
            
            //OnChestOpened
        }

        [HarmonyPatch(typeof(Souls_EV), MethodType.StaticConstructor)]
        public static class EconomyEV_StaticConstructorPatch
        {
            private static void Postfix()
            {
                SetBossesSoulsGain(WobSettings.Get(SETTINGS_BOSS_SOULS_GAIN, 100));
                
                Log($"Set Bosses Souls Gain to {WobSettings.Get(SETTINGS_BOSS_SOULS_GAIN, 100)}");
            }
        }
        
        [HarmonyPatch(typeof(ItemEventTracker), "OnChestOpened")]
        public static class ItemEventTrackerPatch
        {
            static void Postfix(MonoBehaviour sender, EventArgs eventArgs)
            {
                Log($"On chest opened 1");

                if (eventArgs is ChestOpenedEventArgs args)
                {
                    Log($"On chest opened 2");

                    if (args.SpecialItemType == SpecialItemType.Rune 
                        || args.SpecialItemType == SpecialItemType.Ore 
                        || args.SpecialItemType == SpecialItemType.Gold)
                    {
                        Log($"On chest opened 3");

                        //GivePlayerSouls(WobSettings.Get(SETTINGS_CHEST_BONUS, 0));
                        GivePlayerSouls(1);
                    }
                }
            }

            private static void GivePlayerSouls(int amount)
            {
                //Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, (EventArgs) null);
                Log($"Giving player souls");

                ItemDropManager.DropItem(ItemDropType.Soul, amount, Vector3.zero, false, true);

                //var d = new SoulDrop();
                //var args = new ItemCollectedEventArgs();
            }
        }
        
        private static void SetBossesSoulsGain(int newValue)
        {
            var keys = new List<BossID>(Souls_EV.BOSS_SOUL_DROP_TABLE.Keys);

            foreach (var bossID in keys)
            {
                Souls_EV.BOSS_SOUL_DROP_TABLE[bossID] = new Vector2Int(newValue, newValue);
            }
        }

        [HarmonyPatch(typeof(Economy_EV), "GetItemDropValue")]
        public static class EconomyEV_GetItemDropValue_Patch
        {
            private static void Postfix(ItemDropType itemDrop, bool getGoldValueOnly, ref int __result)
            {
                Log($"EconomyEV_GetItemDropValue_Patch {itemDrop} {getGoldValueOnly}");

                if (itemDrop == ItemDropType.Soul && !getGoldValueOnly)
                {
                    var increment = WobSettings.Get(SETTINGS_EVERYTHING_BONUS, 0);

                    __result += increment;
                }
            }
        }

        // Apply damage dealt modifiers on boss fights
        [HarmonyPatch(typeof(EnemyController), "GetInsightPlayerDamageMod")]
        internal static class EnemyController_GetInsightPlayerDamageMod_Patch
        {
            // Modify the insight damage bonuses
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Log("EnemyController.GetInsightPlayerDamageMod Transpiler Patch");
                // Set up the transpiler handler with the instruction list
                var transpiler = new WobTranspiler(instructions);

                // Perform the patching
                transpiler.PatchAll(
                    // Define the IL code instructions that should be matched
                    new List<WobTranspiler.OpTest>
                    {
                        /*  0 */ new WobTranspiler.OpTest(OpCodes.Ldc_R4, Insight_EV.INSIGHT_PLAYER_DAMAGE_MOD), // 1.15f
                        /*  1 */ new WobTranspiler.OpTest(OpCodes.Ret), // return 1.15f
                    },
                    // Define the actions to take when an occurrence is found
                    new List<WobTranspiler.OpAction>
                    {
                        new WobTranspiler.OpAction_SetOperand(0, WobSettings.Get("InsightBonus", 0.15f) + 1f),
                    });
                // Perform the patching
                transpiler.PatchAll(
                    // Define the IL code instructions that should be matched
                    new List<WobTranspiler.OpTest>
                    {
                        /*  0 */ new WobTranspiler.OpTest(OpCodeSet.Ldloc), // num
                        /*  1 */ new WobTranspiler.OpTest(OpCodes.Ldc_R4, Insight_EV.INSIGHT_FINALBOSS_PLAYER_DAMAGE_MOD), // 0.05f
                        /*  2 */ new WobTranspiler.OpTest(OpCodes.Add), // num + 0.05f
                        /*  3 */ new WobTranspiler.OpTest(OpCodeSet.Stloc), // num += 0.05f
                    },
                    // Define the actions to take when an occurrence is found
                    new List<WobTranspiler.OpAction>
                    {
                        new WobTranspiler.OpAction_SetOperand(1, WobSettings.Get("InsightPrime", 0.05f)),
                    });
                // Return the modified instructions
                return transpiler.GetResult();
            }

            // Add the additional bonus for all
            internal static void Postfix(EnemyController __instance,
                ref float __result)
            {
                // Check if the enemy is a boss
                // For non-bosses, get the bonus based on the rank
                if (RankBonus.TryGetValue((int)__instance.EnemyRank, out var bonus))
                {
                    __result += bonus;
                }

                // Print to the log if the multiplier is not one - i.e. has probably been changed by these patches
                if (__result != 1f)
                {
                    Log(__instance.EnemyType + "." + __instance.EnemyRank + " hit, damage modifier " + __result);
                }
            }
        }

        private static void Log(string message, bool error = true)
        {
            WobPlugin.Log($"{DateTime.Now}: " + message, error);
        }
    }
}