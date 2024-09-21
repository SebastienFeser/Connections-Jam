using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Camera camera;

    private Vector3 mouseOrigin;
    [SerializeField] float zoomStep;
    [SerializeField] float minimumZoom;
    [SerializeField] float maximumZoom;

    private void Update()
    {
        MouseCameraMovement();
        //Zoom();
    }

    private void MouseCameraMovement()
    {
        if(Input.GetMouseButtonDown(2))
        {
            mouseOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Input.GetMouseButton(2))
        {
            Vector3 difference = mouseOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += difference;
        }
    }

    private void Zoom()
    {
        float newSize = camera.orthographicSize + zoomStep * Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize = Mathf.Clamp(newSize, minimumZoom, maximumZoom);
    }

}
