using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandling : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AwaitInput());
    }

    private static List<Touch> GetTouches()
    {
        return Input.touches.Where(t => EventSystem.current.IsPointerOverGameObject(t.fingerId)).ToList();
    } 

    private IEnumerator AwaitInput()
    {
        while (true)
        {
            var touches = GetTouches();
            if (touches.Count == 1 && touches[0].phase == TouchPhase.Ended)
            {
                HandleTap(touches[0]);
                break;
            }

            if (touches.Count > 1 || touches.Any(t => t.phase == TouchPhase.Moved))
            {
                yield return StartCoroutine(HandleGesture());
                break;
            }

            yield return null;
        }
    }

    private IEnumerator HandleGesture()
    {
        while (Input.touchCount > 0)
        {
            var touches = GetTouches();

            if (touches.Count == 1)
                HandleSwipe(touches[0]);
            else
                HandlePinch(touches);

            yield return null;
        }
    }

    private void HandlePinch(List<Touch> touches)
    {
        throw new System.NotImplementedException();
    }

    private void HandleSwipe(Touch touch)
    {
        throw new System.NotImplementedException();
    }

    private void HandleTap(Touch tap)
    {
        throw new System.NotImplementedException();
    }
}