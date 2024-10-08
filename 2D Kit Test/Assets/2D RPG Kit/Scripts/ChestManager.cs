using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    // Make instance of this script to be able to reference from other scripts!
    public static ChestManager instance;

    [Header("Chest Settings")]
    public string[] chests;
    public bool[] openedChests;

    // Use this for initialization
    void Start()
    {
        instance = this;
        openedChests = new bool[chests.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get the number of a chest
    public int GetChestNumber(string chestToFind)
    {
        for (int i = 0; i < chests.Length; i++)
        {
            if (chests[i] == chestToFind)
            {
                return i;
            }
        }

        Debug.LogError("Chest " + chestToFind + " does not exist");
        return 0;
    }

    // Check if a chest was opened
    public bool CheckIfOpened(string chestToCheck)
    {
        if (GetChestNumber(chestToCheck) != 0)
        {
            return openedChests[GetChestNumber(chestToCheck)];
        }

        return false;
    }

    // Mark a chest as opened
    public void MarkChestOpened(string chestToMark)
    {
        openedChests[GetChestNumber(chestToMark)] = true;
        UpdateLocalChestObjects();
    }

    // Mark a chest as unopened
    public void MarkChestUnopened(string chestToMark)
    {
        openedChests[GetChestNumber(chestToMark)] = false;
        UpdateLocalChestObjects();
    }

    // Update game objects associated with a chest
    public void UpdateLocalChestObjects()
    {
        ChestObjectActivator[] chestObjects = FindObjectsOfType<ChestObjectActivator>();

        if (chestObjects.Length > 0)
        {
            for (int i = 0; i < chestObjects.Length; i++)
            {
                chestObjects[i].CheckCompletion();
            }
        }
    }

    // Save chest data to a specified slot
    public void SaveChestData(int saveSlot)
    {
        // Define a prefix based on the save slot number
        string slotPrefix = "SaveSlot_" + saveSlot + "_";

        for (int i = 0; i < chests.Length; i++)
        {
            if (openedChests[i])
            {
                PlayerPrefs.SetInt(slotPrefix + "ChestMarker_" + chests[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt(slotPrefix + "ChestMarker_" + chests[i], 0);
            }
        }
    }

    // Load chest data from a specified slot
    public void LoadChestData(int saveSlot)
    {
        // Define a prefix based on the save slot number
        string slotPrefix = "SaveSlot_" + saveSlot + "_";

        for (int i = 0; i < chests.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey(slotPrefix + "ChestMarker_" + chests[i]))
            {
                valueToSet = PlayerPrefs.GetInt(slotPrefix + "ChestMarker_" + chests[i]);
            }

            if (valueToSet == 0)
            {
                openedChests[i] = false;
            }
            else
            {
                openedChests[i] = true;
            }
        }
    }
}
