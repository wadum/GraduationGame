using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class I18n {
	
	public enum LanguageKeys {
		Danish, English
	};

	LanguageKeys chosenLanguage;
	private static I18n instance;
	private Dictionary<string,string> fields;
	
	public I18n(){
		fields = new Dictionary<string, string>();
		LoadLanguage (LanguageKeys.English);
	}
	
	public static I18n GetInstance(){
		if (instance == null){
			instance = new I18n();
		}
		return instance;
	}
	
	public LanguageKeys GetLanguage(){
		return chosenLanguage;
	}

	public string GetLanguageName(){
		return chosenLanguage.ToString();
	}
	
	public void LoadLanguage(LanguageKeys lang){
		chosenLanguage = lang;
		fields.Clear();
		TextAsset textAsset=Resources.Load<TextAsset>("Texts/" + GetLanguageName() + "/buttons");
		if(textAsset==null){
			return;
		}
		string[] lines = textAsset.text.Split (new char[] {'\n', '\r'},StringSplitOptions.RemoveEmptyEntries);
		for(int i=0;i < lines.Length;i++){
			if(lines[i].IndexOf("=") >= 0 && !lines[i].StartsWith("#")){
				string[] keyValue = lines[i].Split(new char[] { '=' }, 2);
				fields.Add(keyValue[0], keyValue[1].Replace("\\n", "\n"));
			}
		}
	}
	
	public string Translate(string i18nKey){
		if (fields.ContainsKey(i18nKey)) {
			return fields[i18nKey];
		}
		return "$" + i18nKey + "$";
	}
}