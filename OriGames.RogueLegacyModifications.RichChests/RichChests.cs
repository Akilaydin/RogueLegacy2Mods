using BepInEx;

using Wob_Common;

//note: IMPORTANT NOTE: Patches moved to separate classes, but still use [HarmonyPatch] overload with both class and method names.
//See this issue for more info: https://discord.com/channels/131466550938042369/361891646742462467/1297901342701912168

namespace OriGames.RogueLegacyModifications.RichChests
{
    using System;

    [BepInPlugin("OriMods.RogueLegacyModifications.RichChests", "Rich Chests Mod", "1.0.0")]
    public partial class RichChests : BaseUnityPlugin
    {
        protected void Awake()
        {
            WobPlugin.Initialise(this, Logger);
            
            try
            {
                InitializeSettings();
                
                WobPlugin.Patch();
                
                Log("WobPlugin patched");
                
                Log("Mod fully initialized");
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }

        private static void InitializeSettings()
        {
            WobSettings.Add(new WobSettings.Entry[]
            {
                // Gold Multiplier Setting
                new WobSettings.Num<float>(
                    Constants.SETTINGS_GOLD_MULTIPLIER, 
                    "Multiplier for gold drops from chests", 
                    value: Constants.DEFAULT_GOLD_MULTIPLIER, 
                    scaler: 1f
                ),
        
                // Every Ore Multiplier Setting
                new WobSettings.Num<float>(
                    Constants.SETTINGS_EVERY_ORE_MULTIPLIER, 
                    "Multiplier for all ore drops from chests", 
                    value: Constants.DEFAULT_EVERY_ORE_MULTIPLIER, 
                    scaler: 1f
                ),
        
                // Aether Ore Base Drop Amount Setting
                new WobSettings.Num<float>(
                    Constants.SETTINGS_AETHER_ORE_BASE_DROP_AMOUNT, 
                    "Base drop amount for Aether Ore from chests", 
                    value: Constants.DEFAULT_AETHER_ORE_BASE_DROP_AMOUNT, 
                    scaler: 1f
                ),
        
                // Iron Ore Base Drop Amount Setting
                new WobSettings.Num<float>(
                    Constants.SETTINGS_IRON_ORE_BASE_DROP_AMOUNT, 
                    "Base drop amount for Iron Ore from chests", 
                    value: Constants.DEFAULT_IRON_ORE_BASE_DROP_AMOUNT, 
                    scaler: 1f
                ),
            });
        }

                
        public static void Log(string message, bool error = true)
        {
            WobPlugin.Log($"{DateTime.Now}: " + message, error);
        }
    }
}