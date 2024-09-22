using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum InvestmentProfile { safe, risky }

public class AI : MonoBehaviour
{
    private float earningFactor, costFactor, excessFactor;
    private InvestmentProfile profile;
    private Gang gang;

    private float connectionTimerDuration = 5f;
    private float initialTimerDuration = 3f;
    private float replanTimerDuration = 1f;
    private float investmentTimerDuration = 20f;
    private float timer, investmentTimer;

    private float deltaInvestment = 25f;

    private bool activated = false;

    public void SetGang(Gang gang, float earningFactor = 8f, float costFactor = 0.05f, float excessFactor = 1f, InvestmentProfile profile = InvestmentProfile.risky)
    {
        this.gang = gang;
        this.earningFactor = earningFactor;
        this.costFactor = costFactor;
        this.excessFactor = excessFactor;
        this.profile = profile;

        timer = initialTimerDuration;
        investmentTimer = investmentTimerDuration;
        activated = true;
    }

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
                    float earning = SimpleFlow(pp, dp, true) * dp.productPrice;
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

    private float PlanInvestment(out DistributionPoint distributionPoint, out float investment)
    {
        HashSet<DistributionPoint> distributionPoints = new HashSet<DistributionPoint>();
        foreach (ProductionPoint pp in gang.productionPoints) distributionPoints.UnionWith(pp.connections);

        distributionPoint = null;
        investment = 0;
        float maxReward = Mathf.NegativeInfinity;

        foreach (DistributionPoint dp in distributionPoints)
        {
            for (int i = 0; i < Mathf.FloorToInt(Mathf.Min(gang.money, maxInvestment()) / deltaInvestment); i++)
            {
                float deltaEarning = dp.Earning(gang, i*deltaInvestment) - dp.Earning(gang);
                float currentReward = earningFactor * deltaEarning - i*deltaInvestment;

                if (currentReward > maxReward)
                {
                    distributionPoint = dp;
                    investment = i * deltaInvestment;
                    maxReward = currentReward;
                }
            }
        }

        return maxReward;
    }

    private float maxInvestment()
    {
        switch(profile)
        {
            case InvestmentProfile.safe:
                return 100f;
            default:
            case InvestmentProfile.risky:
                return gang.money;
        }
    }

    private void play()
    {
        // invest
        if (investmentTimer <= 0)
        {
            float projectedReward = PlanInvestment(out DistributionPoint distributionPoint, out float investment);
            if (projectedReward > 0)
            {
                if (gang.Pay(-investment))
                {
                    Debug.Log(gang.name + " invested!");
                    distributionPoint.IncrementInfluence(gang, investment);
                    investmentTimer = investmentTimerDuration;
                }
                else investmentTimer = replanTimerDuration;
            }
            else investmentTimer = replanTimerDuration;
        }

        // create a connection
        if (gang.money >= Level.connectionBaseCost && timer <= 0)
        {
            float projectedReward = PlanConnection(out ProductionPoint productionPoint, out DistributionPoint distributionPoint);
            if (projectedReward > 0)
            {
                DraggableConnection draggableConnection = productionPoint.GetComponent<DragConnection>().createConnection();
                float cost = draggableConnection.Cost(distributionPoint.transform.position);
                if (draggableConnection.AddConnection(distributionPoint, cost))
                {
                    // success
                    timer = connectionTimerDuration;
                }
                else
                {
                    // not enough money or failure
                    timer = replanTimerDuration;
                    Destroy(draggableConnection.gameObject);
                }
            }
            else timer = replanTimerDuration;
        }

    }

    private void Update()
    {
        if (activated)
        {
            if (timer > 0) timer -= Time.deltaTime;
            if (investmentTimer > 0) investmentTimer -= Time.deltaTime;
            play();
        }
    }
}
