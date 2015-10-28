using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchHandling : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AwaitInput());
    }

    IEnumerator AwaitInput()
    {
        while (true)
        {
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
            {
                HandleTap(Input.touches[0]);
                break;
            }

            if (Input.touchCount > 1 || Input.touches.Any(t => t.phase == TouchPhase.Moved))
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
            if (Input.touchCount == 1)
                HandleSwipe(Input.touches[0]);
            else
                HandlePinch(Input.touches.ToList());

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