using UnityEngine;
using System.Collections;

public class KillHerDetector : MonoBehaviour {

    public float DeathWaitTime = 3.0f;
    public Transform SpawnLocation;
    public Shader DissolverShader;

    public GameObject _playerBody;
    public GameObject _playerEyes;
    public GameObject _playerHair;

    Material _playerBodyMaterial;
    Material _playerEyesMaterial;
    Material _playerHairMaterial;

    GameObject _player;
    float _oldTimePos;
    bool _Killing;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _playerBodyMaterial = _playerBody.GetComponent<Renderer>().material;

        _playerEyesMaterial = _playerEyes.GetComponent<Renderer>().material;

        _playerHairMaterial = _playerHair.GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider _object)
    {
        if (_object.gameObject.tag == "Player")
        {
            _object.GetComponent<Respawnable>().SetTransformLocationAsRespawn(SpawnLocation);
            _oldTimePos = this.gameObject.GetComponentInParent<ObjectTimeController>().TimePos;
        }
    }

    void OnTriggerStay(Collider _object)
    {
        
        if (this.gameObject.GetComponentInParent<ObjectTimeController>().TimePos != _oldTimePos && !_Killing)
        {
            _Killing = true;
            StartCoroutine(KillTheBitch());
        }
    }

    IEnumerator KillTheBitch()
    {
        float _disAmount = 0;
        while (_disAmount < 1)
        {
            _playerBodyMaterial.SetFloat("_DissolveAmount", _disAmount);
            _playerEyesMaterial.SetFloat("_DissolveAmount", _disAmount);
            _playerHairMaterial.SetFloat("_DissolveAmount", _disAmount);

            _disAmount += Time.deltaTime/2;
            yield return null;
        }

        _player.GetComponent<Respawnable>().Respawn();
        _playerBodyMaterial.SetFloat("_DissolveAmount", 0);
        _playerEyesMaterial.SetFloat("_DissolveAmount", 0);
        _playerHairMaterial.SetFloat("_DissolveAmount", 0);
        _Killing = false;
    }
}
