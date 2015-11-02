using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Text))]
public class StoryText : MonoBehaviour {

	private string[] _lines;
	private int currentLine;

	public void Show(string textFile)
	{
		currentLine = 0;
		PopulateList(textFile);
		UpdateTextField();
	}

	public bool HasNext()
	{
		return currentLine < _lines.Length-1;
	}

	public bool HasPrevious()
	{
		return currentLine > 0;
	}

	public void Next()
	{
		if(!HasNext()) return;

		currentLine += 1;
		UpdateTextField();
	}

	public void Previous()
	{
		if(!HasPrevious()) return;

		currentLine -= 1;
		UpdateTextField();
	}

	private void UpdateTextField()
	{
		GetComponent<Text>().text = _lines[currentLine];
	}

	private void PopulateList(string textFile)
	{
		_lines = Resources.Load<TextAsset>(
			string.Format("Texts/{0}/{1}",I18n.GetInstance().GetLanguageName(), textFile))
			.text.Split (new char[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries);
	}
}
