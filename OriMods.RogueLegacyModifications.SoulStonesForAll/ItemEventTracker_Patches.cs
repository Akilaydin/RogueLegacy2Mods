namespace OriGames.RogueLegacyModifications.SoulStonesForAll
{
    using System;

    using GameEventTracking;

    using HarmonyLib;

    using UnityEngine;

    [HarmonyPatch(typeof(ItemEventTracker), nameof(ItemEventTracker.OnChestOpened))]
    public class ItemEventTracker_Patches
    {
        static void Postfix(MonoBehaviour sender, EventArgs eventArgs)
        {
            try
            {
                if (BossRoomController_Patches.IsInBossFight)
                {
                    SoulStonesForAll.Log($"Not giving additional souls for boss chest");
                    
                    return;
                }
                
                SoulStonesForAll.Log($"On chest opened 1");

                if (eventArgs is ChestOpenedEventArgs args)
                {
                    SoulStonesForAll.Log($"On chest opened 2");

                    if (SoulStonesForAll.ChestBonusByType.TryGetValue(args.SpecialItemType, out int bonus))
                    {
                        SoulStonesForAll.Log($"On chest opened 3");
                        
                        SoulStonesForAll.GivePlayerSouls(bonus, args.Chest.transform.position);
                    }
                }
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(ItemEventTracker_Patches)}: {e.ToString()}");
            }
        }
    }
}