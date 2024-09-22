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

    public GameObject policeSummary;
    public GameObject prefabText;
    public GameObject prefabPoliceIcon;

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
    private void hideUI()
    {
        influenceMoneyInput.text = "";
        buyInfluenceOutput.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void updateInfluence()
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

        foreach(Transform child in policeSummary.transform)
        {
            Destroy(child.gameObject);
        }

        float offest_y = 0;
        foreach (KeyValuePair<Gang, int> police_value in distributionPoint.policeValue)
        {
            var newText = Instantiate(prefabText);
            newText.transform.parent = policeSummary.transform;
            newText.transform.localPosition = new Vector3(60, 0 + offest_y);
            string gang_color = police_value.Key.color.ToHexString();
            newText.GetComponent<TextMeshProUGUI>().text = "<color=#" + gang_color + ">" + police_value.Key.name + "</color >";

            float offset_x = 0;
            int police_value_temp = police_value.Value;
            while(police_value_temp > 0)
            {
                var newIcon = Instantiate(prefabPoliceIcon);
                newIcon.transform.parent = policeSummary.transform;
                newIcon.transform.localPosition = new Vector3(130 + offset_x, 15 + offest_y);

                offset_x += 40f;
                police_value_temp -= 100;
            }

            offest_y += -35f;
        }
    }

    public void DisplayUI(DistributionPoint newDistributionPoint)
    {
        distributionPoint = newDistributionPoint;

        updateInfluence();

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
