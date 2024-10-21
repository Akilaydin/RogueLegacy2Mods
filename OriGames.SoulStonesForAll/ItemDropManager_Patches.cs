namespace OriGames.SoulStonesForAll
{
    using System;

    using HarmonyLib;

    [HarmonyPatch(typeof(ItemDropManager), nameof(ItemDropManager.Initialize))]
    public static class ItemDropManager_Patches
    {
        public static ItemDropManager ItemDropManagerInstance { get; private set; }
        
        private static void Postfix(ItemDropManager __instance)
        {
            try
            {
                ItemDropManagerInstance = __instance;

                SoulStonesForAll.Log($"Initialized ItemDropManager with {ItemDropManagerInstance}");
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(ItemDropManager_Patches)}: " + e.ToString());
            }
                
        }
    }
}