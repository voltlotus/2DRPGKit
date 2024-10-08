using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Make instance of this script to be able to reference from other scripts!
    public static EventManager instance;

    [Header("Event Settings")]
    public string[] events;
    public bool[] completedEvents;

    // Use this for initialization
    void Start()
    {
        instance = this;

        completedEvents = new bool[events.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get the number of an event
    public int GetEventNumber(string eventToFind)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i] == eventToFind)
            {
                return i;
            }
        }

        Debug.LogError("Event " + eventToFind + " does not exist");
        return 0;
    }

    // Check if an event was completed
    public bool CheckIfComplete(string eventToCheck)
    {
        if (GetEventNumber(eventToCheck) != 0)
        {
            return completedEvents[GetEventNumber(eventToCheck)];
        }

        return false;
    }

    // Complete an event
    public void MarkEventComplete(string eventToMark)
    {
        completedEvents[GetEventNumber(eventToMark)] = true;
        UpdateLocalEventObjects();
    }

    // Mark an event as incomplete
    public void MarkEventIncomplete(string questToMark)
    {
        completedEvents[GetEventNumber(questToMark)] = false;
        UpdateLocalEventObjects();
    }

    // Update game objects associated with an event
    public void UpdateLocalEventObjects()
    {
        EventObjectActivator[] eventObjects = FindObjectsOfType<EventObjectActivator>();

        if (eventObjects.Length > 0)
        {
            for (int i = 0; i < eventObjects.Length; i++)
            {
                eventObjects[i].CheckCompletion();
            }
        }
    }

    // Save event data to a specified slot
    public void SaveEventData(int saveSlot)
    {
        // Define a prefix based on the save slot number
        string slotPrefix = "SaveSlot_" + saveSlot + "_";

        for (int i = 0; i < events.Length; i++)
        {
            if (completedEvents[i])
            {
                PlayerPrefs.SetInt(slotPrefix + "EventMarker_" + events[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt(slotPrefix + "EventMarker_" + events[i], 0);
            }
        }
    }

    // Load event data from a specified slot
    public void LoadEventData(int saveSlot)
    {
        // Define a prefix based on the save slot number
        string slotPrefix = "SaveSlot_" + saveSlot + "_";

        for (int i = 0; i < events.Length; i++)
        {
            int valueToSet = 0;
            if (PlayerPrefs.HasKey(slotPrefix + "EventMarker_" + events[i]))
            {
                valueToSet = PlayerPrefs.GetInt(slotPrefix + "EventMarker_" + events[i]);
            }

            if (valueToSet == 0)
            {
                completedEvents[i] = false;
            }
            else
            {
                completedEvents[i] = true;
            }
        }
    }
}
