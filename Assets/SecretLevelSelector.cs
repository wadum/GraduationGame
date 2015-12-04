using System.Collections;
using System.Linq;
using UnityEngine;

public class SecretLevelSelector : MonoBehaviour {

    public int[] SecretSequence;

    private LoadLevel[] _loaders;

	void Start () {
	    _loaders = FindObjectsOfType<LoadLevel>();
	    if (!SecretSequence.Any()) {
	        Destroy(this);
	        return;
	    }

	    StartCoroutine(ReadSecretSequence());
	}

    private int? Raycast() {
        if (!Input.GetMouseButtonDown(0))
            return null;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return null;

        var loader = _loaders.FirstOrDefault(l => l.gameObject == hit.collider.gameObject);
        if (!loader)
            return null;

        return loader.level;
    }

    private IEnumerator ReadSecretSequence() {
        var index = 0;
        var sequenceLength = SecretSequence.Length;
        while (index < sequenceLength) {
            yield return null;

            var level = Raycast();
            if (!level.HasValue)
                continue;

            if (SecretSequence[index] == level.Value)
                index++;
            else
                index = 0;
        }

        Application.LoadLevel("ConducterFixScene");
    }
}