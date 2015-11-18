using UnityEngine;
using System.Collections;

public class TimeAnimationSync : MonoBehaviour {

    Animator _animator;
    ObjectTimeController _controller;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<ObjectTimeController>();
	}
	
	// Update is called once per frame
	void Update () {       
       _animator.SetFloat("Blend", _controller.GetFloat() / 100);
	}
}
