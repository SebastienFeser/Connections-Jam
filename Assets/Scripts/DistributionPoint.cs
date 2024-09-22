using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DistributionSize
{
    isolated = 0,
    small = 1,
    medium = 2,
    large = 3
}

public class DistributionPoint : MonoBehaviour
{
    public int productDemand { get { return demandAroundDP.Count; } }
    public int askedProducts;
    public float localProductPrice = 10;
    [SerializeField] float demandTimer;
    float actualTime = 0;
    [SerializeField] int maximumDemand;
    public List<ProductionPoint> connections = new List<ProductionPoint>();
    public Dictionary<Gang, float> influence = new Dictionary<Gang, float>();
    [SerializeField] Transform[] productDemandSpawnPoints;
    public List<GameObject> demandAroundDP = new List<GameObject>();
    [SerializeField] GameObject demandGameObject;

    public DistributionSize size;

    private float upgradeProbability = 0f;
    private float spreadProbability = 0f;

    public int ProductionDemand
    {
        get { return productDemand; }
    }

    public float demandFrequency { get { return 1 / (demandTimer + Mathf.Epsilon); } }

    public void Initialize(DistributionSize size = DistributionSize.isolated)
    {
        ChangeSize(size);
    }

    public void ChangeSize(DistributionSize size)
    {
        this.size = size;
        switch (size)
        {
            default:
            case DistributionSize.isolated:
                demandTimer = 15f;
                upgradeProbability = 0.1f;
                spreadProbability = 0f;
                break;
            case DistributionSize.small:
                demandTimer = 10f;
                upgradeProbability = 0.05f;
                spreadProbability = 0f;
                break;
            case DistributionSize.medium:
                demandTimer = 5f;
                upgradeProbability = 0.01f;
                spreadProbability = 0.05f;
                break;
            case DistributionSize.large:
                demandTimer = 2f;
                upgradeProbability = 0f;
                spreadProbability = 0.1f;
                break;
        }
    }

    public void Upgrade()
    {
        switch (size)
        {
            case DistributionSize.isolated:
                ChangeSize(DistributionSize.small);
                break;
            case DistributionSize.small:
                ChangeSize(DistributionSize.medium);
                break;
            case DistributionSize.medium:
                ChangeSize(DistributionSize.large);
                break;
        }
    }

    public void Downgrade()
    {
        switch (size)
        {
            case DistributionSize.small:
                ChangeSize(DistributionSize.isolated);
                break;
            case DistributionSize.medium:
                ChangeSize(DistributionSize.small);
                break;
            case DistributionSize.large:
                ChangeSize(DistributionSize.medium);
                break;
        }
    }

    public static bool StrictlyGreater(DistributionSize a, DistributionSize b) { return (int)a > (int)b; }
    public static bool StrictlySmaller(DistributionSize a, DistributionSize b) { return (int)a < (int)b; }

    public bool IsConnectedTo(ProductionPoint productionPoint) { return connections.Contains(productionPoint); }
    public void AddConnection(ProductionPoint productionPoint)
    {
        connections.Add(productionPoint);
        influence[productionPoint.owner] = 1;
    }

    public void IncrementInfluence(Gang gang, float additionnalInfluence)
    {
        influence[gang] += additionnalInfluence;
    }

    public float GetInfluence(Gang gang)
    {
        return influence[gang];
    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime > demandTimer)
        {
            if (productDemand < maximumDemand)
            {
                SpawnDemand();
            }
            actualTime -= demandTimer;
        }
        if(productDemand > askedProducts)
        {
            RequestGoods();
        }
    }

    public void RequestGoods()
    {
        bool hasAskedDemand = false;
        foreach(ProductionPoint element in connections)
        {
            if (element.AskProducts(this)) hasAskedDemand = true;
        }
        if(hasAskedDemand)
        {
            askedProducts++;
        }
    }

    void SpawnDemand()
    {
        GameObject newDemand = Instantiate(demandGameObject, transform);
        newDemand.transform.position = productDemandSpawnPoints[productDemand].position;
        demandAroundDP.Add(newDemand);
    }

    private void Payout()
    {
        float sum = influence.Values.Sum();
        foreach (KeyValuePair<Gang,float> i in influence)
        {
            i.Key.Pay(localProductPrice * i.Value / sum);
        }

        if (UnityEngine.Random.Range(0f,1f) < upgradeProbability) Upgrade(); // random upgrade
        if (UnityEngine.Random.Range(0f, 1f) < spreadProbability) Level.Spread(this); // random spread
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Product")
        {
            if(productDemand > 0)
            {
                Destroy(demandAroundDP[productDemand - 1]);
                demandAroundDP.RemoveAt(productDemand - 1);
                askedProducts--;
                Payout();
            }

            Destroy(collision.gameObject);
        }
    }
}
