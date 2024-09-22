using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    float velocity = 0;
    bool move = false;
    Vector3 origin;
    Vector3 direction;
    ProductionPoint originPoint;
    DistributionPoint targetPoint;
    float currentDistance, totalDistance;

    public void SetVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    public void StartMovement(ProductionPoint productionPoint, DistributionPoint distributionPoint, float velocity)
    {
        originPoint = productionPoint;
        targetPoint = distributionPoint;
        this.velocity = velocity;
        origin = productionPoint.transform.position;
        direction = distributionPoint.transform.position - origin;
        totalDistance = direction.magnitude;
        direction = direction.normalized;

        transform.position = origin;
        currentDistance = 0;
        
        move = true;
    }

    private void Arrive()
    {
        targetPoint.ReceiveProduct(originPoint.owner);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (move)
        {
            currentDistance += Time.deltaTime * velocity;
            transform.position = origin + direction * currentDistance;

            if (currentDistance >= totalDistance)
            {
                Arrive();
            }
        }
    }
}
