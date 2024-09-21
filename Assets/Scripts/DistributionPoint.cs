using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionPoint : MonoBehaviour
{
    int productDemand;
    List<ProductionPoint> connections = new List<ProductionPoint>();
    public int ProductionDemand
    {
        get { return productDemand; }
    }

    public bool AddConnection(ProductionPoint productionPoint)
    {
        if (!connections.Contains(productionPoint))
        {
            connections.Add(productionPoint);
            return true;
        }
        else
        {
            return false;
        }
    }


}
