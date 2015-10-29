using UnityEngine;
using System.Collections;

public class StoryTextController : MonoBehaviour {

	public GameObject CloseButton;
	public GameObject PreviousButton;
	public GameObject NextButton;
	public StoryText TextController;

	public void Show(string textFile)
	{
		TextController.Show(textFile);
		UpdateButtons();
	}

	public void Next()
	{
		TextController.Next();
		UpdateButtons();

	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void Previous()
	{
		TextController.Previous();
		UpdateButtons();
	}

	private void UpdateButtons()
	{
		DisableButtons();
		if(TextController.HasPrevious())
			PreviousButton.SetActive(true);
		if(TextController.HasNext())
			NextButton.SetActive(true);
		else
			CloseButton.SetActive(true);
	}

	private void DisableButtons()
	{
		PreviousButton.SetActive(false);
		NextButton.SetActive(false);
		CloseButton.SetActive(false);
	}

}
