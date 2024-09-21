using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionPoint : MonoBehaviour
{
    int productDemand;
    int askedProducts;
    [SerializeField] float demandTimer;
    float actualTime = 0;
    [SerializeField] int maximumDemand;
    public List<ProductionPoint> connections = new List<ProductionPoint>();
    [SerializeField] Transform[] productDemandSpawnPoints;
    GameObject[] demandAroundDP = new GameObject[4];
    [SerializeField] GameObject demandGameObject;
    Level level;
    public int ProductionDemand
    {
        get { return productDemand; }
    }

    public float demandFrequency { get { return 1 / (demandTimer + Mathf.Epsilon); } }

    public bool IsConnectedTo(ProductionPoint productionPoint) { return connections.Contains(productionPoint); }
    public void AddConnection(ProductionPoint productionPoint) { connections.Add(productionPoint); }

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
                productDemand++;
                askedProducts++;
                SpawnDemand();
            }
            actualTime -= demandTimer;
        }
        if(askedProducts > 0)
        {
            RequestGoods();
        }
    }

    public void RequestGoods()
    {
        bool hasAskedDemand = false;
        foreach(ProductionPoint element in connections)
        {
            hasAskedDemand = element.AskProducts(this);
        }
        if(hasAskedDemand)
        {
            askedProducts--;
        }
    }

    void SpawnDemand()
    {
        for (int i = 0; i < demandAroundDP.Length; i++)
        {
            if (demandAroundDP[i] == null)
            {
                GameObject newDemand = Instantiate(demandGameObject, transform);
                newDemand.transform.position = productDemandSpawnPoints[i].position;
                demandAroundDP[i] = newDemand;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Product")
        {
            if(productDemand > 0)
            {
                for(int i = demandAroundDP.Length -1; i >= 0; i--)
                {
                    if (demandAroundDP[i] != null)
                    {
                        Destroy(demandAroundDP[i]);
                        break;
                    }
                }
                productDemand--;
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
