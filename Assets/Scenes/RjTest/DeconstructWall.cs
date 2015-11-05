﻿using UnityEngine;
using System.Linq;
using System;

public class DeconstructWall : MonoBehaviour, TimeControllable {

    public float PercentageLeftBehind = 3;

    private Transform[] _bricks;
    private float _level;

    // Use this for initialization
    void Start()
    {
        _bricks = this.GetComponentsInChildren<Transform>().Where(b => b != transform).OrderBy(b => b.position.y).ToArray();
        TouchHandling touchHandling = FindObjectOfType<TouchHandling>();
        touchHandling.RegisterTapHandlerByTag("Breakable Wall", hit =>
        {
            if (hit.collider.gameObject.transform.root.gameObject == gameObject)
            {
                Debug.Log("Got Hit");
                FindObjectOfType<GameOverlayController>().Activate(this);
            }
        });
    }
	
	public void SetConstructionLevel(float level)
    {
        Debug.Log(_bricks.Length);
        _level = level;
        float percentage = _bricks.Length * (level + 1 + PercentageLeftBehind/100);

        for (int i = 0; i < _bricks.Length; i++)
        {
            _bricks[i].gameObject.SetActive((!(i > percentage)));
        }
    }

    public void SetFloat(float var)
    {
        SetConstructionLevel(var);
    }

    public float GetFloat()
    {
        return _level;
    }
}
