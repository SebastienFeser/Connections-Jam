using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst.CompilerServices;
using UnityEngine.InputSystem;

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

    public Dictionary<Gang, int> policeValue = new Dictionary<Gang, int>();
    private Dictionary<Gang, float> policeTime = new Dictionary<Gang, float>();
    private float policeTimer = 3f;

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
        policeValue[productionPoint.owner] = 0;
        policeTime[productionPoint.owner] = 0;
        influence[productionPoint.owner] = Level.connectionBaseCost;

        for (int i = 0; i < askedProducts; i++) productionPoint.AskProducts(this); // request from new source too
    }

    public void IncrementInfluence(Gang gang, float additionnalInfluence)
    {
        influence[gang] += additionnalInfluence;

        //Comportement de la police:
        if (additionnalInfluence >= 100f)
        {
            policeValue[gang] += 100;
        }
        else
        {
            float threshold = UnityEngine.Random.Range(0f, 100f);
            if(threshold < additionnalInfluence)
            {
                policeValue[gang] += 100;
            }
        }

        if(policeValue[gang] >= 300) //Le gang perd 1/2 de ce qu'il a mis et le point perd un niveau de distributionSize
        {
            policeValue[gang] = 0;
            influence[gang] /= 2;
            Downgrade();
        }

        UISystem.distributionPointUI.updateInfluence();
    }

    public float GetInfluence(Gang gang)
    {
        return influence[gang];
    }
    public void OpenDistribUI()
    {
        UISystem.distributionPointUI.DisplayUI(this);
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

        //Open Distrib UI check
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(new Vector3(mousePosition.x, mousePosition.y, -0.1f), Vector3.forward);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1f, 1 << LayerMask.NameToLayer("Default"));
        if (Input.GetMouseButtonDown(0) && !(hit.collider is null))
        {
            DistributionPoint distrib;
            bool test = hit.collider.gameObject.TryGetComponent(out distrib);
            if (test)
            {
                if (hit.collider.gameObject.GetComponent<DistributionPoint>().Equals(this))
                {
                    OpenDistribUI();
                }
            }
        }

        //Police control
        foreach(Gang g in influence.Keys)
        {
            if (policeValue[g] > 0) //Le gang est surveillé par la police
            {
                policeTime[g] += Time.deltaTime;
                if (policeTime[g] > policeTimer)
                {
                    policeValue[g] -= 20;
                    UISystem.distributionPointUI.updateInfluence();
                    policeTime[g] = 0f;
                }
            }
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

    private void Payout(Gang gang)
    {
        float sum = influence.Values.Sum();
        foreach (KeyValuePair<Gang,float> i in influence)
        {
            i.Key.Pay(localProductPrice * i.Value / sum);
        }

        IncrementInfluence(gang, localProductPrice * influence[gang] / sum); // increase influence

        if (UnityEngine.Random.Range(0f,1f) < upgradeProbability) Upgrade(); // random upgrade
        if (UnityEngine.Random.Range(0f, 1f) < spreadProbability) Level.Spread(this); // random spread
    }

    public void ReceiveProduct(Gang gang)
    {
        if (productDemand > 0)
        {
            Destroy(demandAroundDP[productDemand - 1]);
            demandAroundDP.RemoveAt(productDemand - 1);
            askedProducts--;
            Payout(gang);
        }
    }
}
