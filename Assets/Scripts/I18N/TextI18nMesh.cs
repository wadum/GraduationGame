using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class TextI18nMesh : MonoBehaviour {
	
	public string TranslationKey;
	
	private TextMesh text;
	
	void Start(){
		text = GetComponentInParent<TextMesh>();
		UpdateField ();
	}
	
	public void UpdateField(){
		if (I18n.GetInstance () != null && text != null) {
			text.text = I18n.GetInstance ().Translate (TranslationKey);
		}
	}
}