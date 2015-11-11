using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class MouseToTouch {
    public class FakeTouch {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
        private object _touch = new Touch();
        private readonly Dictionary<string, FieldInfo> _fields = typeof (Touch).GetFields(Flags).ToDictionary(f => f.Name);

        public float DeltaTime { get { return ((Touch)_touch).deltaTime; } set { _fields["m_TimeDelta"].SetValue(_touch, value); } }
        public int TapCount { get { return ((Touch)_touch).tapCount; } set { _fields["m_TapCount"].SetValue(_touch, value); } }
        public TouchPhase Phase { get { return ((Touch)_touch).phase; } set { _fields["m_Phase"].SetValue(_touch, value); } }
        public Vector2 DeltaPosition { get { return ((Touch)_touch).deltaPosition; } set { _fields["m_PositionDelta"].SetValue(_touch, value); } }
        public int FingerId { get { return ((Touch)_touch).fingerId; } set { _fields["m_FingerId"].SetValue(_touch, value); } }
        public Vector2 Position { get { return ((Touch)_touch).position; } set { _fields["m_Position"].SetValue(_touch, value); } }
        public Vector2 RawPosition { get { return ((Touch)_touch).rawPosition; } set { _fields["m_RawPosition"].SetValue(_touch, value); } }

        public Touch Touch { get { return (Touch) _touch; } set { _touch = value; } }
    }

    private static float _lastEventTime = Time.time;
    private static Vector2 _lastEventPosition = Vector2.zero;
    private static Touch? _touch;
    private static float _lastPolled;

    public static Touch? GetTouch(float swipeSensitivity) {
        if ((Time.time - _lastPolled) < 0.0001)
            return _touch;

        _lastPolled = Time.time;

        var mouse = Input.GetMouseButton(0);
        var up = Input.GetMouseButtonUp(0);
        var down = Input.GetMouseButtonDown(0);

        if (!(mouse || up || down))
            return _touch = null;

        if (down) {
            _lastEventTime = Time.time;
            _lastEventPosition = Input.mousePosition;
        }

        var fake = new FakeTouch {
            FingerId = -1,
            TapCount = 1,
            Position = Input.mousePosition,
            RawPosition = Input.mousePosition,
            Phase =
                  up ? TouchPhase.Ended
                : down ? TouchPhase.Began
                : Vector3.Distance(Input.mousePosition, _lastEventPosition) < swipeSensitivity ? TouchPhase.Stationary
                : TouchPhase.Moved,
            DeltaPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _lastEventPosition,
            DeltaTime = Time.time - _lastEventTime
        };

        if (!down) {
            _lastEventTime = Time.time;
            _lastEventPosition = Input.mousePosition;
        }

        return _touch = fake.Touch;
    }
}