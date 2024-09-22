using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionPoint : MonoBehaviour
{
    int productDemand { get { return demandAroundDP.Count; } }
    int askedProducts;
    public float localProductPrice = 10;
    [SerializeField] float demandTimer;
    float actualTime = 0;
    [SerializeField] int maximumDemand;
    public List<ProductionPoint> connections = new List<ProductionPoint>();
    public Dictionary<Gang, float> influence = new Dictionary<Gang, float>();
    [SerializeField] Transform[] productDemandSpawnPoints;
    List<GameObject> demandAroundDP = new List<GameObject>();
    [SerializeField] GameObject demandGameObject;
    Level level;
    public int ProductionDemand
    {
        get { return productDemand; }
    }

    public float demandFrequency { get { return 1 / (demandTimer + Mathf.Epsilon); } }

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

    private void Start()
    {
        level = FindObjectOfType<Level>();
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
