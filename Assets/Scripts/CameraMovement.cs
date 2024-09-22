using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Camera camera;

    private Vector3 mouseOrigin;
    [SerializeField] float zoomStep;
    [SerializeField] float minimumZoom;
    [SerializeField] float maximumZoom;

    PlayerInput playerInput;

    InputAction zoomAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        zoomAction = playerInput.actions.FindAction("Zoom");
    }
    private void Update()
    {
        MouseCameraMovement();
        Zoom();
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
        //Debug.Log(Input.mouseScrollDelta.y);
        float newSize = camera.orthographicSize + zoomStep * -Input.mouseScrollDelta.y;
        camera.orthographicSize = Mathf.Clamp(newSize, minimumZoom, maximumZoom);
    }

}
