using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionPoint : MonoBehaviour
{
    int productCount;
    HashSet<DistributionPoint> connections = new HashSet<DistributionPoint>();
    [SerializeField] float productTimer;
    float actualTime = 0;
    [SerializeField] int maximumGoods;
    [SerializeField] Transform[] productSpawnPoints;

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

    public bool IsConnectedTo(DistributionPoint distributionPoint) { return connections.Contains(distributionPoint); }
    public void AddConnection(DistributionPoint distributionPoint) { connections.Add(distributionPoint); }

}
