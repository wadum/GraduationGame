using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {

    // There should only be one of this object in the game.
    public static SaveLoad saveLoad;

    [Range(0, 100)]
    public float SaveInterval = 0;
    private float elapsedtime;

    [Header("Debuggin")]
    public string levelname = "null";

    public ObjectTimeController[] _TimeControllers;
    private List<SaveState> _SaveData;

    CharacterInventory inv;
    Cockpart[] COCKS;

    void Awake()
    {
        levelname = Application.loadedLevelName;
        if (saveLoad == null)
        {
            DontDestroyOnLoad(gameObject);
            saveLoad = this;
        }
        else if(saveLoad != this)
        {
            Destroy(this);
        }
        _TimeControllers = GameObject.FindObjectsOfType<ObjectTimeController>();
        _SaveData = new List<SaveState>();
    }

    void Start()
    {
        inv = FindObjectOfType<CharacterInventory>();
    }

    void OnEnable()
    {
        // Save on enable?
        inv = FindObjectOfType<CharacterInventory>();
        COCKS = GameObject.FindObjectsOfType<Cockpart>();
        Load();
    }

    void OnDisable()
    {

    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        foreach (ObjectTimeController TimeController in _TimeControllers)
        {
            _SaveData.Add(new SaveState(TimeController.name, TimeController.TimePos));
        }
        List<int> taggedcocks = new List<int>();
        foreach (GameObject cock in inv.clockParts)
        {
            taggedcocks.Add(cock.GetInstanceID());
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save" + levelname + ".save", FileMode.Create);
        bf.Serialize(file, new SaveData(_SaveData, taggedcocks));
        file.Close();
        _SaveData.Clear();
        Debug.Log("saved");
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/save" + levelname + ".save"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save" + levelname + ".save", FileMode.Open);
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();
            List<int> taggedcocks = new List<int>();
            _SaveData = data.data;
            taggedcocks = data.inv;
            foreach (SaveState state in _SaveData)
            {
                foreach (ObjectTimeController controller in _TimeControllers)
                    if (controller.name == state.name)
                        controller.TimePos = state.pos;
            }
            Debug.Log(taggedcocks.Count + " " + COCKS.Length);
            foreach(int ID in taggedcocks)
            {
                Debug.Log("ID: " + ID);
                foreach (Cockpart cock in COCKS)
                {
                    Debug.Log("COCKS: " + cock.GetInstanceID());
                    if (cock.GetInstanceID() == ID)
                    {
                        Debug.Log(cock + " " + ID);
                        return;
        
                    }
                }
            }
        }
    }

    void Update()
    {
        // If there's no automatic saving, we save on keyinput instead.
        if (SaveInterval == 0)
        {
            if (Input.anyKey)
            {
                if (Input.GetKeyDown(KeyCode.A)) { Save(); }
                if (Input.GetKeyDown(KeyCode.D)) { Load(); }
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
}

[Serializable]
class SaveData
{
    public List<SaveState> data;
    public List<int> inv;
    public SaveData(List<SaveState> data, List<int> inv)
    {
        this.data = data;
        this.inv = inv;
    }
}

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

[Serializable]
class SaveInventory
{
    public int ID;
}