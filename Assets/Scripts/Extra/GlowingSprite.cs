using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class GlowingSprite : MonoBehaviour {

    SpriteRenderer _sprite;

    public float
        MinAlpha = 75,
        MaxAlpha = 255,
        TransitionTime = 1.5f,
        Offset;

    Color 
        _minColor,
        _maxColor;

	void Start () {
        _sprite = GetComponent<SpriteRenderer>();

        _minColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, MinAlpha/255.0f);
        _maxColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, MaxAlpha/255.0f);
    }
	
	void Update () {
        _sprite.color = Color.Lerp(_minColor, _maxColor,Mathf.PingPong(Time.time + Offset, TransitionTime)/ TransitionTime);
	}
}
