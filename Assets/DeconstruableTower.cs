using UnityEngine;
using System.Collections;

public class DeconstruableTower : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TouchHandling touchHandling = FindObjectOfType<TouchHandling>();
        touchHandling.RegisterTapHandlerByTag("Breakable Wall", hit => FindObjectOfType<GameOverlayController>().ToggleWallSlider());
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag == "Player")
        {
            FindObjectOfType<GameOverlayController>().HideWallSlider();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {

        }
    }

}

/*
using UnityEngine;
using System.Collections;
using System.Linq;

public class DeconstructWall : MonoBehaviour {

    public float PercentageLeftBehind = 3;

    private Transform[] _bricks;

    // Use this for initialization
    void Start()
    {
        _bricks = this.GetComponentsInChildren<Transform>().Where(b => b != transform).OrderBy(b => b.position.y).ToArray();
        TouchHandling touchHandling = FindObjectOfType<TouchHandling>();
        touchHandling.RegisterTapHandlerByTag("Breakable Wall", hit => FindObjectOfType<GameOverlayController>().ToggleWallSlider());
    }
	
	public void SetConstructionLevel(float level)
    {
        Debug.Log(_bricks.Length);
        float percentage = _bricks.Length * (level + 1 + PercentageLeftBehind/100);

        for (int i = 0; i < _bricks.Length; i++)
        {
            _bricks[i].gameObject.SetActive((i > percentage) ? false : true);
        }
    }
}
*/