using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchHandling : MonoBehaviour
{
    public float SwipeSensitivity = 4f;
    public float TapHoldSeconds = 1f;
    public bool SimulateMouseTapInEditor = true;

    private Dictionary<string, List<Action<RaycastHit>>> _tapEventHandlers;
    private Dictionary<string, List<Action<RaycastHit>>> _tapAndHoldEventHandlers;
    private Action<Touch> _swipeHandler = (s) => { };
    private Action<List<Touch>> _pinchHandler = (p) => { };
    private GraphicRaycaster _guiCaster;

    void Awake() {
        _tapEventHandlers = new Dictionary<string, List<Action<RaycastHit>>>();
        _tapAndHoldEventHandlers = new Dictionary<string, List<Action<RaycastHit>>>();
    }

    void Start()
    {
        var gui = GameObject.FindGameObjectWithTag("Gui");
        if (!gui)
        {
            Debug.Log("No tagged canvas Gui, getting canvas by fallback...");
            var canvas = FindObjectOfType<Canvas>();
            if (canvas)
                gui = canvas.gameObject;
            else
                Debug.Log("No canvas found for fallback.");
        }
        if (gui)
            _guiCaster = gui.GetComponent<GraphicRaycaster>();
        
        if (!_guiCaster)
            Debug.Log("GraphicRaycaster could not be gotten from canvas, ui blocking will not be available.");
    }

    void OnEnable() {
        StartCoroutine(AwaitInput());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    #region State machine for recognizing multitouch taps and gestures
    private IEnumerator AwaitInput() {
        var touch1Began = 0f;

        while (true) {
            var touches = GetTouches();

            if (SimulateMouseTapInEditor && Application.isEditor && Input.GetMouseButtonDown(0))
            {
                HandleTap(Input.mousePosition);
                yield return null;
                continue;
            }

            if (!touches.Any()) {
                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Began) {
                touch1Began = Time.time;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended) {
                var position = touches[0].position;
                if (Time.time - touch1Began > TapHoldSeconds)
                    HandleTapAndHold(position);
                else
                    HandleTap(position);
                yield return null;
                continue;
            }

            if (touches.Count > 1 || touches.Any(t => t.phase == TouchPhase.Moved && t.deltaPosition.magnitude > SwipeSensitivity)) {
                yield return StartCoroutine(HandleGesture());
                continue;
            }

            yield return null;
        }
    }


    private IEnumerator HandleGesture() {
        while (Input.touchCount > 0) {
            var touches = GetTouches();

            if (!touches.Any()) {
                yield return null;
                continue;
            }

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

    private void HandleTap(Vector3 position)
    {
        var hit = Raycast(position);
        List<Action<RaycastHit>> handlers;
        if (hit.HasValue && _tapEventHandlers.TryGetValue(hit.Value.collider.tag, out handlers))
            foreach(var handler in handlers)
                handler(hit.Value);
    }

    private void HandleTapAndHold(Vector3 position)
    {
        var hit = Raycast(position);
        List<Action<RaycastHit>> handlers;
        if (hit.HasValue && _tapAndHoldEventHandlers.TryGetValue(hit.Value.collider.tag, out handlers))
            foreach(var handler in handlers)
                handler(hit.Value);
    }

    #endregion

    #region registration of handlers
    public void RegisterTapHandlerByTag(string objTag, Action<RaycastHit> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return;

        if (!_tapEventHandlers.ContainsKey(objTag))
            _tapEventHandlers[objTag] = new List<Action<RaycastHit>>();

        _tapEventHandlers[objTag].Add(handler);
    }

    public void RegisterTapAndHoldHandlerByTag(string objTag, Action<RaycastHit> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return;

        if (!_tapAndHoldEventHandlers.ContainsKey(objTag))
            _tapAndHoldEventHandlers[objTag] = new List<Action<RaycastHit>>();

        _tapAndHoldEventHandlers[objTag].Add(handler);
    }

    public void RegisterSwipeHandler(Action<Touch> handler) {
        _swipeHandler = handler;
    }

    public void RegisterPinchHandler(Action<List<Touch>> handler) {
        _pinchHandler = handler;
    }
    #endregion

    #region removing handlers
    public void ClearTapHandler(string objTag) {
        // Functions even if tag does not exist.
        _tapEventHandlers.Remove(objTag);
    }
    public void ClearTapAndHoldHandler(string objTag) {
        // Functions even if tag does not exist.
        _tapAndHoldEventHandlers.Remove(objTag);
    }

    public void ClearAllTapHandlers() {
        _tapEventHandlers.Clear();
    }

    public void ClearAllTapAndHoldHandlers() {
        _tapAndHoldEventHandlers.Clear();
    }

    public void ClearSwipeHandler() {
        _swipeHandler = s => { };
    }

    public void ClearPinchHandler() {
        _pinchHandler = p => { };
    }

    public void ClearAllHandlers() {
        ClearAllTapHandlers();
        ClearAllTapAndHoldHandlers();
        ClearSwipeHandler();
        ClearPinchHandler();
    }
    #endregion

    #region private helpers
    private List<Touch> GetTouches()
    {
        return Input.touches.Where(t => !IsPointerOverGui(t.position)).ToList();
    }

    private static RaycastHit? Raycast(Vector3 position) {
        var ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit)?
            (RaycastHit?)hit :
            null;
    }

    /// <summary>
    /// Since unity has marked the bug where IsPointerOverGameObject as "by design"
    /// we have to make our own replacement raycasting against the canvas.
    /// 
    /// *bleep* Unity.
    /// </summary>
    /// <param name="pointer">The position of the pointer in screen pixel coordinates</param>
    /// <returns>Whether or not the pointer is over an active ui element</returns>
    private bool IsPointerOverGui(Vector2 pointer)
    {
        if (!_guiCaster)
            return false;

        var fakePointerEvent = new PointerEventData(EventSystem.current) {position = pointer};;
        var hits = new List<RaycastResult>();
        _guiCaster.Raycast(fakePointerEvent, hits);

        return hits.Any();
    }
    #endregion
}