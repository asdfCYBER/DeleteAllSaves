using Game.Hud.MainMenu;
using HarmonyLib;

namespace DeleteAllSaves
{
    [HarmonyPatch(typeof(LevelListItem), nameof(LevelListItem.EllipsisClicked))]
    internal class ShowButtonsPatch
    {
        private static void Prefix(LevelListItem __instance)
        {
            // Can't access saves for this item
            if (!__instance.Item.SaveLoadPossible)
                return;

            // Button already exists (and confirmation panel too probably)
            if (__instance.AdditionalActionsPanel?.transform?.Find(UImagic.ButtonDeleteAllName) != null)
                return;

            UImagic.AddDeleteAllButton(__instance);
            UImagic.AddDeleteAllConfirmation(__instance);
            UImagic.AddDeleteAllButOneButton(__instance);
            UImagic.AddDeleteAllButOneConfirmation(__instance);
        }
    }
}
