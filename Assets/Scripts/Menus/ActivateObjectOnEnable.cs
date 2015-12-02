using UnityEngine;
using System.Collections;

public class ActivateObjectOnEnable : MonoBehaviour
{
    // Utility script which allows for enabling or disabling items related to this GameObject.
    // Primarily used for main menu animation, so the animation on the camera can affect objects outside the animation.
    public GameObject obj;

    void OnEnable()
    {
        obj.SetActive(true);
    }

    void OnDisable()
    {
        obj.SetActive(false);
    }
}