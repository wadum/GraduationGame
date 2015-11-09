using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class TimeTrackable : MonoBehaviour {

    [System.Serializable]
    public class TrackFragment
    {
        public Vector3 pos;
        public float time;
        public Quaternion rotation;

        public TrackFragment(Vector3 pos, Quaternion rotation, float time)
        {
            this.pos = pos;
            this.rotation = rotation;
            this.time = time;
        }
    }

    [SerializeField]
    public int index = -1;

    // If true, each frame which influences the object, will be tracked.
    public bool tracking = false;

    [SerializeField]
    public List<TrackFragment> queue;

    private TrackFragment _lastFragment;
    private Rigidbody _body;

    void Awake () {
        _body = GetComponent<Rigidbody>();
        if(queue == null)
            queue = new List<TrackFragment>();
    }

    void Start()
    {
        Material material = (Material)Resources.Load("Materials/INTERACTWITHME", typeof(Material));
        Renderer rend = GetComponent<Renderer>();
        rend.material = material;
    }

    public void Initialize()
    {
        // Get the 'original' frame.
        // Add it to the stack.
        queue.Add(new TrackFragment(transform.localPosition, transform.localRotation, RecordMaster.time));
        index += 1;
    }

    void Update () {
        // If game is paused, so are we
        if (Time.timeScale == 0)
            return;

        // If we're recording the object.
        if (tracking)
        {
            // If the object didn't move, we dont care.
            if(queue.Count > 0)
                if (transform.localPosition == queue[index].pos && transform.localRotation == queue[index].rotation)
                    return;
            queue.Add(new TrackFragment(transform.localPosition, transform.localRotation, RecordMaster.time));
            index += 1;
            return;
        }

        if (queue.Count == 0)
            return;

        // Turn OFF unity's gravity, we're in control now.
        MoveMe();
    }

    void MoveMe()
    {
        _body.isKinematic = true;
        // If index is ahead of the RecordMaster, move back in list.
        if (RecordMaster.time <= queue[index].time)
        {
            if (index > 0)
                index -= 1;
            _lastFragment = queue[index];
            transform.localPosition = _lastFragment.pos;
            transform.localRotation = _lastFragment.rotation;
        }
        // If index is behind of the RecordMaster, move it forward, but only if the timegap is significant
        // In order to avoid "shaking" between indexes.
        else if (RecordMaster.time - 0.2f >= queue[index].time)
        {
            if (queue.Count > index + 1)
                index += 1;
            _lastFragment = queue[index];
            transform.localPosition = _lastFragment.pos;
            transform.localRotation = _lastFragment.rotation;
        }

    }

    // Start rewinding.
    public void Move()
    { 
        tracking = false;
    }

    // Start recording.
    public void Record()
    {
        tracking = true;
        _body.isKinematic = false;
        queue = new List<TrackFragment>();
        index = -1;
        Initialize();
    }

    // I'm an idiot atm, wont be a feature until I can think straight.
/*    public void MoveInstantly()
    {
        if (RecordMaster.time <= queue[index].time)
            while (RecordMaster.time <= queue[index].time)
                MoveMe();
                else if (RecordMaster.time - 0.2f >= queue[index].time)
                    while (RecordMaster.time - 0.2f >= queue[index].time)
                        MoveMe();
            
    }*/
}