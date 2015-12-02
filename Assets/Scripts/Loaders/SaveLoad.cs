using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {

    // We need some sort of persistant identifier for objects in the game
    // Unity sux, so we are forced to use the object names.
    // So far this script can save and load the state of pickupitems and the mechanim pilars.

    // There should only be one of this object in the game.
    public static SaveLoad saveLoad;

    [Range(0, 100)]
    public float SaveInterval = 2;
    private float _SaveInterval;
    private float elapsedtime;

    [Header("Debuggin")]

    public ObjectTimeController[] _TimeControllers;
    private List<SaveState> _SaveData;
    public int _lvl = -1;

    public CharacterInventory inv;
    Clockpart[] cogs;


    void Awake()
    {
        _SaveInterval = SaveInterval;
        SaveInterval = 0f;
    }

    // When the script is loaded for the first time, it will load progress from last time.
    void OnEnable()
    {
        // there can only be one, so if there's not one, this will be the one
        if (saveLoad == null)
        {
            // this will persist between levels.
            DontDestroyOnLoad(gameObject);
            saveLoad = this;
        }
        // there's already a saveload script, so we destroy this one and leave.
        else if (saveLoad != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Pepares to save and load levels, by finding all objects of intreset, and clear the lists for storing data.
    void Prepare(bool AndLoad)
    {
        // The level number
        _lvl = Application.loadedLevel;
        // The ObjectTimeControllers, we are interested in their float values.
        _TimeControllers = GameObject.FindObjectsOfType<ObjectTimeController>();
        // The datastructre we save is defined in this class
        _SaveData = new List<SaveState>();
        // The inventory of the player, in case a piece is already looted.
        inv = FindObjectOfType<CharacterInventory>();
        // The pieces in the level, in case they are already looted.
        cogs = GameObject.FindObjectsOfType<Clockpart>();
        // We load the data for the current level
        if(AndLoad)
            Load();
    }

    // When loading a level, we check if it's a special case level, like the intro or the main menu, in which case we do not save.
    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
            return;
        PlayerPrefs.SetInt("LastLevel", Application.loadedLevel);
        // 4 = Intro, 3 = Main menu, 10 = Ending, if the build order changes, this also needs to be adjusted, alternative is by name, but they are just as liekly to change or be misspelled.
        // 6 & 8 are cutscenes.
		if (Application.loadedLevel == 3 || Application.loadedLevel == 4 || Application.loadedLevel == 10)// || Application.loadedLevel == 6 || Application.loadedLevel == 8)
            // We simple set the interval for saving to 0, and we do not save.
            SaveLoad.saveLoad.SaveInterval = 0f;
        else
        {
            PlayerPrefs.SetInt("Playing" + level, 1);
            // We set the interval for the saves to the initially set value, 2 sec by default
            SaveLoad.saveLoad.SaveInterval = _SaveInterval;
            // We Prepare a level
            if (Application.loadedLevel == 5 && PlayerPrefs.GetInt(TutorialController.PlayerPrefAlreadySeen) == 1)
            {
                Prepare(false);
                return;
            }
            Prepare(true);
        }
    }

    // If the application is trying to shut down, we save the game state and allow it to continue to shut down.
    void OnApplicationQuit()
    {
        // If SaveInterval is zero, we are do not care to save here either as it will very likely cause problems
        if(SaveInterval != 0)
            Save();
    }

    // Function for saving the game, public in order to be triggered by game events.
    public void Save()
    {
        // Add all the float TimePos data for the objects we need to save
        foreach (ObjectTimeController TimeController in _TimeControllers)
        {
            // We identify the ObjectTimeController object by name, so changing the names or have them share a name is ill adviced.
            _SaveData.Add(new SaveState(TimeController.name, TimeController.TimePos));
        }
        // Prepare a new string for all the pickupitems' names.
        List<string> pickups = new List<string>();
        foreach (GameObject clock in inv.clockParts)
        {
            // If the inventory is empty, we do not save the info
            if(clock != null)
                if (!pickups.Contains(clock.name))
                {
                    pickups.Add(clock.name);
                }
        }
        // Open new BinaryFormatter, with a filename depending on the level we're playing.
        BinaryFormatter bf = new BinaryFormatter();
        // Every level has its own .save file
        FileStream file = File.Open(Application.persistentDataPath + "/save" + Application.loadedLevel + ".save", FileMode.Create);
        // Save the data to the file.
        bf.Serialize(file, new SaveData(_SaveData, pickups));
        file.Close();

        // Clears data for next save
        _SaveData.Clear();
    }

    public void Load()
    {
        // Reverse from Save, now we load.
        BinaryFormatter bf = new BinaryFormatter();
        // If there's a file, we can continnue to load, this is not always the case, for example when we prepare a new game or after reset, thus we just skip loading.
        if(File.Exists(Application.persistentDataPath + "/save" + Application.loadedLevel + ".save"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save" + Application.loadedLevel + ".save", FileMode.Open);
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();
            // Restore the data of the pilars.
            foreach (SaveState state in data.data)
            {
                foreach (ObjectTimeController controller in _TimeControllers)
                    // Sadly we have to identify by name.
                    if (controller.name == state.name)
                        controller.TimePos = state.pos;
            }
            // Restore the data for pickups
            foreach(string Cog in data.inv)
            {
                foreach (Clockpart clock in cogs)
                {
                    if (clock.name == Cog)
                    {
                        // Copy the functionality of the Cogs and Inventory, should most likely be a localiced helperfunction in those scripts instead.
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        clock.transform.parent = player.transform;
                        clock.pickedUp = true;
                        clock.GetComponent<Collider>().enabled = false;
                        inv.AddClockPart(clock.gameObject);
                    }
                }
            }
        }
    }

    // Instead of deleting the file, we can simply overwrite it with blank data
    public void Reset()
    {
        PlayerPrefs.GetInt("Playing" + Application.loadedLevel, 0);
        // If there's data, we clear it out
        if (_SaveData.Count > 0)
            _SaveData.Clear();
        List<string> taggedclocks = new List<string>();
        BinaryFormatter bf = new BinaryFormatter();
        // we overwrite the .save file, with our empty information effectively resetting its data
        FileStream file = File.Open(Application.persistentDataPath + "/save" + Application.loadedLevel + ".save", FileMode.Create);
        bf.Serialize(file, new SaveData(_SaveData, taggedclocks));
        file.Close();
    }


    void Update()
    {
        if (SaveInterval == 0)
            return;
        // Save on fixed time interval.
        if (elapsedtime < SaveInterval)
        {
            elapsedtime += Time.deltaTime;
            return;
        }
        Save();
        elapsedtime = 0;
    }

    public void ResetEverything()
    {
        // If we are Resetting Everything we delete all save files, couldn't be easier.
        int q = Application.levelCount;

        for (int i = 0; i < q; i++)
            {
            if (File.Exists(Application.persistentDataPath + "/save" + i + ".save")){
                File.Delete(Application.persistentDataPath + "/save" + i + ".save");
                }
            }
    }

    // Allows to reset from a certain level, deleting all savedata beyond this point
    public void ResetFrom(int level)
    {
        int q = Application.levelCount;

        for (int i = level; i < q; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/save" + i + ".save"))
            {
                File.Delete(Application.persistentDataPath + "/save" + i + ".save");
            }
        }
    }

    public void ResetLevel(int level)
    {
        if (File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
        {
            File.Delete(Application.persistentDataPath + "/save" + level + ".save");
        }
    }

}

// SaveData is the data which is being serializd, it needs to hold all information which we store in a load, so expand this as needed.
[Serializable]
class SaveData
{
    public List<SaveState> data;
    public List<string> inv;
    public SaveData(List<SaveState> data, List<string> inv)
    {
        this.data = data;
        this.inv = inv;
    }
}

// Helper class for the pillars
[Serializable]
class SaveState
{
    public string name;
    public float pos;
    public SaveState(string name, float pos)
    {
        this.name = name;
        this.pos = pos;
    }
}