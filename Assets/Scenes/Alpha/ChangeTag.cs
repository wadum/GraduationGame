using UnityEngine;
using System.Collections;

public class ChangeTag : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (this.tag == "Breakable Wall" && RecordMaster.time > 0.4)
            this.tag = "Finish";
        else if (RecordMaster.time <= 0.4f)
            this.tag = "Breakable Wall";

                
	}
}
