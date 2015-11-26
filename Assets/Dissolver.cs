using UnityEngine;
using System.Collections;

public class Dissolver : MonoBehaviour {
    Renderer rend;
    public float AmountOfTime;
    private float _AmountOfTime;
    private float _disAmount = 0, _timeAmound = 0;

    void Start()
    {
        _AmountOfTime = AmountOfTime;
    }
    public Dissolver(float T)
    {
        AmountOfTime = T;
    }

    public IEnumerator Dissolve()
    {
        rend = GetComponent<Renderer>();
        if(!rend)
            yield return null;
        _disAmount = 0;
        _timeAmound = 0;
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
        rend = GetComponent<Renderer>();
        if (!rend)
            yield return null;
        _disAmount = 0;
        _timeAmound = 0;
        AmountOfTime = 3;
        while (_disAmount < 1)
        {
            rend.material.SetFloat("_DissolveAmount", 1-_disAmount);
            _timeAmound += Time.deltaTime;
            _disAmount = _timeAmound / AmountOfTime;
            yield return null;
        }
    }

}
