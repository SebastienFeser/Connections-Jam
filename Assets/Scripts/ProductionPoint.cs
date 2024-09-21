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
    [SerializeField] Transform[] productSpawnPoints;

    [SerializeField] GameObject productGameObject;
    GameObject[] productsAroundPP = new GameObject[8];

    private void Update()
    {
        actualTime += Time.deltaTime;
        if(actualTime > productTimer)
        {
            if (productCount < maximumGoods)
            {
                SpawnProduct();
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

    public void SpawnProduct()
    {
        for(int i = 0; i < productsAroundPP.Length; i++)
        {
            if (productsAroundPP[i] == null)
            {
                GameObject newProduct = Instantiate(productGameObject, transform);
                newProduct.transform.position = productSpawnPoints[i].position;
                productsAroundPP[i] = newProduct;
                break;
            }
        }
    }

    public bool AskProducts(DistributionPoint distributionPoint)
    {
        if(productCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void SendProducts()
    {

    }

}
