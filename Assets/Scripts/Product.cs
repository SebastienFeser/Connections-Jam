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

    public void StartMovement(Transform start, Transform destination, float newVelocity, DistributionPoint newDistributionPoint)
    {
        velocity = newVelocity;
        startTransform = start;
        destinationTransform = destination;

        transform.position = startTransform.position;
        direction = destinationTransform.position - startTransform.position;
        distributionPoint = newDistributionPoint;
        move = true;
    }

    private void Awake()
    {
        move = false;
    }
    private void Update()
    {
        if(move) transform.position += direction * Time.deltaTime * velocity;
    }
}
