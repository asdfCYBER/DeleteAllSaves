using Game.Hud.MainMenu;
using HarmonyLib;

namespace DeleteAllSaves
{
    [HarmonyPatch(typeof(LevelListItem), nameof(LevelListItem.DeleteNoClicked))]
    internal class ShowButtonsOnDeleteCancelPatch
    {
        private static void Postfix(LevelListItem __instance)
        {
            // Show own delete buttons if the map delete cancel button was clicked
            __instance.AdditionalActionsRect.Find(UImagic.ButtonDeleteAllName)?.gameObject?.SetActive(true);
            __instance.AdditionalActionsRect.Find(UImagic.ButtonDeleteAllButOneName)?.gameObject?.SetActive(true);
        }
    }
}
