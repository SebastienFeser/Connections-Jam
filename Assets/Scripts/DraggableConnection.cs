using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableConnection : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector2 startPosition = Vector2.zero;
    Vector2 mousePosition;

    public bool placed;

    private int layer_mask;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        layer_mask = 1 << LayerMask.NameToLayer("Default");
        placed = false;

    }
    public void SetStartPosition(Vector2 position)
    {
        startPosition = position;
    }

    private bool CheckUnderMouse(out RaycastHit2D hit)
    {
        Ray ray = new Ray(new Vector3(mousePosition.x, mousePosition.y, -0.1f), Vector3.forward);
        hit = Physics2D.GetRayIntersection(ray, 1f, layer_mask);

        return !(hit.collider is null);
    }

    private void Update()
    {
        if (!placed)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, 0f));
                lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0f));
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (CheckUnderMouse(out RaycastHit2D hit))
                {
                    DistributionPoint distribution_point = hit.transform.GetComponent<DistributionPoint>();
                    if (!(distribution_point is null))
                    {
                        Debug.Log("Hey");

                        // Add connection to distribution point if it does not exist already
                        // ...
                        lineRenderer.SetPosition(1, hit.transform.position);
                        placed = true;
                    }
                    else Destroy(gameObject);
                }
                else Destroy(gameObject);
            }
        }
    }
}
