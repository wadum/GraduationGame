using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class TimeTrackable : MonoBehaviour {

    [System.Serializable]
    class TimeData : ScriptableObject
    {
        public List<TrackFragment> queue;
    }
    [SerializeField]
    public int index = -1;
//    public string path;
    public static TimeTrackable current;
    public bool forward;
    public bool frozen = false;
    public float TimeMultiplier = 1f;

    // Needs its own Time.time
//    private float PrivateTime;

    // If true, each frame which influences the object, will be tracked.
    public bool tracking;

    [SerializeField]
    public List<TrackFragment> queue;
    public float _reversedTime = 0;

    private TrackFragment _lastFragment;
    private Rigidbody _body;
    // Helper value for when reversing time.
    private float freezeTime = 0;

    void Awake () {
        _body = GetComponent<Rigidbody>();
        if(queue == null)
            queue = new List<TrackFragment>();
    }

    public void Initialize()
    {
        // Get the 'original' frame.
        TrackFragment fragment = new TrackFragment();
        fragment.pos = transform.localPosition;
        fragment.time = RecordMaster.time;
        fragment.rotation = transform.localRotation;
        // Add it to the stack.
        queue.Add(fragment);
    }

    void Update () {
//        float deltaTime = Time.deltaTime * TimeMultiplier;
        // If we're recording the object.
        if (tracking)
        {
            // If the object didn't move, we dont care.
            if(queue.Count > 0)
                if (transform.localPosition == queue[index].pos)// queue[queue.Count - 1].pos)
                    return;
//            PrivateTime = RecordMaster.time;
//            PrivateTime += Time.deltaTime;
            TrackFragment fragment = new TrackFragment();
            fragment.pos = (SerializableVector3)transform.localPosition;
            fragment.time = RecordMaster.time;
            fragment.rotation = (SerializableQuaternion)transform.localRotation;
            queue.Add(fragment);
            index += 1;
            return;
        }

        if (frozen || queue.Count == 0)
            return;

        // Turn OFF unity's gravity, we're in control now.
        _body.isKinematic = true;
        // Keep track of the rewind time.
    //    Debug.Log(RecordMaster.time);
        if(RecordMaster.time + 0.2f <= queue[index].time)
        {
            if (index > 0)
                index -= 1;

            // Rewind the frame.
            _lastFragment = queue[index];

            transform.localPosition = _lastFragment.pos;
            transform.localRotation = (Quaternion)_lastFragment.rotation;

        }
        else if(RecordMaster.time >= queue[index].time)
        {
            if (queue.Count > index + 1)
                index += 1;
            // Rewind the frame.
            _lastFragment = queue[index];

            transform.localPosition = _lastFragment.pos;
            transform.localRotation = (Quaternion)_lastFragment.rotation;
        }


        /*
                if (forward)
                {
                    if (index <= 0 )
                        return;

                    _reversedTime += deltaTime;

                    // See if there's a keyframe for the curret rewind period.
                    if (freezeTime - _reversedTime <= queue[index-1].time)
                    {
                        index -= 1;
                        // Rewind the frame.
                        _lastFragment = queue[index];

                        transform.localPosition = _lastFragment.pos;
                        transform.localRotation = (Quaternion)_lastFragment.rotation;
                    }
                    return;
                }
                else
                {
                    if (index >= queue.Count -1)
                        return;

                    _reversedTime += deltaTime;

                    // See if there's a keyframe for the curret rewind period.
                    if (_reversedTime > queue[index].time)
                    {
                        index += 1;
                        // Rewind the frame.
                        _lastFragment = queue[index];

                        transform.localPosition = _lastFragment.pos;
                        transform.localRotation = (Quaternion)_lastFragment.rotation;
                    }
                    return;
                }*/
    }

    // Start rewinding.
    public void Move()
    { 
        freezeTime = RecordMaster.time;
        _reversedTime = 0;
        tracking = false;
    }

    // Start recording.
    public void Record()
    {
/*        if (File.Exists(path + this.name + ".dat"))
        {
            File.Delete(path + this.name + ".dat");
        }*/
        tracking = true;
        _body.isKinematic = false;
        queue = new List<TrackFragment>();
        Initialize();
    }
}