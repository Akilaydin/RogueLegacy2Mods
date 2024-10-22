namespace OriGames.RogueLegacyModifications.SoulStonesForAll
{
    using System;

    using HarmonyLib;

    [HarmonyPatch(typeof(BossRoomController), nameof(BossRoomController.Initialize))]
    public static class BossRoomController_Patches
    {
        public static bool IsInBossFight { get; private set; }

        private static void Postfix(BossRoomController __instance)
        {
            try
            {
                __instance.IntroStartRelay.AddListener(() =>
                {
                    SoulStonesForAll.Log($"Intro Start Relay called");

                    IsInBossFight = true;
                });
                
                __instance.OutroCompleteRelay.AddListener(() =>
                {
                    SoulStonesForAll.Log($"Outro Completed Relay called");
                    
                    IsInBossFight = false;
                });
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(BossRoomController_Patches)}: {e.ToString()}");
            }
        }
    }
}