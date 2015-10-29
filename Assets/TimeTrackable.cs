using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeTrackable : MonoBehaviour {

    public struct TrackFragment{
        public Vector3 pos;
        public float time;
        public Vector3 velocity;
        public Quaternion rotation;
//        public Transform transform;
    }

    private float PrivateTime;

    public bool tracking;
    public Stack<TrackFragment> queue;
    private TrackFragment _lastFragment;
    private Rigidbody _body;

    public float _reversedTime = 0;
    private float freezeTime = 0;

	// Use this for initialization
	void Start () {
        PrivateTime = Time.time;
        _body = GetComponent<Rigidbody>();
        queue = new Stack<TrackFragment>();
        TrackFragment fragment;
        fragment.velocity = _body.velocity;
        fragment.pos = transform.position;
        fragment.time = Time.time;
        fragment.rotation = transform.rotation;
        queue.Push(fragment);
    }

    // Update is called once per frame
    void Update () { 
        if (tracking)
        {
            _body.isKinematic = false;
            if(queue.Count > 0)
                if (transform.position == queue.Peek().pos && _body.velocity == queue.Peek().velocity)
                    return;
            PrivateTime += Time.deltaTime;
            TrackFragment fragment;
            fragment.velocity = _body.velocity;
            fragment.pos = transform.position;
            fragment.time = PrivateTime;
            fragment.rotation = transform.rotation;
            queue.Push(fragment);
            return;
        }
        _body.isKinematic = true;
        _reversedTime += Time.deltaTime;
        if (queue.Count == 0)
        {
            return;
        }
        if (freezeTime - _reversedTime <= queue.Peek().time)
        {
            _lastFragment = queue.Pop();
            transform.position = _lastFragment.pos;
            _body.velocity = _lastFragment.velocity;
            transform.rotation = _lastFragment.rotation;
        }
    }

    public void Reverse()
    {
        if (tracking)
        {
            freezeTime = PrivateTime;
            _reversedTime = 0;
            tracking = false;
        }
    }

    public void Record()
    {
        if (queue.Count > 0)
            Debug.Log("I got stuff in me");
        tracking = true;
    }
}
