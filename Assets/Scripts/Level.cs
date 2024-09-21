using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level level;

    public Gang playerGang;
    public List<Gang> adversaryGangs;

    public List<DistributionPoint> distributionPoints;
    public List<ProductionPoint> productionPoints;


    private void InitializeGangs()
    {
        // arbitrary values for now
        playerGang = new Gang("Milo's Club", 200, true);

        adversaryGangs = new List<Gang>();
        adversaryGangs.Add(new Gang("MegaCorp Inc. (TM)", 500));
        adversaryGangs.Add(new Gang("Cosa Nostradamus", 300));
    }

    void Start()
    {
        level = this;

        InitializeGangs();
    }

    void Update()
    {
        
    }
}
