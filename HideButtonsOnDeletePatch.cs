using Game.Hud.MainMenu;
using HarmonyLib;

namespace DeleteAllSaves
{
    [HarmonyPatch(typeof(LevelListItem), nameof(LevelListItem.DeleteClicked))]
    internal class HideButtonsOnDeletePatch
    {
        private static void Prefix(LevelListItem __instance)
        {
            // Hide own delete buttons if the map delete button was clicked
            __instance.AdditionalActionsRect.Find(UImagic.ButtonDeleteAllName)?.gameObject?.SetActive(false);
            __instance.AdditionalActionsRect.Find(UImagic.ButtonDeleteAllButOneName)?.gameObject?.SetActive(false);
        }
    }
}
