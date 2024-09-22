using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistributionPointUI : MonoBehaviour
{
    public Button buyInfluence;
    public TextMeshPro influenceMoneyInput;
    public TextMeshPro buyInfluenceOutput;
    public GameObject viewportContent;
    public DistributionPoint distributionPoint;

    //influence output timing
    private float influenceOutputAppearanceTime;
    private float influenceOutputTimer;
    private float influenceOutputFadeoutTime;
    private float influenceOutputFadeoutTimer;

    private void BuyInfluence()
    {
        influenceOutputTimer = 0f;
        influenceOutputFadeoutTimer = 0f;
        buyInfluenceOutput.alpha = 1f;
        buyInfluenceOutput.gameObject.SetActive(true);

        float result = 0f;
        bool valid_transaction = false;
        bool valid_float = float.TryParse(influenceMoneyInput.text, out result);
        if (valid_float)
        {
            valid_transaction = Level.playerGang.Pay(-result);
        }

        if (valid_float && valid_transaction)
        {
            buyInfluenceOutput.color = Color.green;
            buyInfluenceOutput.text = "Your transaction has been accepted.";

            distributionPoint.IncrementInfluence(Level.playerGang, result);
            
        }
        else
        {
            buyInfluenceOutput.color = Color.red;
            if (!valid_float)
            {
                buyInfluenceOutput.text = "Your input is not a valid amount.";
            }
            else
            {
                buyInfluenceOutput.text = "You don't have enough money.";
            }
        }

        
        
        //TODO:
        // 3) Add a new transaction to the list (need to define what is a transaction) (Not necessary?)
        // 4) Update influence summary
    }

    public void DisplayUI(DistributionPoint newDistributionPoint)
    {
        distributionPoint = newDistributionPoint;
        this.gameObject.SetActive(true);
    }

    public void Start()
    {
        influenceOutputTimer = 0f;
        influenceOutputAppearanceTime = 1.5f;
        influenceOutputFadeoutTime = 0.5f;
        influenceOutputFadeoutTimer = 0f;
        buyInfluence.onClick.AddListener(BuyInfluence);
    }

    public void Update()
    {
        if (buyInfluenceOutput.IsActive())
        {
            influenceOutputTimer += Time.deltaTime;
            if(influenceOutputTimer > influenceOutputAppearanceTime)
            {
                influenceOutputFadeoutTimer += Time.deltaTime;
                if(influenceOutputFadeoutTimer > influenceOutputFadeoutTime)
                {
                    buyInfluenceOutput.gameObject.SetActive(false);
                }
                else
                {
                    buyInfluenceOutput.alpha = 1 - ((influenceOutputFadeoutTime - influenceOutputFadeoutTimer) / influenceOutputFadeoutTimer);
                }
            }
        }
    }
}
