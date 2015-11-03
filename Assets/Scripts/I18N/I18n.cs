using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class I18n {
	
	public static int DANISH = 0;
	public static int ENGLISH = 1;
	public int chosenLanguage;
	public static I18n instance;
	
	private Dictionary<string,string> fields;
	
	public I18n(){
		fields = new Dictionary<string, string>();
		//LoadLanguage ("en");
		chosenLanguage = ENGLISH;
	}
	
	public static I18n GetInstance(){
		if (instance == null){
			instance = new I18n();
		}
		return instance;
	}
	
	public int GetLanguage(){
		return chosenLanguage;
	}

	public string GetLanguageName(){
		return (chosenLanguage == DANISH) ? "Danish" : "English";
	}
	
	public void LoadLanguage(string lang){
		fields.Clear();
		TextAsset textAsset=Resources.Load<TextAsset>("I18N/"+lang);
		if(textAsset==null){
			return;
		}
		string[] lines = textAsset.text.Split (new char[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries);
		for(int i=0;i < lines.Length;i++){
			if(lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#")){
				string[] keyValue = lines[i].Split(new char[] { '=' }, 2);
				fields.Add(keyValue[0], keyValue[1]);
			}
		}
		if (lang == "en") {
			chosenLanguage = ENGLISH;
		} else {
			chosenLanguage = DANISH;
		}
	}
	
	public string translate(string i18nKey){
		if (fields.ContainsKey(i18nKey)) {
			return fields[i18nKey];
		}
		return "$" + i18nKey + "$";
	}
}