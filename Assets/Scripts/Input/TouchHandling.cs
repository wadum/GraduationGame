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
    public bool SimulateMouseTapInEditor = true;

    private Dictionary<string, List<Action<RaycastHit>>> _tapEventHandlers;
    private Action<Touch> _swipeHandler = (s) => { };
    private Action<List<Touch>> _pinchHandler = (p) => { };
    private GraphicRaycaster _guiCaster;

    void Awake() {
        _tapEventHandlers = new Dictionary<string, List<Action<RaycastHit>>>();
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
        while (true) {
            var touches = GetTouches();

            if (SimulateMouseTapInEditor && Application.isEditor && Input.GetMouseButtonDown(0))
            {
                HandleMouseTap(Input.mousePosition);
                yield return null;
                continue;
            }

            if (!touches.Any()) {
                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended) {
                HandleTap(touches[0]);
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

    private void HandleMouseTap(Vector3 mousePosition)
    {
        HandleTapRaycast(mousePosition);
    }

    private void HandleTap(Touch tap) {
        HandleTapRaycast(tap.position);
    }

    private void HandleTapRaycast(Vector3 position)
    {
        var ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        List<Action<RaycastHit>> handlers;
        if (Physics.Raycast(ray, out hit) && _tapEventHandlers.TryGetValue(hit.collider.tag, out handlers))
            foreach(var handler in handlers)
                handler(hit);
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
    private List<Touch> GetTouches()
    {
        return Input.touches.Where(t => !IsPointerOverGui(t.position)).ToList();
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