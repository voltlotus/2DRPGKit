using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Make instance of this script to be able reference from other scripts!
    public static GameManager instance;

    public int loadedSaveId;

    [HideInInspector]
    public int toolbar;
    [HideInInspector]
    public string currentTab;

    [Header("Initialization")]
    //Game objects used by this code
    public List<GameObject> characterSlots;
    //public GameObject character0;
    //public GameObject character1;
    //public GameObject character2;

    //Initialize a list of all character stats
    public List<CharacterStatus> characterStatus;

    [Header("Currently active menus")]
    //Bools for checking if one of these menus is currently active
    public bool cutSceneActive;
    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;
    public bool shopActive;
    public bool battleActive;
    public bool saveMenuActive;
    public bool innActive;
    public bool itemCharChoiceMenu;
    public bool loadPromt;
    public bool quitPromt;
    public bool itemMenu;
    public bool equipMenu;
    public bool statsMenu;
    public bool skillsMenu;

    [Header("Character Bools")]
    //For checking if the player can move
    public bool confirmCanMove;

    [Header("Existing Game Items")]
    //Put items that the game uses here!
    public Item[] existingItems;

    [Header("Currently Owned Items")]
    public Item[] itemsInInventory;
    //View items that are in your inventory Can also be used to give the player some items to start the game with
    public string[] itemsHeld;
    //public int[] numberOfItems;
    public Item[] equipmentInInventory;
    public string[] equipItemsHeld;    
    //public int[] numberOfEquipItems;
    
    [Header("Gold Settings")]
    //The amount of gold currently owned by the player. Can also be used to give the player some gold to start the game with
    public int currentGold;

    [Header("Dificulty Settings")]
    public bool easy;
    public bool normal;
    public bool hard;

    [Header("Cheats")]
    public bool infiniteHP;
    public bool infiniteSP;
    public bool infiniteGold;
    public bool noEncounters;

    // Use this for initialization
    void Start () {
        instance = this;

        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i] != null)
            {
                itemsHeld[i] = itemsInInventory[i].itemName;
            }
            else
            {
                itemsHeld[i] = "";
            }            
        }

        for (int i = 0; i < equipmentInInventory.Length; i++)
        {
            if (equipmentInInventory[i] != null)
            {
                equipItemsHeld[i] = equipmentInInventory[i].itemName;
            }
            else
            {
                equipItemsHeld[i] = "";
            }
        }

        DontDestroyOnLoad(gameObject);

        SortItems();
	}
	
	// Update is called once per frame
	void Update () {

        if (infiniteGold)
        {
            currentGold = 9999;
        }

        //Check if any meu is currently open and prevent the player from moving
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive || innActive || itemCharChoiceMenu || loadPromt || quitPromt || itemMenu || equipMenu ||statsMenu)
        {
            PlayerController.instance.canMove = false;
            confirmCanMove = PlayerController.instance.canMove;
        } else
        {
            PlayerController.instance.canMove = true;
            confirmCanMove = PlayerController.instance.canMove;
        }
    }

    //Returns the details of a list of items
    public Item GetItemDetails(string itemToGrab)
    {

        for(int i = 0; i < existingItems.Length; i++)
        {
            if(existingItems[i].itemName == itemToGrab)
            {
                return existingItems[i];
            }
        }
        
        return null;
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    //numberOfItems[i] = numberOfItems[i + 1];
                    //numberOfItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    //An algorithm to sort items in a list to avoid empty spaces in the inventory
    public void SortEquipItems()
    {
        bool itemAFterSpace = true;

        while (itemAFterSpace)
        {
            itemAFterSpace = false;
            for (int i = 0; i < equipItemsHeld.Length - 1; i++)
            {
                if (equipItemsHeld[i] == "")
                {
                    equipItemsHeld[i] = equipItemsHeld[i + 1];
                    equipItemsHeld[i + 1] = "";

                    //numberOfEquipItems[i] = numberOfEquipItems[i + 1];
                    //numberOfEquipItems[i + 1] = 0;

                    if (equipItemsHeld[i] != "")
                    {
                        itemAFterSpace = true;
                    }
                }
            }
        }
    }

    //A method to add items to the inventory
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
        if (Shop.instance.selectedItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "" )//|| itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        }

        if (Shop.instance.selectedItem.offense || Shop.instance.selectedItem.defense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == "" )//|| equipItemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = equipItemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    equipItemsHeld[newItemPosition] = itemToAdd;
                    //numberOfEquipItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }
        }

        GameMenu.instance.ShowItems();

        GameMenu.instance.buyItemButton.interactable = true;
    }

    //A method for equipping items
    public void EquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == "")// || equipItemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = equipItemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    equipItemsHeld[newItemPosition] = itemToAdd;
                    //numberOfEquipItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }

        GameMenu.instance.ShowItems();
        
    }

    //A method to add reward items after battle
    public void AddRewardItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == "")// || itemsHeld[i] == itemToAdd)
                {
                    newItemPosition = i;
                    i = itemsHeld.Length;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                bool itemExists = false;
                for (int i = 0; i < existingItems.Length; i++)
                {
                    if (existingItems[i].itemName == itemToAdd)
                    {
                        itemExists = true;

                        i = existingItems.Length;
                    }
                }

                if (itemExists)
                {
                    itemsHeld[newItemPosition] = itemToAdd;
                    //numberOfItems[newItemPosition]++;
                }
                else
                {
                    Debug.LogError(itemToAdd + " Does Not Exist!!");
                }
            }        
        
        GameMenu.instance.ShowItems();
    }

    //A method to add reward equip items after battle
    public void AddRewardEquipItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < equipItemsHeld.Length; i++)
        {
            if (equipItemsHeld[i] == "") //|| equipItemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = equipItemsHeld.Length;
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for (int i = 0; i < existingItems.Length; i++)
            {
                if (existingItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = existingItems.Length;
                }
            }

            if (itemExists)
            {
                equipItemsHeld[newItemPosition] = itemToAdd;
                //numberOfEquipItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " Does Not Exist!!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    //A method for removing items after usage
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        if (GameMenu.instance.activeItem.item || GameMenu.instance.activeItem.battleStatusModifier)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = itemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfItems[itemPosition]--;
                itemsHeld[itemPosition] = "";
                //if (numberOfItems[itemPosition] <= 0)
                //{
                    //itemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }

        if (GameMenu.instance.activeItem.defense || GameMenu.instance.activeItem.offense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = equipItemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfEquipItems[itemPosition]--;
                equipItemsHeld[itemPosition] = "";
                //if (numberOfEquipItems[itemPosition] <= 0)
                //{
                //equipItemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }
    }

    //A method for removing a sold item
    public void RemoveSoldItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        if (Shop.instance.selectedItem.item)
        {
            for (int i = 0; i < itemsHeld.Length; i++)
            {
                if (itemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = itemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfItems[itemPosition]--;

                //if (numberOfItems[itemPosition] <= 0)
                //{
                    itemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }

        if (Shop.instance.selectedItem.offense || Shop.instance.selectedItem.defense)
        {
            for (int i = 0; i < equipItemsHeld.Length; i++)
            {
                if (equipItemsHeld[i] == itemToRemove)
                {
                    foundItem = true;
                    itemPosition = i;

                    i = equipItemsHeld.Length;
                }
            }

            if (foundItem)
            {
                //numberOfEquipItems[itemPosition]--;

                //if (numberOfEquipItems[itemPosition] <= 0)
                //{
                    equipItemsHeld[itemPosition] = "";
                //}

                GameMenu.instance.ShowItems();
                GameMenu.instance.ShowEquipItems();
            }
            else
            {

                Debug.LogError("Couldn't find " + itemToRemove);
            }
        }
    }

    // Save game data to the specified slot
    public void SaveData(int saveSlot)
    {
        // Save the current scene name to a slot-specific key
        string sceneKey = $"Current_Scene_Slot{saveSlot}";
        PlayerPrefs.SetString(sceneKey, SceneManager.GetActiveScene().name);

        // Save the player's position to slot-specific keys
        PlayerPrefs.SetFloat($"Player_Position_x_Slot{saveSlot}", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat($"Player_Position_y_Slot{saveSlot}", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat($"Player_Position_z_Slot{saveSlot}", PlayerController.instance.transform.position.z);

        // Save character stats
        for (int i = 0; i < characterStatus.Count; i++)
        {
            string characterKey = $"Player_{characterStatus[i].characterName}_Slot{saveSlot}";

            PlayerPrefs.SetInt($"{characterKey}_active", characterStatus[i].gameObject.activeInHierarchy ? 1 : 0);
            PlayerPrefs.SetInt($"{characterKey}_Level", characterStatus[i].level);
            PlayerPrefs.SetInt($"{characterKey}_CurrentExp", characterStatus[i].currentEXP);
            PlayerPrefs.SetInt($"{characterKey}_CurrentHP", characterStatus[i].currentHP);
            PlayerPrefs.SetInt($"{characterKey}_MaxHP", characterStatus[i].maxHP);
            PlayerPrefs.SetInt($"{characterKey}_CurrentMP", characterStatus[i].currentSP);
            PlayerPrefs.SetInt($"{characterKey}_MaxMP", characterStatus[i].maxSP);
            PlayerPrefs.SetInt($"{characterKey}_Strength", characterStatus[i].strength);
            PlayerPrefs.SetInt($"{characterKey}_Defence", characterStatus[i].defence);
            PlayerPrefs.SetInt($"{characterKey}_WpnPwr", characterStatus[i].offenseStrength);
            PlayerPrefs.SetInt($"{characterKey}_ArmrPwr", characterStatus[i].defenseStrength);
            PlayerPrefs.SetString($"{characterKey}_EquippedWpn", characterStatus[i].equippedOffenseItem);
            PlayerPrefs.SetString($"{characterKey}_EquippedArmr", characterStatus[i].equippedDefenseItem);

            // Save skills
            PlayerPrefs.SetInt($"{characterKey}_SkillCount", characterStatus[i].skills.Length);
            for (int j = 0; j < characterStatus[i].skills.Length; j++)
            {
                if (characterStatus[i].skills[j] != null)
                {
                    PlayerPrefs.SetString($"{characterKey}_Skill_{j}", characterStatus[i].skills[j].skill.skillName);
                }
            }
        }

        // Save inventory items and equipment to slot-specific keys
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString($"ItemInInventory_Slot{saveSlot}_{i}", itemsHeld[i]);
            PlayerPrefs.SetString($"EquipItemInInventory_Slot{saveSlot}_{i}", equipItemsHeld[i]);
        }

        // Save gold to a slot-specific key
        PlayerPrefs.SetInt($"Gold_Slot{saveSlot}", currentGold);

        Debug.Log($"Game saved to slot {saveSlot}");
    }


    // Load game data from the specified slot
    public void LoadData(int saveSlot)
    {
        // Load the current scene name for the specified slot
        string sceneKey = $"Current_Scene_Slot{saveSlot}";
        string sceneToLoad = PlayerPrefs.GetString(sceneKey, "");

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError($"No scene name found for the given save slot {saveSlot}. Make sure the game was saved properly.");
            return;
        }

        // Load the player's position from slot-specific keys
        PlayerController.instance.transform.position = new Vector3(
            PlayerPrefs.GetFloat($"Player_Position_x_Slot{saveSlot}"),
            PlayerPrefs.GetFloat($"Player_Position_y_Slot{saveSlot}"),
            PlayerPrefs.GetFloat($"Player_Position_z_Slot{saveSlot}")
        );

        // Load character stats
        for (int i = 0; i < characterStatus.Count; i++)
        {
            string characterKey = $"Player_{characterStatus[i].characterName}_Slot{saveSlot}";

            characterStatus[i].gameObject.SetActive(PlayerPrefs.GetInt($"{characterKey}_active", 0) == 1);
            characterStatus[i].level = PlayerPrefs.GetInt($"{characterKey}_Level");
            characterStatus[i].currentEXP = PlayerPrefs.GetInt($"{characterKey}_CurrentExp");
            characterStatus[i].currentHP = PlayerPrefs.GetInt($"{characterKey}_CurrentHP");
            characterStatus[i].maxHP = PlayerPrefs.GetInt($"{characterKey}_MaxHP");
            characterStatus[i].currentSP = PlayerPrefs.GetInt($"{characterKey}_CurrentMP");
            characterStatus[i].maxSP = PlayerPrefs.GetInt($"{characterKey}_MaxMP");
            characterStatus[i].strength = PlayerPrefs.GetInt($"{characterKey}_Strength");
            characterStatus[i].defence = PlayerPrefs.GetInt($"{characterKey}_Defence");
            characterStatus[i].offenseStrength = PlayerPrefs.GetInt($"{characterKey}_WpnPwr");
            characterStatus[i].defenseStrength = PlayerPrefs.GetInt($"{characterKey}_ArmrPwr");
            characterStatus[i].equippedOffenseItem = PlayerPrefs.GetString($"{characterKey}_EquippedWpn");
            characterStatus[i].equippedDefenseItem = PlayerPrefs.GetString($"{characterKey}_EquippedArmr");

            // Load skills
            int skillCount = PlayerPrefs.GetInt($"{characterKey}_SkillCount");
            System.Array.Resize(ref characterStatus[i].skills, skillCount);

            for (int j = 0; j < skillCount; j++)
            {
                string skillName = PlayerPrefs.GetString($"{characterKey}_Skill_{j}");
                for (int k = 0; k < BattleManager.instance.skillList.Length; k++)
                {
                    if (BattleManager.instance.skillList[k].skill.skillName == skillName)
                    {
                        characterStatus[i].skills[j] = BattleManager.instance.skillList[k];
                    }
                }
            }
        }

        // Load inventory items and equipment from slot-specific keys
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString($"ItemInInventory_Slot{saveSlot}_{i}");
            equipItemsHeld[i] = PlayerPrefs.GetString($"EquipItemInInventory_Slot{saveSlot}_{i}");
        }

        // Load gold from a slot-specific key
        currentGold = PlayerPrefs.GetInt($"Gold_Slot{saveSlot}");

        Debug.Log($"Game loaded from slot {saveSlot}");
    }


    //Check if inventory contains a specific item
    public bool HasItem(string searchItem)
    {
        for (int i = 0; i < itemsHeld.Length - 1; i++)
        {
            if (itemsHeld[i] == searchItem)
            {
                return true;
            }
        }

        for (int i = 0; i < equipItemsHeld.Length - 1; i++)
        {
            if (equipItemsHeld[i] == searchItem)
            {
                return true;
            }
        }
 
        return false;

    }
}
