namespace OriGames.RogueLegacyModifications.SoulStonesForAll
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

                if (__instance.DisableDeath)
                {
                    //todo: check if this is the right way to disable reward for trait on immortality
                    SoulStonesForAll.Log($"Not giving bonus to enemy with disabled death");

                    return;
                }
                
                if (Constants.Bosses.Contains(__instance.EnemyType))
                {
                    SoulStonesForAll.Log($"Not giving bonus to BOSS with type {__instance.EnemyType}");

                    return;
                }

                if (Constants.EnemiesToExcludeFromBonus.Contains(__instance.EnemyType))
                {
                    SoulStonesForAll.Log($"Not giving bonus to excluded enemy with type {__instance.EnemyType}");

                    return;
                }
                
                if (SoulStonesForAll.EnemyBonusByRank.TryGetValue(__instance.EnemyRank, out int soulStonesForEnemy))
                {
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