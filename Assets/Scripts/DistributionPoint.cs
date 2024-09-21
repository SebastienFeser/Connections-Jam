using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionPoint : MonoBehaviour
{
    int productDemand { get { return demandAroundDP.Count; } }
    public int askedProducts;
    [SerializeField] float demandTimer;
    float actualTime = 0;
    [SerializeField] int maximumDemand;
    public List<ProductionPoint> connections = new List<ProductionPoint>();
    [SerializeField] Transform[] productDemandSpawnPoints;
    List<GameObject> demandAroundDP = new List<GameObject>();
    [SerializeField] GameObject demandGameObject;
    public int ProductionDemand
    {
        get { return productDemand; }
    }

    public float demandFrequency { get { return 1 / (demandTimer + Mathf.Epsilon); } }

    public bool IsConnectedTo(ProductionPoint productionPoint) { return connections.Contains(productionPoint); }
    public void AddConnection(ProductionPoint productionPoint) { connections.Add(productionPoint); }

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
        //...
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
