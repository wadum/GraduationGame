﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TimeSliderController : MonoBehaviour
{

    public float TimeSpeed;
    public float JaggingTime = 0.5f;
    public float JaggingCD = 0.5f;
    public bool HalfJaggedSpeed = false;
    private bool jagging = false;

    //Audio sources
    public AudioSource
        BackwardsButton,
        ForwardsButton;

    private AudioSource _jaggeringSound;

    private Slider _slider;
    private SoundMaster _soundMaster;
    private TimeControllable _obj;
    private GameObject _player;

    private Coroutine _increaseCorout;
    private Coroutine _decreaseCorout;
    private float bar { get { return JaggingTime; } set { JaggingTime = value; } }
    private List<CockRotator> _bottunsToRotate = new List<CockRotator>();
    RaycastHit hit;
 
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _soundMaster = FindObjectOfType<SoundMaster>();
        _slider = GetComponentInChildren<Slider>();
        _jaggeringSound = GetComponent<AudioSource>();
        _bottunsToRotate.AddRange(GetComponentsInChildren<CockRotator>());
    }

    public void SetTimeControllable(TimeControllable obj)
    {
        _obj = obj;
    }

    public void IncreaseTimePressed()
    {
		if(_increaseCorout != null)
			StopCoroutine(_increaseCorout);
		if(_decreaseCorout != null)
			StopCoroutine(_decreaseCorout);
        _increaseCorout = StartCoroutine(changeValue(TimeSpeed));
        if(!jagging)
            _obj.MusicForward();
    }

    public void IncreaseTimeReleased()
    {
		if(_increaseCorout != null)
			StopCoroutine(_increaseCorout);
		if(_decreaseCorout != null)
			StopCoroutine(_decreaseCorout);
        if (_increaseCorout == null) return;
        StopCoroutine(_increaseCorout);
        _obj.StopMusic();
    }

    public void DecreaseTimePressed()
    {
		if(_decreaseCorout != null)
			StopCoroutine(_decreaseCorout);
		if(_increaseCorout != null)
			StopCoroutine(_increaseCorout);
		if (_soundMaster)
            _soundMaster.PlayReversed();
        _decreaseCorout = StartCoroutine(changeValue(-TimeSpeed));
        if(!jagging)
            _obj.MusicBackward();
    }

    public void DecreaseTimeReleased()
    {
		if(_increaseCorout != null)
			StopCoroutine(_increaseCorout);
		if(_decreaseCorout != null)
			StopCoroutine(_decreaseCorout);
        if (_decreaseCorout == null) return;
        if (_soundMaster)
            _soundMaster.PlayNormal();
        StopCoroutine(_decreaseCorout);
        _obj.StopMusic();
    }

    IEnumerator changeValue(float var)
    {
        jagging = false;
        float foo = 0;
        float jaggingCDTime = 10;
        while (true)
        {
            while (jagging)
            {

                jaggingCDTime += Time.deltaTime;
                if (jaggingCDTime > JaggingCD)
                {
                    foo += Time.deltaTime;
                    _bottunsToRotate.ForEach(cock => cock.RotateWheel(Mathf.Sign(var), HalfJaggedSpeed));

                    BackwardsButton.mute = true;
                    ForwardsButton.mute = true;
                    if (_jaggeringSound && !_jaggeringSound.isPlaying)
                        _jaggeringSound.Play();

                    if (foo > bar)
                    {
                        foo = 0;
                        _bottunsToRotate.ForEach(cock => cock.ResetRot());
                        jaggingCDTime = 0;
                    }
                }
                yield return null;
                continue;
            }
            BackwardsButton.mute = false;
            ForwardsButton.mute = false;
            // If the slider is valid to move (in between max or min), and we're not a child of it, then it can move.
            if (_slider.value < (100 - 100f * Time.deltaTime / var) && _slider.value > - (100f * Time.deltaTime / var) && (!_player.transform.IsChildOf(_obj.transform.root) || _obj.tag == "Moveable Rock"))
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(_player.transform.position, -Vector3.up, 0.6f);

                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];

                    if (_obj.tag != "Moveable Rock" && hit.collider.gameObject.transform.root == _obj.transform)
                    {
                        jagging = true;
                        _bottunsToRotate.ForEach(cock => cock.SaveRot());
                        yield return null;
                        continue;
                    }
                }
            }
            else
            {
                jagging = true;
                _bottunsToRotate.ForEach(cock => cock.SaveRot());
                yield return null;
                continue;
            }
            if (!jagging)
            {
                _bottunsToRotate.ForEach(cock => cock.RotateWheel(Mathf.Sign(var)));
                _slider.value += 100f * Time.deltaTime / var;
            }
            yield return null;
        }
    }
}
