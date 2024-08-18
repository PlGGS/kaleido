using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float wallLength = 10f; // Adjust this to your wall length

    // Start is called before the first frame update
    void Start()
    {
        AdjustCameraDistance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AdjustCameraDistance()
    {
        if (mainCamera != null)
        {
            float fov = mainCamera.fieldOfView;
            float distance = wallLength / (2 * Mathf.Tan(fov * Mathf.Deg2Rad / 2));
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -distance);
            mainCamera.transform.LookAt(Vector3.zero); // Adjust if needed to keep the camera looking at the center
        }
    }
}
