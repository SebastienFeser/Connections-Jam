using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public static MoneyDisplayUI moneyDisplayUI;
    public static DistributionPointUI distributionPointUI;


    [SerializeField] private MoneyDisplayUI _moneyDisplayUI;
    [SerializeField] private DistributionPointUI _distributionPointUI;

    void Start()
    {
        moneyDisplayUI = _moneyDisplayUI;
        distributionPointUI = _distributionPointUI;
    }
}
