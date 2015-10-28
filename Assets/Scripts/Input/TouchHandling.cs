using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandling : MonoBehaviour {
    private Dictionary<string, Action<RaycastHit>> _tapEventHandlers;
    private Action<Touch> _swipeHandler;
    private Action<List<Touch>> _pinchHandler;

    void Start() {
        _tapEventHandlers = new Dictionary<string, Action<RaycastHit>>();
        ClearSwipeHandler();
        ClearPinchHandler();
    }

    void OnEnable() {
        StartCoroutine(AwaitInput());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    #region State machine for recognizing multitouch taps and gestures
    private IEnumerator AwaitInput() {
        while (true) {
            var touches = GetTouches();
            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended) {
                HandleTap(touches[0]);
                break;
            }

            if (touches.Count > 1 || touches.Any(t => t.phase == TouchPhase.Moved)) {
                yield return StartCoroutine(HandleGesture());
                break;
            }

            yield return null;
        }
    }

    private IEnumerator HandleGesture() {
        while (Input.touchCount > 0) {
            var touches = GetTouches();

            if (touches.Count == 1)
                HandleSwipe(touches[0]);
            else
                HandlePinch(touches);

            yield return null;
        }
    }
    #endregion

    #region callback handling for the different taps and gestures
    private void HandlePinch(List<Touch> touches) {
        // Right now we just do this, as we are pretty certain we only need one pinch handler,
        // specifically for the camera. Keep in mind that we ignore UI touches, so UI pinches should still function.
        _pinchHandler(touches);
    }

    private void HandleSwipe(Touch touch) {
        // Right now we just do this, as we are pretty certain we only need one swipe handler,
        // specifically for the camera. Keep in mind that we ignore UI touches, so UI swipes should still function.
        _swipeHandler(touch);
    }

    private void HandleTap(Touch tap) {
        // We get the handler for the type of gameobject we tapped based on its tag.
        var ray = Camera.main.ScreenPointToRay(tap.position);
        RaycastHit hit;
        Action<RaycastHit> handler;
        if (Physics.Raycast(ray, out hit) &&
            _tapEventHandlers.TryGetValue(hit.collider.tag, out handler))
                handler(hit);
    }
    #endregion

    #region registration of handlers
    public void RegisterTapHandlerByTag(string tag, Action<RaycastHit> handler) {
        // Overwrites old one if it exists
        _tapEventHandlers[tag] = handler;
    }

    public void RegisterSwipeHandler(Action<Touch> handler) {
        _swipeHandler = handler;
    }

    public void RegisterPinchHandler(Action<List<Touch>> handler) {
        _pinchHandler = handler;
    }
    #endregion

    #region removing handlers
    public void ClearTapHandler(string tag) {
        // Functions even if tag does not exist.
        _tapEventHandlers.Remove(tag);
    }

    public void ClearAllTapHandlers() {
        _tapEventHandlers.Clear();
    }

    public void ClearSwipeHandler() {
        _swipeHandler = s => { };
    }

    public void ClearPinchHandler() {
        _pinchHandler = p => { };
    }

    public void ClearAllHandlers() {
        ClearAllTapHandlers();
        ClearSwipeHandler();
        ClearPinchHandler();
    }
    #endregion

    #region private helpers
    private static List<Touch> GetTouches() {
        return Input.touches.Where(t => EventSystem.current.IsPointerOverGameObject(t.fingerId)).ToList();
    }
    #endregion
}