using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextI18n : MonoBehaviour {
	
	public string TranslationKey;
	
	private Text text;
	
	void Start(){
		text = GetComponentInParent<Text>();
		UpdateField ();
	}
	
	public void UpdateField(){
		if (I18n.GetInstance () != null && text != null) {
			text.text = I18n.GetInstance ().Translate (TranslationKey);
		}
	}
}