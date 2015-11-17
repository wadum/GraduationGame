using UnityEngine;
using System.Collections;

public class AddLethalCollisionOnTimeObjects : MonoBehaviour {

    public float DeathWaitTime = 2.0f;
    public Shader DissolverShader;

    public GameObject _playerBody;
    public GameObject _playerEyes;
    public GameObject _playerHair;

    private Material _playerBodyMaterial;
    private Material _playerEyesMaterial;
    private Material _playerHairMaterial;

    private const string timeManipulationObject = "TimeManipulationObject";
    private float _timePos;
    private bool _killing;
    private Respawnable _respawn;

    // Use this for initialization
    void Start () {
        _playerBodyMaterial = _playerBody.GetComponent<Renderer>().material;
        _playerEyesMaterial = _playerEyes.GetComponent<Renderer>().material;
        _playerHairMaterial = _playerHair.GetComponent<Renderer>().material;
        _respawn = GameObject.FindGameObjectWithTag("Player").GetComponent<Respawnable>();
        
        foreach (var timeObject in GameObject.FindGameObjectsWithTag(timeManipulationObject))
        {
            var col = GetComponent<MeshCollider>();
            if (!col)
                col = timeObject.AddComponent<MeshCollider>();

            col.convex = true;

            var rb = GetComponent<Rigidbody>();
            if (!rb)
                rb = timeObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != timeManipulationObject)
            return;

        var someTime = GetTime(col);
        if (!someTime)
            return;

        if (someTime.Moving)
            StartCoroutine(MurderTheBitch());
    }
    
    private ObjectTimeController GetTime(Collider col)
    {
        var parent = col.transform.parent;
        ObjectTimeController targetController = null;

        while (parent != null && !targetController)
        {
            targetController = parent.GetComponent<ObjectTimeController>();
            parent = parent.parent;
        }

        return targetController;
    }

    public IEnumerator MurderTheBitch()
    {
        if (!_killing)
        {
            _killing = true;
            float _disAmount = 0;
            while (_disAmount < 1)
            {
                _playerBodyMaterial.SetFloat("_DissolveAmount", _disAmount);
                _playerEyesMaterial.SetFloat("_DissolveAmount", _disAmount);
                _playerHairMaterial.SetFloat("_DissolveAmount", _disAmount);

                _disAmount += Time.deltaTime / DeathWaitTime;
                yield return null;
            }

            transform.parent.parent = null;
            this.gameObject.transform.parent.GetComponentInChildren<AnimationController>().StopMagic();
            GameOverlayController.gameOverlayController.DeactivateSlider();
            _respawn.Respawn();
            _playerBodyMaterial.SetFloat("_DissolveAmount", 0);
            _playerEyesMaterial.SetFloat("_DissolveAmount", 0);
            _playerHairMaterial.SetFloat("_DissolveAmount", 0);
            _killing = false;
        }
    }
}