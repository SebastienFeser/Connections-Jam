using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionPoint : MonoBehaviour
{
    int productCount;
    List<DistributionPoint> connections;
    [SerializeField] float productTimer;
    float actualTime = 0;
    [SerializeField] int maximumGoods;

    private void Update()
    {
        actualTime += Time.deltaTime;
        if(actualTime > productTimer)
        {
            if (productCount < maximumGoods)
            {
                productCount++;
            }
            actualTime -= productTimer;
        }
    }

}
