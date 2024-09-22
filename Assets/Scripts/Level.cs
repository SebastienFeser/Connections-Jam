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

    [SerializeField] private GameObject aiAgent;

    private List<ProductionPoint> _productionPoints;
    private List<DistributionPoint> _distributionPoints;

    public static float connectionCostPerUnit = 10f;
    public static float connectionBaseCost = 25f;


    private void InitializeGangs()
    {
        // arbitrary values for now
        _playerGang = new Gang("Milo's Club", 200, Color.green, true);

        _adversaryGangs = new List<Gang>();
        createAIGang(new Gang("MegaCorp Inc. (TM)", 500, Color.yellow));
        createAIGang(new Gang("Cosa Nostradamus", 300, Color.red));
    }

    private void createAIGang(Gang gang)
    {
        _adversaryGangs.Add(gang);
        GameObject aiGang = Instantiate(aiAgent, transform);
        aiGang.GetComponent<AI>().SetGang(gang, 8f, 0.1f, 0f);
    }

    private void InitializePoints()
    {
        _productionPoints = new List<ProductionPoint>();
        _distributionPoints = new List<DistributionPoint>();

        // setup production points
        foreach (ProductionPoint pp in GetComponentsInChildren<ProductionPoint>())
        {
            _productionPoints.Add(pp);
        }

        // sample initial production point for each gang
        List<int> indices = new List<int>();
        for (int i = 0; i < _productionPoints.Count; i++) indices.Add(i);

        int randomIndex = Random.Range(0, indices.Count - 1);
        _productionPoints[indices[randomIndex]].SetOwner(_playerGang);
        indices.Remove(indices[randomIndex]);

        foreach (Gang g in _adversaryGangs)
        {
            randomIndex = Random.Range(0, indices.Count - 1);
            _productionPoints[indices[randomIndex]].SetOwner(g);
            indices.Remove(indices[randomIndex]);
        }

        // setup distribution points
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
