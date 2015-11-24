using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiTouch : MonoBehaviour
{
    public float SwipeSensitivity = 4f;
    public float TapHoldSeconds = 1f;
	public float TapHoldIndicatorDelay = 0.1f;
    public bool SimulateMouseTapInEditor = true;

    public AudioSource
        TapAndHoldCharge,
        TapAndHoldSucces,
        TapAndHoldFail;

	private static readonly Dictionary<string, List<Func<RaycastHit, bool>>> TapEventHandlers = new Dictionary<string, List<Func<RaycastHit, bool>>>();
	private static readonly Dictionary<string, List<Func<RaycastHit, bool>>> TapAndHoldEventHandlers = new Dictionary<string, List<Func<RaycastHit, bool>>>();

    private static Action<Touch> _swipeHandler = (s) => { };
    private static Action<List<Touch>> _pinchHandler = (p) => { };
    private GraphicRaycaster _guiCaster;

	private RadialSlider _tapAndHoldIndicator;

    void Awake() {
        ClearAllHandlers();
    }

    void Start()
    {
        var gui = GameObject.FindGameObjectWithTag("Gui");
        if (!gui)
        {
            Debug.Log("No tagged canvas Gui, getting canvas by fallback...");
            var canvas = FindObjectOfType<Canvas>();
            if (canvas){
                gui = canvas.gameObject;
			}
            else
                Debug.Log("No canvas found for fallback.");
        }
        if (gui)
            _guiCaster = gui.GetComponentInChildren<GraphicRaycaster>();
        
        if (!_guiCaster)
            Debug.Log("GraphicRaycaster could not be gotten from canvas, ui blocking will not be available.");
    }

    void OnEnable() {
		_tapAndHoldIndicator = GameObject.FindGameObjectWithTag("Gui").GetComponentInChildren<RadialSlider>();
        StartCoroutine(AwaitInput());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    #region State machine for recognizing multitouch taps and gestures
    private IEnumerator AwaitInput() {
        var touch1Began = 0f;
		var firedTapAndHold = false;

        while (true) {
            var touches = GetTouches();

            if (!touches.Any())
            {
                StopTapAndHoldIndicator();
                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Began) {
                touch1Began = Time.time;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended) {
                StopTapAndHoldIndicator();
                firedTapAndHold = false;
                if (Time.time - touch1Began <= TapHoldSeconds)
					HandleTap(touches[0].position);
                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Stationary)
            {
				if(Time.time - touch1Began > TapHoldIndicatorDelay){
					var position = touches[0].position;
					_tapAndHoldIndicator.SetPosition(position);
                    if(!TapAndHoldCharge.isPlaying)
                        TapAndHoldCharge.Play();
                    _tapAndHoldIndicator.SetValue((Time.time - touch1Began - TapHoldIndicatorDelay) / (TapHoldSeconds - TapHoldIndicatorDelay));
				}


				if(Time.time - touch1Began > TapHoldSeconds && !firedTapAndHold){
					var position = touches[0].position;
					firedTapAndHold = true;
	                HandleTapAndHold(position);
	                yield return null;
	                continue;
				}
            }

            if (touches.Count > 1 || touches.Any(t => t.phase == TouchPhase.Moved && t.deltaPosition.magnitude > SwipeSensitivity)) {
                StopTapAndHoldIndicator();
                yield return StartCoroutine(HandleGesture(touches));
                continue;
            }

            yield return null;
        }
    }

    private void StopTapAndHoldIndicator()
    {
        if (_tapAndHoldIndicator)
            _tapAndHoldIndicator.SetValue(0);
        if (TapAndHoldCharge.isPlaying)
            TapAndHoldCharge.Stop();
    }

    private IEnumerator HandleGesture(List<Touch> initialTouches) {
        var touches = initialTouches;
        while (touches.Count > 0) {
            if (!touches.Any()) {
                yield return null;
                continue;
            }

            if (touches.Count == 1)
                HandleSwipe(touches[0]);
            else
                HandlePinch(touches);

            yield return null;
            touches = GetTouches();
        }
    }
    #endregion

    #region callback handling for the different taps and gestures
    private static void HandlePinch(List<Touch> touches) {
        // Right now we just do this, as we are pretty certain we only need one pinch handler,
        // specifically for the camera. Keep in mind that we ignore UI touches, so UI pinches should still function.
        _pinchHandler(touches);
    }

    private static void HandleSwipe(Touch touch) {
        // Right now we just do this, as we are pretty certain we only need one swipe handler,
        // specifically for the camera. Keep in mind that we ignore UI touches, so UI swipes should still function.
        _swipeHandler(touch);
    }

    private void HandleTap(Vector3 position)
    {
        var hit = Raycast(position);
		List<Func<RaycastHit, bool>> handlers;
		int hits = 0;
        if (hit.HasValue && TapEventHandlers.TryGetValue(hit.Value.collider.tag, out handlers))
            foreach(var handler in handlers){
				hits++;
                handler(hit.Value);
		}
		if (hits == 0)
			GameOverlayController.gameOverlayController.DeactivateSlider();
    }

    private void HandleTapAndHold(Vector3 position)
    {
        _tapAndHoldIndicator.SetValue(2);

        var hit = Raycast(position);
		List<Func<RaycastHit, bool>> handlers;
        if (hit.HasValue && TapAndHoldEventHandlers.TryGetValue(hit.Value.collider.tag, out handlers))
            if (handlers.Any(h => !h(hit.Value)))
            {
                _tapAndHoldIndicator.TransitionWrong();
                TapAndHoldFail.Play();
            }
            else
            {
                _tapAndHoldIndicator.TransitionRight();
                TapAndHoldSucces.Play();
            }
        else
        {
            _tapAndHoldIndicator.TransitionWrong();
            TapAndHoldFail.Play();
        }
    }
    #endregion

    #region registration of handlers
	public static void RegisterTapHandlerByTag(string objTag, Func<RaycastHit, bool> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return;

        if (!TapEventHandlers.ContainsKey(objTag))
            TapEventHandlers[objTag] = new List<Func<RaycastHit, bool>>();

        TapEventHandlers[objTag].Add(handler);
    }

	public static void RegisterTapAndHoldHandlerByTag(string objTag, Func<RaycastHit, bool> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return;

        if (!TapAndHoldEventHandlers.ContainsKey(objTag))
            TapAndHoldEventHandlers[objTag] = new List<Func<RaycastHit, bool>>();

        TapAndHoldEventHandlers[objTag].Add(handler);
    }

    public static void RegisterSwipeHandler(Action<Touch> handler) {
        _swipeHandler = handler;
    }

    public static void RegisterPinchHandler(Action<List<Touch>> handler) {
        _pinchHandler = handler;
    }
    #endregion

    #region removing handlers
    public static void ClearTapHandler(string objTag) {
        TapEventHandlers.Remove(objTag);
    }
    public static void ClearTapAndHoldHandler(string objTag) {
        TapAndHoldEventHandlers.Remove(objTag);
    }

    public static void ClearAllTapHandlers() {
        TapEventHandlers.Clear();
    }

    public static void ClearAllTapAndHoldHandlers() {
        TapAndHoldEventHandlers.Clear();
    }

    public static void ClearSwipeHandler() {
        _swipeHandler = s => { };
    }

    public static void ClearPinchHandler() {
        _pinchHandler = p => { };
    }

    public static void ClearAllHandlers() {
        ClearAllTapHandlers();
        ClearAllTapAndHoldHandlers();
        ClearSwipeHandler();
        ClearPinchHandler();
    }
    #endregion

    #region private helpers
    private List<Touch> GetTouches()
    {
        var touches = Input.touches.ToList();

        if (SimulateMouseTapInEditor && Application.isEditor) {
            var fake = MouseToTouch.GetTouch(SwipeSensitivity);
            if (fake.HasValue)
                touches.Add(fake.Value);
        }

        return touches.Where(t => !IsPointerOverGui(t.position)).ToList();
    }

    private static RaycastHit? Raycast(Vector3 position) {
        var ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit)?
            (RaycastHit?)hit :
            null;
    }

    /// <summary>
    /// Since unity has marked the misfeature where IsPointerOverGameObject
    /// always return false on android as "by design" we have to make our own
    /// replacement raycasting against the canvas.
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