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
    public string lastLevel;
    Clockpart[] cogs;


    void Awake()
    {
        _SaveInterval = SaveInterval;
        SaveInterval = 0f;
    }

    // When the script is loaded for the first time, it will load progress from last time.
    void OnEnable()
    {
        if (saveLoad == null)
        {
            DontDestroyOnLoad(gameObject);
            saveLoad = this;
        }
        else if (saveLoad != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Prepare()
    {
        _lvl = Application.loadedLevel;
        _TimeControllers = GameObject.FindObjectsOfType<ObjectTimeController>();
        _SaveData = new List<SaveState>();
        // Load on enable?
        inv = FindObjectOfType<CharacterInventory>();
        cogs = GameObject.FindObjectsOfType<Clockpart>();
        Load();
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
            return;
        PlayerPrefs.SetString("LastLevel", Application.loadedLevelName);
        if (Application.loadedLevelName == "Main Menu")
            SaveLoad.saveLoad.SaveInterval = 0f;
        else
        {
            SaveLoad.saveLoad.SaveInterval = _SaveInterval;
            Prepare();
        }
    }

    void OnApplicationQuit()
    {
        // If there's no save interval, we're giving input on when to save, this also allows us to reset the savedata.
        if(SaveInterval != 0)
            Save();
    }

    public void Save()
    {
        // Add all the TimePos data for the objects we need to save
        foreach (ObjectTimeController TimeController in _TimeControllers)
        {
            _SaveData.Add(new SaveState(TimeController.name, TimeController.TimePos));
        }
        // Prepare a new string for all the pickupitems' names.
        List<string> pickups = new List<string>();
        foreach (GameObject clock in inv.clockParts)
        {
            if(clock != null)
                if (!pickups.Contains(clock.name))
                {
                    pickups.Add(clock.name);
                }
        }
        // Open new BinaryFormatter, with a filename depending on the level we're playing.
        BinaryFormatter bf = new BinaryFormatter();
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
        if(_SaveData.Count > 0)
            _SaveData.Clear();
        List<string> taggedclocks = new List<string>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save" + Application.loadedLevel + ".save", FileMode.Create);
        bf.Serialize(file, new SaveData(_SaveData, taggedclocks));
        file.Close();
    }


    void Update()
    {
        // If there's no automatic saving, we save on keyinput instead.
        if (SaveInterval == 0)
        {
            if (Input.anyKey)
            {
                if (Input.GetKeyDown(KeyCode.S)) { Save(); }
                if (Input.GetKeyDown(KeyCode.L)) { Load(); }
                if (Input.GetKeyDown(KeyCode.R)) { Reset(); }
            }
            return;
        }

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
        int q = Application.levelCount;

        for (int i = 0; i < q; i++)
            {
            if (File.Exists(Application.persistentDataPath + "/save" + i + ".save")){
                File.Delete(Application.persistentDataPath + "/save" + i + ".save");
                }
            }

    }

    public void ResetFrom(int level)
    {
        int q = Application.levelCount;

        for (int i = level-1; i < q; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/save" + i + ".save"))
            {
                File.Delete(Application.persistentDataPath + "/save" + i + ".save");
            }
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

// Helper class for the pilars
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