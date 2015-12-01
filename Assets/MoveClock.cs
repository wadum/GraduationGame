using UnityEngine;
using System.Collections;

public class MoveClock : MonoBehaviour {
    Animator anim;
    float _value, _target, _dist, _start;
    public float Speed;
    bool moving;

    void Start () {
        anim = GetComponent<Animator>();
	}

    public void SetLevel(int value)
    {
        _dist = 0;
        moving = true;
        _start = Time.time;
        switch (value)
        {
            case 5:
                _target = (1f / 4f);
                break;
            case 7:
                _target = (2f / 4f);
                break;
            case 9:
                _target = (3f / 4f);
                break;
            default:
                _target = (0f);
                break;
        }
    }
	
    void Update()
    {
        if(!moving)
            return;
        if (_dist < 1) {
            _dist = (Time.time - _start) / Speed;
            anim.SetFloat("Blend", Mathf.Lerp(_value, _target, _dist));
            return;
        }
        _value = _target;
        moving = false;
    }
}
