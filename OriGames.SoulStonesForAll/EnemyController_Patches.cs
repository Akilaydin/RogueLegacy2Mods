namespace OriGames.SoulStonesForAll
{
    using System;

    using HarmonyLib;

    [HarmonyPatch(typeof(EnemyController), nameof(EnemyController.KillCharacter))]
    public static class EnemyController_Patches
    {
        private static void Postfix(EnemyController __instance)
        {
            try
            {
                SoulStonesForAll.Log(__instance.EnemyType + "." + __instance.EnemyRank + " killed. Rank bonus is " + SoulStonesForAll.EnemyBonusByRank.ContainsKey(__instance.EnemyRank));
                
                if (SoulStonesForAll.EnemyBonusByRank.TryGetValue(__instance.EnemyRank, out int soulStonesForEnemy))
                {
                    //Remove for BouncySpike.Basic killed. Rank bonus is True
                    //Target.Basic killed. Rank bonus is True
                    //CaveBoss.Basic killed. Rank bonus is True AND OTHER BOSSES
                    //Skeleton.Miniboss killed. Rank bonus is True
                    //TO CHECK IF PLAYER IS IN BOSS BATTLE OR TRIAL WITH TARGETS
                    //todo: Disable for boss fights. Check for target, bouncySpike, trait on immortality
                    SoulStonesForAll.Log($"Enemy rank {__instance.EnemyRank} was found and rank bonus is {soulStonesForEnemy}");

                    SoulStonesForAll.GivePlayerSouls(soulStonesForEnemy, __instance.transform.position);
                }
            }
            catch (Exception e)
            {
                SoulStonesForAll.Log($"Exception in {nameof(EnemyController_Patches)}: " + e.ToString());
            }
                
        }
    }
}