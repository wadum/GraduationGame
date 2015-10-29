using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeTrackable : MonoBehaviour {

    public struct TrackFragment{
        public Vector3 pos;
        public float time;
        public Vector3 velocity;
        public Quaternion rotation;
    }

    public float TimeMultiplier = 1f;

    // Needs its own Time.time
    private float PrivateTime;

    // If true, each frame which influences the object, will be tracked.
    public bool tracking = false;
    public Stack<TrackFragment> queue;
    public float _reversedTime = 0;

    private TrackFragment _lastFragment;
    private Rigidbody _body;
    // Helper value for when reversing time.
    private float freezeTime = 0;

    public RecordMaster RM;

	void Start () {
        RM = FindObjectOfType<RecordMaster>();
        PrivateTime = Time.time;
        _body = GetComponent<Rigidbody>();
        queue = new Stack<TrackFragment>();
        // Get the 'original' frame.
        TrackFragment fragment;
        fragment.velocity = _body.velocity;
        fragment.pos = transform.position;
        fragment.time = PrivateTime;
        fragment.rotation = transform.rotation;
        // Add it to the stack.
        queue.Push(fragment);
    }

    void Update () {
        float deltaTime = Time.deltaTime * TimeMultiplier;
        // If we're recording the object.
        if (tracking)
        {
            // If the object didn't move, we dont care.
            if(queue.Count > 0)
                if (transform.position == queue.Peek().pos && _body.velocity == queue.Peek().velocity)
                    return;
            // Turn ON unity's movement, should prob me replaced by our custom system.
            _body.isKinematic = false;
            PrivateTime += Time.deltaTime;
            TrackFragment fragment;
            fragment.velocity = _body.velocity;
            fragment.pos = transform.position;
            fragment.time = PrivateTime;
            fragment.rotation = transform.rotation;
            queue.Push(fragment);
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
            transform.position = _lastFragment.pos;
            _body.velocity = _lastFragment.velocity;
            transform.rotation = _lastFragment.rotation;
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
