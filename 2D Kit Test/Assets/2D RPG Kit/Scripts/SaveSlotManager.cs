using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotManager : MonoBehaviour
{
    [Header("Settings")]
    public int maxSaveSlots = 5; // The maximum number of save slots available (including auto-save slot)

    void Start()
    {
        // Update the buttons based on the available save slots
        UpdateSaveSlotButtons();
    }

    public void UpdateSaveSlotButtons()
    {
        Button firstAvailableButton = null; // Keep track of the first interactable button

        // Handle the auto-save slot (Slot 0) separately
        if (transform.childCount > 0)
        {
            Transform autoSaveSlot = transform.GetChild(0);
            if (IsAutoSaveSlotUsed()) // Always consider slot 0 as auto-save
            {
                SetButtonInteractable(autoSaveSlot, true);
                if (firstAvailableButton == null)
                {
                    firstAvailableButton = autoSaveSlot.GetComponent<Button>();
                }
            }
            else
            {
                SetButtonInteractable(autoSaveSlot, false);
            }
        }

        // Loop through each child button starting from index 1 for regular save slots
        for (int i = 1; i < transform.childCount; i++)
        {
            if (i <= maxSaveSlots) // Ensure we only check up to the max save slots defined (excluding the auto-save)
            {
                // Check if the slot has saved data
                if (IsSaveSlotUsed(i)) // Slot 1 and onward
                {
                    SetButtonInteractable(transform.GetChild(i), true);  // Make button interactable if save slot is used

                    // If this is the first interactable button, store it
                    if (firstAvailableButton == null)
                    {
                        firstAvailableButton = transform.GetChild(i).GetComponent<Button>();
                    }
                }
                else
                {
                    SetButtonInteractable(transform.GetChild(i), false); // Make button non-interactable if no save data
                }
            }
            else
            {
                // Make buttons non-interactable that are beyond the maxSaveSlots count
                SetButtonInteractable(transform.GetChild(i), false);
            }
        }

        // Highlight the first available button, if found
        if (firstAvailableButton != null)
        {
            firstAvailableButton.Select(); // Highlight the first available button
        }
    }

    // Helper function to check if the auto-save slot (Slot 0) is used
    private bool IsAutoSaveSlotUsed()
    {
        // Auto-save slot uses a unique PlayerPref key
        string key = $"Player_Position_x_Slot0";
        return PlayerPrefs.HasKey(key);
    }

    // Helper function to check if a regular save slot is used
    private bool IsSaveSlotUsed(int slotNumber)
    {
        // Check if the slot has saved data using the correct keys
        string key = $"Player_Position_x_Slot{slotNumber}";
        return PlayerPrefs.HasKey(key);
    }

    // Helper function to set button interactability
    private void SetButtonInteractable(Transform buttonTransform, bool isInteractable)
    {
        Button button = buttonTransform.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = isInteractable;

            // Optionally, set visual or text changes based on the slot type (Auto-Save or Regular Save)
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                if (buttonTransform.GetSiblingIndex() == 0)
                {
                    buttonText.text = "Auto-Save";
                }
                else
                {
                    buttonText.text = "Save Slot " + buttonTransform.GetSiblingIndex();
                }
            }
        }
        else
        {
            Debug.LogWarning("No Button component found on " + buttonTransform.name);
        }
    }
}
