using UnityEngine;
using System.Collections;

public class ResetPopUp : MonoBehaviour {
    public bool Popup;
    public GameObject panel;

    void OnGUI()
    {
        var w = 250;
        var h = 120;
        if (Popup)
        {
            Rect windowRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

            GUI.color = new Color(1, 1, 0, 1f);
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Reset All Saved Progress");
        }
    }
    void DoMyWindow(int windowID)
    {
        panel.SetActive(false);
        var w = 200;
        var bw = 100;
        var h = 20;
        GUI.color = new Color(1, 1, 0, 1f);
        GUI.Label(new Rect((250 - w) / 2, h, 250, h), "Are you sure you want to reset?");
        GUI.Label(new Rect((250 - w) / 2, h * 2, 250, h), "All progress will be lost.");
        if (GUI.Button(new Rect((250 - bw) / 2, h * 3, bw, h), "Yes"))
        {
            panel.SetActive(true);
            SaveLoad.saveLoad.ResetEverything();
            Popup = false;

        }
        if (GUI.Button(new Rect((250 - bw) / 2, h * 4, bw, h), "No"))
        {
            panel.SetActive(true);
            Popup = false;
        }
    }

}
