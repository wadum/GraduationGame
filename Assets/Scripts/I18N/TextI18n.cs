using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextI18n : MonoBehaviour {
	
	public string translationKey;
	
	private Text text;
	
	void Start(){
		Text[] texts = GetComponentsInParent<Text>();
		if (texts.Length > 0) {
			text = texts [0];
			updateField ();
		}
	}
	
	public void updateField(){
		if (I18n.GetInstance () != null && text != null) {
			text.text = I18n.GetInstance ().translate (translationKey);
		}
	}
}