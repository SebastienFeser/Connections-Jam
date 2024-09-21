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

    public bool IsConnectedTo(ProductionPoint productionPoint) { return connections.Contains(productionPoint); }
    public void AddConnection(ProductionPoint productionPoint) { connections.Add(productionPoint); }
}
