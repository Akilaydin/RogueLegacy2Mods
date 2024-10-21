namespace OriGames.SoulStonesForAll
{
    using System;

    using HarmonyLib;

    [HarmonyPatch(typeof(MinimapHUDController), nameof(MinimapHUDController.OnSoulChanged))]
    public static class MinimapHUDController_Patches
    {
        private static void Prefix(MinimapHUDController __instance)
        {
            try
            {
                SoulStonesForAll.Log($"MinimapHUDController_OnSoulChanged_Patch Souls_EV.GetTotalSoulsCollected {Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true)}");
                SoulStonesForAll.Log($"MinimapHUDController_OnSoulChanged_Patch SoulDrop.FakeSoulCounter_STATIC {SoulDrop.FakeSoulCounter_STATIC}");
                    
                var previousSoulsField = AccessTools.Field(typeof(MinimapHUDController), nameof(MinimapHUDController.m_previousSoulAmount));

                SoulStonesForAll.Log($"MinimapHUDController_OnSoulChanged_Patch m_previousSoulAmount {previousSoulsField.GetValue(__instance)}");

                //SoulDrop.FakeSoulCounter_STATIC = 0;
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(MinimapHUDController_Patches)}: " + e.ToString());
            }
        }
            
        private static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                SoulStonesForAll.Log($"Exception in {nameof(MinimapHUDController_Patches)}: " + __exception.ToString());
            }
        }
    }
}