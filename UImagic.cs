using Game.Hud;
using Game.Hud.MainMenu;
using TMPro;
using UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace DeleteAllSaves
{
    public static class UImagic
    {
        public const string ButtonDeleteAllName = "Delete all saves button";

        public const string ButtonDeleteAllButOneName = "Delete all saves except most recent button";

        public const string ConfirmationDeleteAllName = "Delete all saves confirmation";
        
        public const string ConfirmationDeleteAllButOneName = "Delete all saves except most recent confirmation";

        /// <summary>
        /// Add a button to the additional options panel of <paramref name="item"/> to delete all saves
        /// </summary>
        internal static void AddDeleteAllButton(LevelListItem item)
        {
            // Create the button, change tooltip
            GameObject buttonHolder = AddDeleteButtonTemplate(item, ButtonDeleteAllName);
            ReplaceTooltip(buttonHolder, "Erase all savefiles for this map");

            // Change button behaviour
            MultiTargetButton button = buttonHolder.GetComponent<MultiTargetButton>();
            ReplaceOnClickEvent(button, delegate { OnClickDeleteAll(item, keepMostRecent: false); });
        }

        /// <summary>
        /// Add a button to the additional options panel of <paramref name="item"/>
        /// to delete all saves but the most recent one
        /// </summary>
        internal static void AddDeleteAllButOneButton(LevelListItem item)
        {
            // Create the button, change tooltip
            GameObject buttonHolder = AddDeleteButtonTemplate(item, ButtonDeleteAllButOneName);
            ReplaceTooltip(buttonHolder, "Erase all savefiles except the most recent one for this map");

            // Change button behaviour
            MultiTargetButton button = buttonHolder.GetComponent<MultiTargetButton>();
            ReplaceOnClickEvent(button, delegate { OnClickDeleteAll(item, keepMostRecent: true); });

            // Change color to orange
            ColorBlock buttonColors = button.colors;
            buttonColors.normalColor = buttonColors.pressedColor = new Color(1f, 0.65f, 0f);
            button.colors = buttonColors;
        }

        /// <summary>
        /// Create a new confirmation panel for <paramref name="item"/>
        /// </summary>
        /// <param name="objectName">The name to give the new GameObject</param>
        /// <param name="keepMostRecent">Whether to keep the most recent save</param>
        private static void AddDeleteConfirmation(LevelListItem item, string objectName, bool keepMostRecent)
        {
            // Create panel
            GameObject eraseConfirmation = DuplicateObject(item.DeleteConfirmationContent, objectName);
            eraseConfirmation.transform.SetSiblingIndex(2); // VerticalLayoutGroup magic

            // Change button behaviour
            MultiTargetButton[] buttons = eraseConfirmation.GetComponentsInChildren<MultiTargetButton>();
            ReplaceOnClickEvent(buttons[0], delegate { OnClickConfirm(item, keepMostRecent); });
            ReplaceOnClickEvent(buttons[1], delegate { OnClickCancel(item); });

            // Remove existing LocalizeStringEvents
            foreach (LocalizeStringEvent localize in eraseConfirmation.GetComponentsInChildren<LocalizeStringEvent>())
                Object.Destroy(localize);
        }

        /// <summary>
        /// Create a new confirmation panel for deleting all saves of <paramref name="item"/>
        /// </summary>
        internal static void AddDeleteAllConfirmation(LevelListItem item)
            => AddDeleteConfirmation(item, ConfirmationDeleteAllName, keepMostRecent: false);

        /// <summary>
        /// Create a new confirmation panel for deleting all but one save of <paramref name="item"/>
        /// </summary>
        internal static void AddDeleteAllButOneConfirmation(LevelListItem item)
            => AddDeleteConfirmation(item, ConfirmationDeleteAllButOneName, keepMostRecent: true);

        /// <summary>
        /// Show the relevant confirmation panel and hide the options bar for <paramref name="item"/>
        /// </summary>
        /// <param name="keepMostRecent">Wheter to keep the most recent save</param>
        private static void OnClickDeleteAll(LevelListItem item, bool keepMostRecent)
        {
            Debug.Log($"[DeleteAllSaves] Erase button clicked for {item.Item.Name}. Keep most recent: {keepMostRecent}");

            Transform parent = item.DeleteConfirmationContent.transform.parent;
            if (keepMostRecent)
            {
                parent.Find(ConfirmationDeleteAllButOneName).gameObject.SetActive(true);
                item.LsDelete.FillLocalizedString(delegate (string s) { item.Name.text = "Delete all saves but one?"; });
            }
            else
            {
                parent.Find(ConfirmationDeleteAllName).gameObject.SetActive(true);
                item.LsDelete.FillLocalizedString(delegate (string s) { item.Name.text = "Delete all saves?"; });
            }

            ShowConfirmationButtonsOnly(item);
        }

        /// <summary>
        /// Find and delete saves for <paramref name="item"/>
        /// </summary>
        /// <param name="keepMostRecent">Whether to keep the most recent save</param>
        private static void OnClickConfirm(LevelListItem item, bool keepMostRecent)
        {
            Debug.Log($"[DeleteAllSaves] Erase confirmed for {item.Item.Name}. Keep most recent: {keepMostRecent}");
            DeleteAllSaves.DeleteSaves(item, keepMostRecent);
            ShowAllButtons(item);
        }

        private static void OnClickCancel(LevelListItem item)
        {
            Debug.Log($"[DeleteAllSaves] Erase canceled for {item.Item.Name}");
            DisableConfirmations(item);
        }

        /// <summary>
        /// Hide the confirmation panels
        /// </summary>
        /// <param name="item"></param>
        internal static void DisableConfirmations(LevelListItem item)
        {
            Transform parent = item.DeleteConfirmationContent.transform.parent;
            parent.Find(ConfirmationDeleteAllName).gameObject.SetActive(false);
            parent.Find(ConfirmationDeleteAllButOneName).gameObject.SetActive(false);
            item.Name.text = item.Item.Name;
            ShowAllButtons(item);
        }

        /// <summary>
        /// Create a new button to serve as the base for the delete-all and delete-all-but-one buttons
        /// </summary>
        private static GameObject AddDeleteButtonTemplate(LevelListItem item, string objectName)
        {
            // Create the button
            GameObject buttonHolder = DuplicateObject(item.DeleteButton.gameObject, objectName);
            buttonHolder.transform.SetAsFirstSibling(); // Make button appear on the left
            buttonHolder.SetActive(true);

            // Remove existing LocalizeStringEvents
            foreach (LocalizeStringEvent localize in buttonHolder.GetComponentsInChildren<LocalizeStringEvent>())
                Object.Destroy(localize);

            // Change the icon
            TextMeshProUGUI[] tmpComponents = buttonHolder.GetComponentsInChildren<TextMeshProUGUI>();
            tmpComponents[0].text = "\uF12D"; // 'erase' font-awesome 5 icon

            return buttonHolder;
        }

        /// <summary>
        /// Duplicate the GameObject <paramref name="obj"/>, give it the name <paramref name="name"/>
        /// and attach it as a child to the original object's panel. Returns the new object.
        /// </summary>
        private static GameObject DuplicateObject(GameObject obj, string name)
        {
            GameObject newObj = Object.Instantiate(obj);
            Object.DontDestroyOnLoad(newObj);
            newObj.transform.SetParent(obj.transform.parent, worldPositionStays: false);
            newObj.name = name;
            return newObj;
        }

        /// <summary>
        /// Reset the onClick events of <paramref name="button"/> and 
        /// set <paramref name="call"/> as a new listener for onClick
        /// </summary>
        private static void ReplaceOnClickEvent(MultiTargetButton button, UnityAction call)
        {
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(call);
            button.onRightClick = new Button.ButtonClickedEvent(); // probably not necessary
        }

        /// <summary>
        /// Change the tooltip attached to <paramref name="obj"/> to display <paramref name="text"/>
        /// </summary>
        private static void ReplaceTooltip(GameObject obj, string text)
        {
            Tooltip tooltip = obj.GetComponent<Tooltip>();
            tooltip.TooltipText = text;
            tooltip.Text = text;
            tooltip.LocalizedText = new LocalizedString();
        }

        internal static void ShowConfirmationButtonsOnly(LevelListItem item)
        {
            // Disable game buttons
            item.DeleteClicked();
            item.DeleteConfirmationContent.SetActive(false);

            // Disable own buttons
            item.AdditionalActionsRect.Find(ButtonDeleteAllName)?.gameObject?.SetActive(false);
            item.AdditionalActionsRect.Find(ButtonDeleteAllButOneName)?.gameObject?.SetActive(false);
        }

        internal static void ShowAllButtons(LevelListItem item)
        {
            item.DeleteNoClicked();

            // Enable own buttons
            item.AdditionalActionsRect.Find(ButtonDeleteAllName)?.gameObject?.SetActive(true);
            item.AdditionalActionsRect.Find(ButtonDeleteAllButOneName)?.gameObject?.SetActive(true);
        }
    }
}
