using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeFromBlackCreditsScreen : MonoBehaviour {

    Image _sprite;

    public float
        MinAlpha = 0,
        MaxAlpha = 255,
        TransitionTime = 0.3f,
        Offset;

    Color
        _AlfaColor,
        _UnAlfaColor;

    float
        realTransTime;

    void Start()
    {
        _sprite = GetComponent<Image>();

        _AlfaColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, MinAlpha/255f);
        _UnAlfaColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, MaxAlpha/255f);
    }

    void OnEnable()
    {
        realTransTime = Time.time;
    }

    void Update()
    {
        _sprite.color = Color.Lerp(_UnAlfaColor, _AlfaColor, (Time.time-realTransTime)/TransitionTime);

        if(realTransTime + TransitionTime < Time.time)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        _sprite.color = _UnAlfaColor;
    }
}
