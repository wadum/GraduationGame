using UnityEngine;
using System.Collections;

public class Dissolver : MonoBehaviour {
    Renderer rend;
    public float AmountOfTime = 2f;
    private float _disAmount = 0, _timeAmound = 0;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (!rend)
            Destroy(this);
    }

    public IEnumerator Dissolve()
    {
        _disAmount = 0;
        _timeAmound = 0;
        AmountOfTime = 2f;
        while (_disAmount < 1)
        {
            rend.material.SetFloat("_DissolveAmount", _disAmount);
            _timeAmound += Time.deltaTime;
            _disAmount = _timeAmound / AmountOfTime;
            yield return null;
        }
    }

    public IEnumerator Resolve()
    {
        _disAmount = 0;
        _timeAmound = 0;
        AmountOfTime = 2f;
        while (_disAmount < 1)
        {
            rend.material.SetFloat("_DissolveAmount", 1-_disAmount);
            _timeAmound += Time.deltaTime;
            _disAmount = _timeAmound / AmountOfTime;
            yield return null;
        }
    }

}
