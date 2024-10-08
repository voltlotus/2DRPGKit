using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [Header("Load Settings")]
    public float waitToLoad;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                // Use the loadedSaveId from GameManager to determine which slot to load
                int saveSlot = GameManager.instance.loadedSaveId;
                string sceneToLoad = PlayerPrefs.GetString($"Current_Scene_Slot{saveSlot}", "");

                if (string.IsNullOrEmpty(sceneToLoad))
                {
                    Debug.LogError($"Cannot load scene: No scene name found in PlayerPrefs for 'Current_Scene_Slot{saveSlot}'. Make sure the scene name is saved correctly.");
                    return;
                }

                GameManager.instance.cutSceneActive = false;

                // Load the scene if the name is valid
                SceneManager.LoadScene(sceneToLoad);

                // Load other game data
                GameManager.instance.LoadData(saveSlot);
                QuestManager.instance.LoadQuestData(saveSlot);
                ChestManager.instance.LoadChestData(saveSlot);
                EventManager.instance.LoadEventData(saveSlot);
            }
        }
    }
}
