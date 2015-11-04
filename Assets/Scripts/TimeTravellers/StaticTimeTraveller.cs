using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class StaticTimeTraveller : BaseTimeTraveller
{
    public TimePeriod DefaultTimePeriod;

    public Mesh PastMesh;
    public Mesh PresentMesh;
    public Mesh FutureMesh;

    private TimePeriod _currenTimePeriod;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();

        switch (DefaultTimePeriod)
        {
            case TimePeriod.Past:
                SetMesh(PastMesh);
                break;
            case TimePeriod.Present:
                SetMesh(PresentMesh);
                break;
            case TimePeriod.Future:
                SetMesh(FutureMesh);
                break;
        }

        _currenTimePeriod = DefaultTimePeriod;
    }

    public override TimePeriod GetCurrentTimePeriod()
    {
        return _currenTimePeriod;
    }

    public override TimePeriod GetDefaultTimePeriod()
    {
        return DefaultTimePeriod;
    }

    public override void SetPast()
    {
        if (_currenTimePeriod == TimePeriod.Past)
            return;

        _currenTimePeriod = TimePeriod.Past;
        SetMesh(PastMesh);
    }

    public override void SetPresent()
    {
        if (_currenTimePeriod == TimePeriod.Present)
            return;

        _currenTimePeriod = TimePeriod.Present;
        SetMesh(PresentMesh);
    }

    public override void SetFuture()
    {
        if (_currenTimePeriod == TimePeriod.Future)
            return;

        _currenTimePeriod = TimePeriod.Future;
        SetMesh(FutureMesh);
    }

    private void SetMesh(Mesh mesh)
    {
        _meshFilter.mesh = mesh;

        if (_meshCollider)
            _meshCollider.sharedMesh = mesh;
    }
}