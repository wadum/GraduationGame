using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public WorldViewController WorldView;
    public GameObject CameraContainer;
    public GameObject Cam;
    public TopDownCamController TopDownView;

    public bool WorldViewMode = false;

	void Start ()
	{
        if (WorldViewMode)
        {
            CameraContainer.transform.position = WorldView.GetCurrentTransform().position;
            CameraContainer.transform.rotation = WorldView.GetCurrentTransform().rotation;
            WorldView.Run(CameraContainer);
        } else
        {
            Cam.transform.position = TopDownView.StartingPos;
            TopDownView.Run(Cam);
        }
	}
	
}
