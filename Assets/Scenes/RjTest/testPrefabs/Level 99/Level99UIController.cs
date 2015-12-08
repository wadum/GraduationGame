using UnityEngine;
using System.Collections.Generic;

public class Level99UIController : MonoBehaviour {

    public GameObject DeathScreen;
    public GameObject PowerUpChoosinScreen;

	public RectTransform PowerUp1Pos;
	public RectTransform PowerUp2Pos;	
	public RectTransform PowerUp3Pos;

	public Level99DragableButton PowerUp1;
	public Level99DragableButton PowerUp2;	
	public Level99DragableButton PowerUp3;

	public List<GameObject> Buttons;

	public void DisableNonChosen ()
	{
		Buttons.ForEach(b => {
			Level99DragableButton l = b.GetComponent<Level99DragableButton>();
			if(l)
				l.DisableNonChosen();
			else
				b.SetActive(false);
		});
	}

	public void EnableNonChosen ()
	{
		Buttons.ForEach(b => {
			Level99DragableButton l = b.GetComponent<Level99DragableButton>();
			if(l)
				l.EnableNonChosen();
			else
				b.SetActive(true);
		});
	}

    public void Play()
    {
        FindObjectOfType<Level99EnemySpawnController>().Reset();
        FindObjectOfType<Level99GameOverDetector>().EnableDeathCollider(true);
		DisableNonChosen();
    }
}
