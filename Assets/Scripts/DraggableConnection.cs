using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableConnection : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector2 startPosition = Vector2.zero;
    Vector2 mousePosition;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void SetStartPosition(Vector2 position)
    {
        startPosition = position;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, 0f));
            lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0f));
        }
        if(Input.GetMouseButtonUp(0))
        {
            Destroy(gameObject);
        }
    }
}
