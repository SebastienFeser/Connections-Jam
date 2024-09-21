using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    float velocity = 0;
    bool move = false;
    Transform startTransform;
    Transform destinationTransform;
    Vector3 direction;
    DistributionPoint distributionPoint;

    public void SetVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    public void StartMovement(Transform start, Transform destination, float newVelocity, DistributionPoint distributionPoint)
    {
        velocity = newVelocity;
        startTransform = start;
        destinationTransform = destination;

        transform.position = startTransform.position;
        direction = startTransform.position - destinationTransform.position;
        move = true;
    }

    private void Start()
    {
        move = false;
    }
    private void Update()
    {
        if(move)
        {
            transform.position += direction * Time.deltaTime * velocity;
            if(Vector3.Distance(transform.position, destinationTransform.position) < 0.5f)
            {
                if(distributionPoint.ProductionDemand > 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}
