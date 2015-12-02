using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngineInternal;

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

    public static MultiTouch Instance { get { return _instance; } }

    private float _prevVolume;
    private const float VolumeFadeTime = 0.03f;

    private static readonly Dictionary<string, List<Func<RaycastHit, bool>>> TapEventHandlers = new Dictionary<string, List<Func<RaycastHit, bool>>>();
	private static readonly Dictionary<string, List<Func<RaycastHit, bool>>> TapAndHoldEventHandlers = new Dictionary<string, List<Func<RaycastHit, bool>>>();

    private static MultiTouch _instance;
    private static Action<Touch> _swipeHandler = (s) => { };
    private static Action<List<Touch>> _pinchHandler = (p) => { };
    private GraphicRaycaster _guiCaster;

	private RadialSlider _tapAndHoldIndicator;
    private const string TapAndHoldIgnoreLayer = "Water"; // TODO: CHANGE THIS :D
    private static int TapAndHoldIgnoreBitMask { get { return ~((1 << LayerMask.NameToLayer(TapAndHoldIgnoreLayer)) ^ (1 << LayerMask.NameToLayer("Ignore Raycast"))); } }
    
    void Awake() {
        ClearAllHandlers();

        _instance = this;
    }

    void Start()
    {
        _prevVolume = TapAndHoldCharge.volume;
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

        // TODO: REMOVE ME
        GameObject.FindGameObjectsWithTag("Terrain").ToList().ForEach(obj => obj.layer = LayerMask.NameToLayer(TapAndHoldIgnoreLayer));
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
        var tapCharge = 0f;
        var currentTapChargeVelocity = 0f;
        const float baseDechargeAcceleration = 7f;
        Func<float, float> normalizeCharge = charge =>
            charge >= TapHoldSeconds ? 1f :
            charge <= 0f ? 0f :
            charge / TapHoldSeconds;

		var firedTapAndHold = false;

        while (true) {
            // We calculate the current tapCharge based on the information from the last frame.
            if (Math.Abs(currentTapChargeVelocity) > 0.0001f) {
                if (tapCharge < 0f) {
                    tapCharge = 0f;
                    currentTapChargeVelocity = 0f;
                }

                tapCharge += Time.deltaTime * currentTapChargeVelocity;

                if (tapCharge > TapHoldSeconds) {
                    tapCharge = TapHoldSeconds;
                    currentTapChargeVelocity = 0f;
                }
            }

            var touches = GetTouches();

            if (!touches.Any()) {
                SetTapAndHoldValue(normalizeCharge(tapCharge), false);

                if (tapCharge > 0)
                    currentTapChargeVelocity -= Time.deltaTime*baseDechargeAcceleration;

                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Began) {

                var touch = touches[0];
                if (InteractableObjectInRange(touch.position)) {
                    _tapAndHoldIndicator.SetPosition(touch.position);
                    SetTapAndHoldValue(normalizeCharge(tapCharge), true);
                    currentTapChargeVelocity = 1f;
                }
                else {
                    SetTapAndHoldValue(0, false);
                    tapCharge = 0;
                    currentTapChargeVelocity = 0;
                }
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended) {
                if (tapCharge <= TapHoldSeconds && !firedTapAndHold)
					HandleTap(touches[0].position);

                if (firedTapAndHold) {
                    tapCharge = 0;
                    currentTapChargeVelocity = 0;
                }

                firedTapAndHold = false;

                yield return null;
                continue;
            }

            if (touches.Count == 1 && touches[0].phase == TouchPhase.Stationary) {
                var touch = touches[0];

                if (InteractableObjectInRange(touch.position)) {
                    SetTapAndHoldValue(normalizeCharge(tapCharge), true);
    				_tapAndHoldIndicator.SetPosition(touch.position);
                }
                else {
                    if (!firedTapAndHold) {
                        SetTapAndHoldValue(0, false);
                        tapCharge = 0;
                        currentTapChargeVelocity = 0;
                    }
                }

				if(tapCharge >= TapHoldSeconds && !firedTapAndHold){
					var position = touch.position;
                    firedTapAndHold = true;
	                HandleTapAndHold(position);

	                yield return null;
	                continue;
				}
            }

            if (touches.Count > 1 || touches.Any(t => t.phase == TouchPhase.Moved && t.deltaPosition.magnitude > SwipeSensitivity)) {
                SetTapAndHoldValue(0, false);
                tapCharge = 0f;
                currentTapChargeVelocity = 0f;

                yield return StartCoroutine(HandleGesture(touches));
                continue;
            }

            yield return null;
        }
    }

    private static bool InteractableObjectInRange(Vector2 position) {
        var hit = Raycast(position, TapAndHoldIgnoreBitMask);
        if (!hit.HasValue)
            return false;

        var highlight = hit.Value.transform.GetComponent<HighlightScript>();
        return highlight && highlight.InRange && !GameOverlayController.gameOverlayController.IsCurrentlySelected(highlight.gameObject);
    }

    private void SetTapAndHoldValue(float val, bool touching) {
        if (!_tapAndHoldIndicator || !TapAndHoldCharge) 
            return;

        if (val <= 0) {
            _tapAndHoldIndicator.SetValue(0);
            if (TapAndHoldCharge.isPlaying)
            {
                StopCoroutine("StopSound");
                StartCoroutine("StopSound", TapAndHoldCharge);
            }
            return;
        }

        if (val >= 1) {
            _tapAndHoldIndicator.SetValue(1);
            if (TapAndHoldCharge.isPlaying)
            {
                StopCoroutine("StopSound");
                StartCoroutine("StopSound", TapAndHoldCharge);
            }
            return;
        }

        _tapAndHoldIndicator.SetValue(val);

        if (touching && !TapAndHoldCharge.isPlaying){
			TapAndHoldCharge.time = TapAndHoldCharge.clip.length * val;
            StopCoroutine("StopSound");
            TapAndHoldCharge.volume = _prevVolume;
            TapAndHoldCharge.Play();
		}
        if (!touching && TapAndHoldCharge.isPlaying)
        {
            StopCoroutine("StopSound");
            StartCoroutine("StopSound", TapAndHoldCharge);
        }
    }

    private IEnumerator StopSound(AudioSource a)
    {
        float time = Time.time + VolumeFadeTime;
        var amount = _prevVolume / (Time.deltaTime / VolumeFadeTime);
        while (time > Time.time)
        {
            a.volume -= amount;
            yield return null;
        }
        a.Stop();
        a.volume = _prevVolume;
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

    private static void HandleTap(Vector3 position) {
        var hit = Raycast(position);
        if (!hit.HasValue) {
            GameOverlayController.gameOverlayController.DeactivateSlider();
            return;
        }

        var obj = hit.Value.transform.gameObject;
		List<Func<RaycastHit, bool>> handlers;
        if (TapEventHandlers.TryGetValue(obj.tag, out handlers))
            foreach(var handler in handlers) {
                handler(hit.Value);
		}

		if (!GameOverlayController.gameOverlayController.IsCurrentlySelected(obj))
			GameOverlayController.gameOverlayController.DeactivateSlider();
    }

    private void HandleTapAndHold(Vector3 position)
    {
        _tapAndHoldIndicator.SetValue(2);

        var hit = Raycast(position, TapAndHoldIgnoreBitMask);
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
	public static Func<RaycastHit, bool> RegisterTapHandlerByTag(string objTag, Func<RaycastHit, bool> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return handler;

        if (!TapEventHandlers.ContainsKey(objTag))
            TapEventHandlers[objTag] = new List<Func<RaycastHit, bool>>();

        TapEventHandlers[objTag].Add(handler);

        return handler;
    }

	public static Func<RaycastHit, bool> RegisterTapAndHoldHandlerByTag(string objTag, Func<RaycastHit, bool> handler)
    {
        if (string.IsNullOrEmpty(objTag))
            return handler;

        if (!TapAndHoldEventHandlers.ContainsKey(objTag))
            TapAndHoldEventHandlers[objTag] = new List<Func<RaycastHit, bool>>();

        TapAndHoldEventHandlers[objTag].Add(handler);

        return handler;
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

    public static void RemoveSpecificTapHandler(Func<RaycastHit, bool> handler) {
        foreach (var handlerList in TapEventHandlers.Values) {
            handlerList.RemoveAll(h => h == handler);
        }
    }

    public static void RemoveSpecificTapAndHoldHandler(Func<RaycastHit, bool> handler) {
        foreach (var handlerList in TapAndHoldEventHandlers.Values) {
            handlerList.RemoveAll(h => h == handler);
        }
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

    private static RaycastHit? Raycast(Vector3 position, int? layerMask = null) {
        var ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;
        
        if (layerMask.HasValue)
            return Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask.Value)?
                (RaycastHit?)hit :
                null;

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