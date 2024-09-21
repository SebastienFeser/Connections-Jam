using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level level;

    public static Gang playerGang { get { return level._playerGang; } }
    public static List<Gang> adversaryGangs { get { return level._adversaryGangs; } }
    public static List<ProductionPoint> productionPoints { get { return level._productionPoints; } }
    public static List<DistributionPoint> distributionPoints { get { return level._distributionPoints; } }

    private Gang _playerGang;
    private List<Gang> _adversaryGangs;

    private List<ProductionPoint> _productionPoints;
    private List<DistributionPoint> _distributionPoints;

    public static float connectionCostPerUnit = 10f;
    public static float connectionBaseCost = 25f;


    private void InitializeGangs()
    {
        // arbitrary values for now
        _playerGang = new Gang("Milo's Club", 200, true);

        _adversaryGangs = new List<Gang>();
        _adversaryGangs.Add(new Gang("MegaCorp Inc. (TM)", 500));
        _adversaryGangs.Add(new Gang("Cosa Nostradamus", 300));
    }

    private void InitializePoints()
    {
        foreach (ProductionPoint pp in GetComponentsInChildren<ProductionPoint>())
        {
            _productionPoints.Add(pp);
        }

        foreach (DistributionPoint dp in GetComponentsInChildren<DistributionPoint>())
        {
            _distributionPoints.Add(dp);
        }
    }

    void Start()
    {
        level = this;

        InitializeGangs();
        InitializePoints();
    }

    void Update()
    {
        
    }
}
