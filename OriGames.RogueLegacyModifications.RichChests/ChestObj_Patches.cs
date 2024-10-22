namespace OriGames.RogueLegacyModifications.RichChests
{
    using HarmonyLib;
    using UnityEngine;
    using Wob_Common;

    [HarmonyPatch(typeof(ChestObj), nameof(ChestObj.OpenChestAnimCoroutine))]
    public static class ChestObj_Patches
    {
        private static void Postfix(ChestObj __instance)
        {
            // Only drop gold if the chest is a Bronze chest
            if (__instance.ChestType == ChestType.Bronze)
            {
                DropAdditionalGold();

                return;
            }

            // Handling of special item drops like Blueprints, Runes, and Challenge items

            // Calculate and drop Blueprints from the chest
            var specialItemDropObj = Traverse.Create(__instance)
                .Method(nameof(ChestObj.CalculateSpecialItemDropObj), SpecialItemType.Blueprint)
                .GetValue(SpecialItemType.Blueprint);

            Traverse.Create(__instance)
                .Method(
                    nameof(ChestObj.DropRewardFromRegularChest),
                    new[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) },
                    new object[] { SpecialItemType.Blueprint, (ISpecialItemDrop)specialItemDropObj, __instance.Level }
                )
                .GetValue(SpecialItemType.Blueprint, specialItemDropObj, __instance.Level);

            // Calculate and drop Runes from the chest
            specialItemDropObj = Traverse.Create(__instance)
                .Method(nameof(ChestObj.CalculateSpecialItemDropObj), SpecialItemType.Rune)
                .GetValue(SpecialItemType.Rune);

            Traverse.Create(__instance)
                .Method(
                    nameof(ChestObj.DropRewardFromRegularChest),
                    new[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) },
                    new object[] { SpecialItemType.Rune, (ISpecialItemDrop)specialItemDropObj, __instance.Level }
                )
                .GetValue(SpecialItemType.Rune, specialItemDropObj, __instance.Level);

            // Calculate and drop Challenge items from the chest
            specialItemDropObj = Traverse.Create(__instance)
                .Method(nameof(ChestObj.CalculateSpecialItemDropObj), SpecialItemType.Challenge)
                .GetValue(SpecialItemType.Challenge);

            Traverse.Create(__instance)
                .Method(
                    nameof(ChestObj.DropRewardFromRegularChest),
                    new[] { typeof(SpecialItemType), typeof(ISpecialItemDrop), typeof(int) },
                    new object[] { SpecialItemType.Challenge, (ISpecialItemDrop)specialItemDropObj, __instance.Level }
                )
                .GetValue(SpecialItemType.Challenge, specialItemDropObj, __instance.Level);

            // Drop additional ore (Equipment Ore and Rune Ore) based on game level and multiplier
            var oreMultiplier = WobSettings.Get(Constants.SETTINGS_EVERY_ORE_MULTIPLIER, Constants.DEFAULT_EVERY_ORE_MULTIPLIER);

            // Retrieve and adjust the drop amount for Equipment Ore (Iron Ore in this case)
            var ironOreDropAmount = GetOreDropAmount(ItemDropType.EquipmentOre, __instance.Level);
            var aetherDropAmount = GetOreDropAmount(ItemDropType.RuneOre, __instance.Level);

            // Apply ore multiplier
            ironOreDropAmount = Mathf.FloorToInt(ironOreDropAmount * oreMultiplier);
            aetherDropAmount = Mathf.FloorToInt(aetherDropAmount * oreMultiplier);

            // Drop the calculated amounts of Equipment and Rune Ore at the chest's position
            ItemDropManager.DropItem(ItemDropType.EquipmentOre, ironOreDropAmount, __instance.transform.position, true, true, true);
            ItemDropManager.DropItem(ItemDropType.RuneOre, aetherDropAmount, __instance.transform.position, true, true, true);

            // Log the total amount of ore dropped for debugging purposes
            RichChests.Log("Amount of ore dropped: EQUIPMENT " + ironOreDropAmount + " RUNE " + aetherDropAmount);

            // Drop additional gold based on the multiplier setting
            DropAdditionalGold();

            // Helper method to calculate and drop gold based on the chest's settings
            void DropAdditionalGold()
            {
                var goldMultiplier = WobSettings.Get(Constants.SETTINGS_GOLD_MULTIPLIER, Constants.DEFAULT_GOLD_MULTIPLIER);
                var goldDropAmount = __instance.Gold * goldMultiplier;

                ItemDropManager.DropGold(Mathf.FloorToInt(goldDropAmount), __instance.transform.position, true, true, true);
            }

            // Helper method to calculate ore drop amounts based on the chest level
            int GetOreDropAmount(ItemDropType oreDropType,
                int chestLevel)
            {
                var baseAmount = oreDropType == ItemDropType.RuneOre
                    ? WobSettings.Get(Constants.SETTINGS_AETHER_ORE_BASE_DROP_AMOUNT, Constants.DEFAULT_AETHER_ORE_BASE_DROP_AMOUNT)
                    : WobSettings.Get(Constants.SETTINGS_IRON_ORE_BASE_DROP_AMOUNT, Constants.DEFAULT_IRON_ORE_BASE_DROP_AMOUNT);

                // Calculate ore amount based on base amount and chest level
                return (int)(baseAmount + chestLevel);
            }
        }
    }
}