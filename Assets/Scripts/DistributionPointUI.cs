using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionPointUI : MonoBehaviour
{
    public Button buyInfluence;
    public Text influenceMoneyInput;
    public GameObject viewportContent;

    private void BuyInfluence()
    {
        // 1) Take the amount written in influenceMoneyInput
        // 2) Send it to DistributionPoint
        // 3) Add a new transaction to the list (need to define what is a transaction)
        // 4) Update influence summary
        // 5) Add an ephemary text to indicate that X amount has been spend OR that the amount was wrong or too high (+ add a timer to update)
    }

    public void Start()
    {
        buyInfluence.onClick.AddListener(BuyInfluence);
    }

    public void Update()
    {

    }
}
