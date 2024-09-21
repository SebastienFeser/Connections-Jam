using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI : MonoBehaviour
{
    private float earningFactor, costFactor, excessFactor;
    private Gang gang;

    private static float productPrice = 10; // for now

    private float SimpleFlow(ProductionPoint productionPoint, DistributionPoint distributionPoint, bool addPoint = false) // estimate flow between two nodes using one-step approximation
    {
        bool connected = distributionPoint.IsConnectedTo(productionPoint);
        if (!addPoint && !connected) return 0;

        float partition = productionPoint.connections.Sum(dp => dp.demandFrequency / dp.connections.Count);

        if (addPoint && !connected) partition += distributionPoint.demandFrequency / (distributionPoint.connections.Count + 1);

        float fraction = partition > 0 ? distributionPoint.demandFrequency / partition : 0;

        return Mathf.Min(fraction * productionPoint.productFrequency, distributionPoint.demandFrequency);
    }

    private float SimpleExcess(ProductionPoint productionPoint) // estimate excess of production using one-step approximation
    {
        return productionPoint.productFrequency - productionPoint.connections.Sum(dp => dp.demandFrequency / (dp.connections.Count));
    }

    private float PlanConnection(out ProductionPoint productionPoint, out DistributionPoint distributionPoint)
    {
        productionPoint = null;
        distributionPoint = null;
        float maxReward = Mathf.NegativeInfinity;

        foreach (ProductionPoint pp in gang.productionPoints)
        {
            float excess = SimpleExcess(pp);
            foreach (DistributionPoint dp in Level.distributionPoints)
            {
                if (!pp.IsConnectedTo(dp))
                {
                    float earning = SimpleFlow(pp, dp, true) * productPrice;
                    float cost = (dp.transform.position - pp.transform.position).magnitude * Level.connectionCostPerUnit + Level.connectionBaseCost;
                    float currentReward = earningFactor * earning - costFactor * cost + excessFactor * excess;

                    if (maxReward < currentReward)
                    {
                        productionPoint = pp;
                        distributionPoint = dp;
                        maxReward = currentReward;
                    }
                }
            }
        }

        return maxReward;
    }
}
