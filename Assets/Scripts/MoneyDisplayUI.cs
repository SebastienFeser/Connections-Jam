using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplayUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private bool displayingCost;
    private float currentCost;

    public void DisplayCost(float cost)
    {
        displayingCost = true;
        currentCost = cost;
    }

    public void StopDisplayCost()
    {
        displayingCost = false;
    }

    void Update()
    {
        bool overbudget = displayingCost && Level.playerGang.money - currentCost < 0;


        text.text = (overbudget ? "<color=#FF0000>" : "") + Level.playerGang.money.ToString("F2") + (displayingCost ? " -" + currentCost.ToString("F2") : "") + " CHF";
    }
}
