using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TimeTrackable : MonoBehaviour {

    public string path;
    public static TimeTrackable current;
    public bool forward;
    public bool frozen = false;
    public int StackSize = 0;
    public float TimeMultiplier = 1f;

    // Needs its own Time.time
    private float PrivateTime;

    // If true, each frame which influences the object, will be tracked.
    public bool tracking;
    public Stack<TrackFragment> queue;
    public Stack<TrackFragment> backup_queue;
    public float _reversedTime = 0;

    private TrackFragment _lastFragment;
    private Rigidbody _body;
    // Helper value for when reversing time.
    private float freezeTime = 0;

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path + this.name + ".dat");
        TimeData data = new TimeData();
        data.queue = this.queue;
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        tracking = false;
        if (File.Exists(path + this.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path + this.name + ".dat", FileMode.Open);
            TimeData data = (TimeData)bf.Deserialize(file);
            file.Close();
            queue = data.queue;
            if (queue.Count == 0)
                return;
            // Rewind the frame.
            _lastFragment = queue.Pop();
            backup_queue.Push(_lastFragment);
            StackSize = queue.Count;

            transform.position = (Vector3)_lastFragment.pos;
            transform.rotation = _lastFragment.rotation;
        }
    }

    void Awake () {
        //        if(Application.platform == RuntimePlatform.Android)
        //          path = "jar:file://" + Application.dataPath + "!/assets/";
        //    else
        path = Application.persistentDataPath + "/Wall"+transform.parent.name;
       //         path = Application.dataPath + "/Scenes/MhTest/WallCrumble/Recording/";
        _body = GetComponent<Rigidbody>();
        queue = new Stack<TrackFragment>();
        backup_queue = new Stack<TrackFragment>();
    }

    public void Initialize()
    {
        // Get the 'original' frame.
        TrackFragment fragment = new TrackFragment();
        fragment.pos = transform.position;
        fragment.time = PrivateTime;
        fragment.rotation = transform.rotation;
        // Add it to the stack.
        queue.Push(fragment);
        StackSize = queue.Count;

    }

    void Update () {
        float deltaTime = Time.deltaTime * TimeMultiplier;
        // If we're recording the object.
        if (tracking)
        {
            // If the object didn't move, we dont care.
            if(queue.Count > 0)
                if (transform.position == queue.Peek().pos)
                    return;
            PrivateTime += Time.deltaTime;
            TrackFragment fragment = new TrackFragment();
            fragment.pos = (SerializableVector3)transform.position;
            fragment.time = PrivateTime;
            fragment.rotation = (SerializableQuaternion)transform.rotation;
            queue.Push(fragment);
            StackSize = queue.Count;
            return;
        }

        if (frozen)
            return;
        // Now we reverse everything.

        // If the stack is empty, return.
        if (queue.Count == 0 && backup_queue.Count == 0)
        {
            return;
        }
        // Turn OFF unity's gravity, we're in control now.
        _body.isKinematic = true;
        // Keep track of the rewind time.
        _reversedTime += deltaTime;

        if (forward)
        {
            // See if there's a keyframe for the curret rewind period.
            if (queue.Count > 0 && freezeTime - _reversedTime <= queue.Peek().time)
            {
                // Rewind the frame.
                _lastFragment = queue.Pop();
                backup_queue.Push(_lastFragment);
                StackSize = queue.Count;

                transform.position = _lastFragment.pos;
                transform.rotation = (Quaternion)_lastFragment.rotation;
            }
            return;
        }

        // See if there's a keyframe for the curret rewind period.
        if (backup_queue.Count > 0 && freezeTime - _reversedTime <= backup_queue.Peek().time)
        {
            // Rewind the frame.
            _lastFragment = backup_queue.Pop();
            queue.Push(_lastFragment);
            StackSize = backup_queue.Count;

            transform.position = _lastFragment.pos;
            transform.rotation = (Quaternion)_lastFragment.rotation;
        }

    }

    // Start rewinding.
    public void Move()
    { 
        freezeTime = PrivateTime;
        _reversedTime = 0;
        tracking = false;
    }

    // Start recording.
    public void Record()
    {
        File.Delete(path + this.name + ".dat");
        tracking = true;
        queue = new Stack<TrackFragment>();
        backup_queue = new Stack<TrackFragment>();
        Initialize();

    }
}

[System.Serializable]
class TimeData
{
    public Stack<TrackFragment> queue;
}