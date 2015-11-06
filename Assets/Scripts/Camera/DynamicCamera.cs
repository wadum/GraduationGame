using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicCamera : MonoBehaviour {

    public float NeutralDistanceFromPlayer = 5f;
    public float NeutralLookAngleBehindPlayer = 20f;

    private Transform _player;
    private Camera _camera;

    private bool Sanity() {
        _camera = Camera.main;
        if (!_camera) {
            Debug.Log("No main camera found.");
            return false;
        }

        var players = GameObject.FindGameObjectsWithTag("Player");
        if (!players.Any()) {
            Debug.Log("No GameObject tagged Player found.");
            return false;
        }

        if (players.Length >= 2) {
            Debug.Log("More than one GameObject tagged player: " + string.Join(", ", players.Select(p => p.name).ToArray()));
            return false;
        }

        _player = players.First().transform;

        return true;
    }

	void Start () {
	    if (!Sanity()) {
            Debug.Log("DynamicCamera disabled due to sanity check failing.");
	        enabled = false;
	        return;
	    }

        MultiTouch.RegisterPinchHandler(HandlePinch);
	}

    private void HandlePinch(List<Touch> pinch) {
        Func<List<Vector2>, Vector2> averagePoint = vectors => vectors.Aggregate((v1, v2) => v1 + v2) / vectors.Count;
        Func<List<Vector2>, Vector2, float> averageDistanceToPoint = (vectors, point) => vectors.Select(p => Vector2.Distance(p, point)).Average();
        Func<Vector2, Vector2> screenNormalize = v => new Vector2(v.x / Screen.width, v.y / Screen.height);

        var positions = pinch.Select(t => t.position).ToList();
        var center = averagePoint(positions);
        var averageDistanceToCenter = averageDistanceToPoint(positions, center);

        var previousPositions = pinch.Select(t => t.position - t.deltaPosition).ToList();
        var previousCenter = averagePoint(previousPositions);
        var previousAverageDistanceToCenter = averageDistanceToPoint(previousPositions, previousCenter);

        var centerMovementDirection = screenNormalize(center - previousCenter);
        var centerDistanceRatio = averageDistanceToCenter/previousAverageDistanceToCenter;
    }
}