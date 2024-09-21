using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableConnection : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector2 startPosition = Vector2.zero;
    Vector2 mousePosition;
    ProductionPoint productionPointOrigin;

    public bool placed;

    private int layerMask;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Default");
        placed = false;

    }
    public void SetStartPosition(Vector2 position, ProductionPoint productionPoint)
    {
        lineRenderer = GetComponent<LineRenderer>();

        startPosition = position;
        productionPointOrigin = productionPoint;
        lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, 0f));
    }

    private bool CheckUnderMouse(out RaycastHit2D hit)
    {
        Ray ray = new Ray(new Vector3(mousePosition.x, mousePosition.y, -0.1f), Vector3.forward);
        hit = Physics2D.GetRayIntersection(ray, 1f, layerMask);

        return !(hit.collider is null);
    }

    public bool AddConnection(DistributionPoint distributionPoint, float cost)
    {
        bool failure = productionPointOrigin.IsConnectedTo(distributionPoint) || distributionPoint.IsConnectedTo(productionPointOrigin) || Level.playerGang.money - cost < 0;
        if (!failure)
        {
            lineRenderer.SetPosition(1, distributionPoint.transform.position);
            productionPointOrigin.AddConnection(distributionPoint);
            distributionPoint.AddConnection(productionPointOrigin);
            Level.playerGang.Pay(-cost);
        }

        placed = true;

        return !failure;
    }

    public float Cost(Vector2 targetPosition) { return (targetPosition - startPosition).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost; }
    public float Cost(Vector3 targetPosition) { return (targetPosition - new Vector3(startPosition.x, startPosition.y, 0)).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost; }
    public float Cost(DistributionPoint distributionPoint) { return Cost(distributionPoint.transform.position); }

    private void Update()
    {
        if (!placed)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                float cost = Level.connectionBaseCost;
                bool snapped = false;
                if (CheckUnderMouse(out RaycastHit2D hit))
                {
                    DistributionPoint distributionPoint = hit.transform.GetComponent<DistributionPoint>();
                    if (!(distributionPoint is null))
                    {
                        snapped = true;
                        cost = (hit.transform.position - new Vector3(startPosition.x, startPosition.y, 0)).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost;
                        lineRenderer.SetPosition(1, hit.transform.position);
                    }
                }
                
                if (!snapped)
                {
                    cost = (mousePosition - startPosition).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost;
                    lineRenderer.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0f));
                }

                UISystem.moneyDisplayUI.DisplayCost(cost);
            }
            if (Input.GetMouseButtonUp(0))
            {
                bool destroy = true;
                if (CheckUnderMouse(out RaycastHit2D hit))
                {
                    DistributionPoint distributionPoint = hit.transform.GetComponent<DistributionPoint>();
                    if (!(distributionPoint is null))
                    {
                        float cost = (hit.transform.position - new Vector3(startPosition.x, startPosition.y, 0)).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost;
                        UISystem.moneyDisplayUI.DisplayCost(cost);

                        if (AddConnection(distributionPoint, cost)) destroy = false;
                    }
                }

                UISystem.moneyDisplayUI.StopDisplayCost();

                if (destroy) Destroy(gameObject);
            }
        }
    }
}
