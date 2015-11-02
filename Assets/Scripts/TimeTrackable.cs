using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TimeTrackable : MonoBehaviour {

    public string path;
    public static TimeTrackable current;

    public int StackSize = 0;
    public float TimeMultiplier = 1f;

    // Needs its own Time.time
    private float PrivateTime;

    // If true, each frame which influences the object, will be tracked.
    public bool tracking;
    public Stack<TrackFragment> queue;
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
            StackSize = queue.Count;

            transform.position = (Vector3)_lastFragment.pos;
            transform.rotation = _lastFragment.rotation;
        }
    }

    void Start () {
        if(Application.platform == RuntimePlatform.Android)
            path = "jar:file://" + Application.dataPath + "!/assets/";
        else
            path = Application.dataPath + "/Scenes/MhTest/WallCrumble/Recording/";
        _body = GetComponent<Rigidbody>();
        queue = new Stack<TrackFragment>();
        if (File.Exists(path + this.name + ".dat"))
        {
            Load();
        }
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

        // Now we reverse everything.

        // If the stack is empty, return.
        if (queue.Count == 0)
        {
            return;
        }

        // Turn OFF unity's gravity, we're in control now.
        _body.isKinematic = true;
        // Keep track of the rewind time.
        _reversedTime += deltaTime;

        // See if there's a keyframe for the curret rewind period.
        if (freezeTime - _reversedTime <= queue.Peek().time)
        {
            // Rewind the frame.
            _lastFragment = queue.Pop();
            StackSize = queue.Count;

            transform.position = _lastFragment.pos;
            transform.rotation = (Quaternion)_lastFragment.rotation;
        }
    }

    // Start rewinding.
    public void Reverse()
    {
        if (tracking)
        {
            freezeTime = PrivateTime;
            _reversedTime = 0;
            tracking = false;
        }
    }

    // Start recording.
    public void Record()
    {
        tracking = true;
    }
}

[System.Serializable]
class TimeData
{
    public Stack<TrackFragment> queue;
}
