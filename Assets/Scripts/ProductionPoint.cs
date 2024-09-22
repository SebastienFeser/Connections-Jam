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
    AudioSource spawnAudioSource;

    public Gang owner;
    private bool produce = false;

    public float productFrequency { get { return 1 / (productTimer + Mathf.Epsilon); } }

    public void SetOwner(Gang gang)
    {
        owner = gang;
        owner.AddProductionPoint(this);
        GetComponent<SpriteRenderer>().color = owner.color;
        produce = true;
    }

    private void Start()
    {
        spawnAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (produce)
        {
            actualTime += Time.deltaTime;
            if (actualTime > productTimer)
            {
                if (productCount < maximumGoods) SpawnProduct();
                actualTime -= productTimer;
            }

            while (waitingDemand.Count > 0 && productCount > 0) // change for a if + timer if we want delay between sending operation
            {
                DistributionPoint dp = waitingDemand[0];
                if (dp.productDemand > 0) SendProduct(dp); // only send if the node is still asking
                waitingDemand.RemoveAt(0);
            }
        }
    }

    public bool IsConnectedTo(DistributionPoint distributionPoint) { return connections.Contains(distributionPoint); }
    public void AddConnection(DistributionPoint distributionPoint) { connections.Add(distributionPoint); }

    public void SpawnProduct()
    {
        for(int i = 0; i < productsAroundPP.Length; i++)
        {
            if (productsAroundPP[i] == null)
            {
                GameObject newProduct = Instantiate(productGameObject, transform);
                newProduct.transform.position = productSpawnPoints[i].position;
                if (owner.IsPlayer())
                {
                    spawnAudioSource.Play();
                }
                productsAroundPP[i] = newProduct;
                productCount++;
                break;
            }
        }
    }

    public bool AskProducts(DistributionPoint distributionPoint)
    {
        waitingDemand.Add(distributionPoint);
        return true;

        /*if(productCount > 0)
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
        }*/
    }

    void SendProduct(DistributionPoint distributionPoint)
    {
        for (int i = productsAroundPP.Length - 1; i >= 0; i--)
        {
            if(productsAroundPP[i] != null)
            {
                productsAroundPP[i].GetComponent<Product>().StartMovement(this, distributionPoint, 5f);
                productsAroundPP[i] = null;
                productCount--;
                break;
            }
        }
    }

}
