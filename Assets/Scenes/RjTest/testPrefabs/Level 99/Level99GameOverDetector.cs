using UnityEngine;
using System.Collections;

public class Level99GameOverDetector : MonoBehaviour
{
    public float DeathWaitTime = 5;

    public Transform ZoomedInState;
    public Transform ZoomedOutState;

    public GameObject _playerBody;
    public GameObject _playerEyes;
    public GameObject _playerHair;

    private Material _playerBodyMaterial;
    private Material _playerEyesMaterial;
    private Material _playerHairMaterial;

    public Camera MainCam;

    public GameObject GameOverScreen;

    void Start()
    {
        _playerBodyMaterial = _playerBody.GetComponent<Renderer>().material;
        _playerEyesMaterial = _playerEyes.GetComponent<Renderer>().material;
        _playerHairMaterial = _playerHair.GetComponent<Renderer>().material;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.gameObject.tag == "Wind")
        {
            col.transform.root.gameObject.GetComponent<NavMeshAgent>().ResetPath();
            FindObjectOfType<Level99EnemySpawnController>().EnemiesCelebrate();
            StartCoroutine(MurderTheBitch());
            EnableDeathCollider(false);
        }
    }

    public IEnumerator MurderTheBitch()
    {
        float _disAmount = 0;
        while (_disAmount < 1)
        {
            _playerBodyMaterial.SetFloat("_DissolveAmount", _disAmount);
            _playerEyesMaterial.SetFloat("_DissolveAmount", _disAmount);
            _playerHairMaterial.SetFloat("_DissolveAmount", _disAmount);

            _disAmount += Time.deltaTime / DeathWaitTime;
            yield return null;
        }
        GameOverScreen.SetActive(true);
    }

    public void EnableDeathCollider(bool state)
    {
        GetComponent<CapsuleCollider>().enabled = state;
        //Zoomed Out state
        if (state)
        {
            _playerBodyMaterial.SetFloat("_DissolveAmount", 0);
            _playerEyesMaterial.SetFloat("_DissolveAmount", 0);
            _playerHairMaterial.SetFloat("_DissolveAmount", 0);

            MainCam.transform.localPosition = ZoomedOutState.localPosition;
            MainCam.transform.localRotation = ZoomedOutState.localRotation;
        }
        //Zoomed in state
        else
        {
            MainCam.transform.localPosition = ZoomedInState.localPosition;
            MainCam.transform.localRotation = ZoomedInState.localRotation;
        }
    }
}

