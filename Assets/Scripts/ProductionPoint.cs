using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionPoint : MonoBehaviour
{
    int productCount;
    List<DistributionPoint> connections = new List<DistributionPoint>();
    [SerializeField] float productTimer;
    float actualTime = 0;
    [SerializeField] int maximumGoods;

    private void Update()
    {
        actualTime += Time.deltaTime;
        if(actualTime > productTimer)
        {
            if (productCount < maximumGoods)
            {
                productCount++;
            }
            actualTime -= productTimer;
        }

        foreach(DistributionPoint element in connections)
        {
            if(element.ProductionDemand > 0)
            {
                SendProducts(element);
            }
        }

        void SendProducts(DistributionPoint distributionPoint)
        {

        }
    }

    public bool AddConnection(DistributionPoint distributionPoint)
    {
        if (!connections.Contains(distributionPoint))
        {
            connections.Add(distributionPoint);
            return true;
        }
        else
        {
            return false;
        }
    }

}
