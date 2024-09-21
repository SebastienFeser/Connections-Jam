using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionPoint : MonoBehaviour
{
    int productCount;
    public HashSet<DistributionPoint> connections = new HashSet<DistributionPoint>();
    [SerializeField] float productTimer;
    float actualTime = 0;
    [SerializeField] int maximumGoods;
    [SerializeField] Transform[] productSpawnPoints;

    [SerializeField] GameObject productGameObject;
    GameObject[] productsAroundPP = new GameObject[8];
    List<DistributionPoint> waitingDemand = new List<DistributionPoint>();
    Gang gang;

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

    public bool IsConnectedTo(DistributionPoint distributionPoint) { return connections.Contains(distributionPoint); }
    public void AddConnection(DistributionPoint distributionPoint) { connections.Add(distributionPoint); }

    public void SetGang(Gang newGang)
    {
        gang = newGang;
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
            if (waitingDemand.Count == 0)
            {
                SendProduct(distributionPoint);
                return true;
            }
            else
            {
                if(waitingDemand[0] == distributionPoint)
                {
                    SendProduct(distributionPoint);
                    waitingDemand.RemoveAt(0);
                    return true;
                }
                else
                {
                    if (!waitingDemand.Contains(distributionPoint))
                    {
                        waitingDemand.Add(distributionPoint);
                    }
                    return false;
                }
            }
        }
        else
        {
            if (!waitingDemand.Contains(distributionPoint))
            {
                waitingDemand.Add(distributionPoint);
            }
            return false;
        }
    }
    void SendProduct(DistributionPoint distributionPoint)
    {
        for (int i = productsAroundPP.Length - 1; i >= 0; i--)
        {
            if(productsAroundPP[i] != null)
            {
                productsAroundPP[i].GetComponent<Product>().StartMovement(transform, distributionPoint.transform, 1, distributionPoint);
                productsAroundPP[i] = null;
                productCount--;
                break;
            }
        }
    }

}
