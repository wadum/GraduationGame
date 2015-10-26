using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MightyMorphingCube : TimeTraveller
{
    public TimePeriod DefaultTimePeriod = TimePeriod.Present;

    private TimePeriod _timePeriod;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Mesh _pastMesh;
    private Mesh _presentMesh;
    private Mesh _futureMesh;
    
	// Use this for initialization
	void Start ()
	{
	    _timePeriod = DefaultTimePeriod;
	    _meshFilter = GetComponent<MeshFilter>();
	    _meshCollider = GetComponent<MeshCollider>();
        MakeMeshs();
        ResetToDefaultTimePeriod();
	}

    private void MakeMeshs()
    {
        _pastMesh = Instantiate(_meshFilter.mesh);
        _presentMesh = Instantiate(_meshFilter.mesh);
        _futureMesh = Instantiate(_meshFilter.mesh);

        Func<Vector3, Vector3> nudgeVertex = v => new Vector3(Random.value/2, Random.value/2, Random.value/2) + v;
        Action<Mesh> nudgeMesh = m => m.SetVertices(m.vertices.Select(nudgeVertex).ToList());

        nudgeMesh(_futureMesh);
        _futureMesh.RecalculateNormals();
        _futureMesh.RecalculateBounds();
        
        nudgeMesh(_pastMesh);
        _pastMesh.RecalculateNormals();
        _pastMesh.RecalculateBounds();

        nudgeMesh(_presentMesh);
        _presentMesh.RecalculateNormals();
        _presentMesh.RecalculateBounds();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.A))
	        SetPast();
        else if (Input.GetKeyDown(KeyCode.S)) 
            SetPresent();
        else if (Input.GetKeyDown(KeyCode.D))
            SetFuture();
	}

    public override TimePeriod GetCurrentTimePeriod()
    {
        return _timePeriod;
    }

    public override TimePeriod GetDefaultTimePeriod()
    {
        return DefaultTimePeriod;
    }

    public override void SetPast()
    {
        _meshCollider.sharedMesh = _meshFilter.sharedMesh = _pastMesh;
        
    }

    public override void SetPresent()
    {
        _meshCollider.sharedMesh = _meshFilter.sharedMesh = _presentMesh;
    }

    public override void SetFuture()
    {
        _meshCollider.sharedMesh = _meshFilter.sharedMesh = _futureMesh;
    }
}
