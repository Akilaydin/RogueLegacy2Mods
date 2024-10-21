namespace OriGames.SoulStonesForAll
{
    using System;

    using HarmonyLib;

    [HarmonyPatch(typeof(Economy_EV), nameof(Economy_EV.GetItemDropValue))]
    public static class EconomyEV_Patches
    {
        private static void Postfix(ItemDropType itemDrop, bool getGoldValueOnly, ref int __result)
        {
            try
            {
                if (itemDrop == ItemDropType.Soul && !getGoldValueOnly)
                {
                    //float increment = WobSettings.Get(SETTINGS_EVERYTHING_BONUS, 0);

                    //__result += (int)increment;
                    
                    //Log($"EconomyEV_GetItemDropValue_Patch increment was {increment} and final result was {__result}");
                }
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(EconomyEV_Patches)}: " + e.ToString());
            }
        }
    }
}