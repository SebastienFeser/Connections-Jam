using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DistributionPointUI : MonoBehaviour
{
    public Button buyInfluence;
    public Button hideDistribPointUI;
    public TMP_InputField influenceMoneyInput;
    public TextMeshProUGUI buyInfluenceOutput;
    public TextMeshProUGUI influenceSummary;
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
        bool valid_gang = distributionPoint.influence.ContainsKey(Level.playerGang);
        if (valid_gang && valid_float)
        {
            valid_transaction = Level.playerGang.Pay(-result);
        }

        if (valid_gang && valid_float && valid_transaction)
        {
            buyInfluenceOutput.color = UnityEngine.Color.green;
            buyInfluenceOutput.text = "Your transaction has been accepted.";

            distributionPoint.IncrementInfluence(Level.playerGang, result);
            updateInfluence();
            
        }
        else
        {
            buyInfluenceOutput.color = UnityEngine.Color.red;
            if (!valid_gang)
            {
                buyInfluenceOutput.text = "You don't have any connexions.";
            }
            else if (!valid_float)
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

    private void updateInfluence()
    {
        influenceSummary.text = "";
        if(distributionPoint.influence.Count != 0)
        {
            float total_influence = 0f;
            foreach (KeyValuePair<Gang, float> gang_influence in distributionPoint.influence)
            {
                total_influence += gang_influence.Value;
            }

            foreach (KeyValuePair<Gang, float> gang_influence in distributionPoint.influence)
            {
                string gang_color = gang_influence.Key.color.ToHexString();
                influenceSummary.text += "<color=#" + gang_color + ">" + gang_influence.Key.name + "</color >" + ": " + gang_influence.Value.ToString() + " (" + ((gang_influence.Value / total_influence) * 100).ToString() + "%) <br>";
            }
        }
    }

    private void hideUI()
    {
        influenceMoneyInput.text = "";
        buyInfluenceOutput.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void DisplayUI(DistributionPoint newDistributionPoint)
    {
        distributionPoint = newDistributionPoint;

        updateInfluence();
        //Modify title


        this.gameObject.SetActive(true);
    }

    public void Start()
    {
        influenceOutputTimer = 0f;
        influenceOutputAppearanceTime = 1.5f;
        influenceOutputFadeoutTime = 0.5f;
        influenceOutputFadeoutTimer = 0f;
        buyInfluence.onClick.AddListener(BuyInfluence);
        hideDistribPointUI.onClick.AddListener(hideUI);
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
