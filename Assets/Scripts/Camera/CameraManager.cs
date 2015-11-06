using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public WorldViewController WorldView;
    public GameObject CameraContainer;
    public GameObject Cam;
    public TopDownCamController TopDownView;

    public bool WorldViewMode;

	void Start ()
	{
        if (WorldViewMode)
        {
			TopDownView.enabled = false;
			WorldView.enabled = true;
            CameraContainer.transform.position = WorldView.GetCurrentTransform().position;
            CameraContainer.transform.rotation = WorldView.GetCurrentTransform().rotation;
            WorldView.Run(CameraContainer);
        } else
        {
			WorldView.enabled = false;
			TopDownView.enabled = true;
            Cam.transform.parent = transform;
            Cam.transform.localPosition = TopDownView.StartingPos;
            TopDownView.Run(Cam);
        }
	}
	
}
