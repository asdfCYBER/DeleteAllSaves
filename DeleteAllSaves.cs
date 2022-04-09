using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Hud.MainMenu;
using Game.Level;
using Game.Mod;
using Utils;
using UnityEngine;
using HarmonyLib;

namespace DeleteAllSaves
{
    public class DeleteAllSaves : AbstractMod
    {
        private const string _id = "mod.asdfcyber.deleteallsaves";

        private readonly Harmony _harmony = new Harmony(_id);

        public static bool IsEnabled { get; set; } = false;

        public override CachedLocalizedString Title => "DeleteAllSaves";

        public override CachedLocalizedString Description
            => "Delete all saves for a level with one button";

        public override async Task OnEnable()
        {
            // Hook into game methods
            try
            {
                _harmony.PatchAll();
                Debug.Log("[DeleteAllSaves] Mod has been enabled");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DeleteAllSaves] Exception during patching: {e.GetType()}, {e.Message}");
            }

            IsEnabled = true;
            await Task.Yield();
        }

        public override async Task OnDisable()
        {
            Debug.Log("[DeleteAllSaves] Mod has been disabled");

            _harmony.UnpatchAll(_id);

            IsEnabled = false;
            await Task.Yield();
        }

        internal static void DeleteSaves(LevelListItem item, bool keepMostRecent)
        {
            // Failsafe
            if (!IsEnabled)
            {
                Debug.Log("[DeleteAllSaves] Mod is disabled, cancelling deletion");
                return;
            }

            // Get the saves
            StorageController storage = item.LevelList.StorageController;
            IOrderedEnumerable<StorageController.SaveFile> saves = storage.GetSaves(item.Item);

            // Skip the first if the most recent save should be kept,
            // and convert to IOrderedEnumerable to overwrite saves
            if (keepMostRecent)
                saves = saves.Skip(1).OrderByDescending(save => save.LastWriteTime);

            // Delete saves
            foreach (StorageController.SaveFile save in saves)
            {
                Debug.Log($"[DeleteAllSaves] Deleting save: {save.FileName}");
                storage.DeleteSave(save.FileName);
            }

            Debug.Log($"[DeleteAllSaves] {saves.Count()} saves deleted");
            UImagic.DisableConfirmations(item);
            item.LevelList.UpdateNow(force: true);
        }
    }
}
