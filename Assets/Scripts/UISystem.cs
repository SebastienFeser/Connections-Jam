using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public static MoneyDisplayUI moneyDisplayUI;

    [SerializeField]
    private MoneyDisplayUI _moneyDisplayUI;

    void Start()
    {
        moneyDisplayUI = _moneyDisplayUI;
    }
}
