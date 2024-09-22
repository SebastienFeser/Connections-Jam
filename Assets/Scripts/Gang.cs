using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Owner
{
    player,
    AI
}

public enum ColorTag { green, yellow, red, blue }

public class Gang
{
    private Owner owner;
    public string name;
    public float money;

    public HashSet<ProductionPoint> productionPoints;
    //public HashSet<DistributionPoint> distributionPoints;

    public ColorTag color;

    public static Color GetColor(ColorTag color)
    {
        switch (color)
        {
            default:
            case ColorTag.green:
                return Color.green;
            case ColorTag.red:
                return Color.red;
            case ColorTag.yellow:
                return Color.yellow;
            case ColorTag.blue:
                return Color.blue;
        }
    }

    public Gang(string name, float initialCapital, ColorTag color, bool player = false)
    {
        this.name = name;
        money = initialCapital;
        this.color = color;
        owner = player ? Owner.player : Owner.AI;

        productionPoints = new HashSet<ProductionPoint>();
        //distributionPoints = new HashSet<DistributionPoint>();
    }

    public bool IsPlayer() { return owner == Owner.player; }
    public void AddProductionPoint(ProductionPoint point) { productionPoints.Add(point); }
    public void RemoveProductionPoint(ProductionPoint point) { productionPoints.Remove(point); }
    //public void AddDistributionPoint(DistributionPoint point) { distributionPoints.Add(point); }
    //public void RemoveDistributionPoint(DistributionPoint point) { distributionPoints.Remove(point); }

    public bool Pay(float amount)
    {
        if (money + amount >= 0)
        {
            money += amount;
            return true;
        }
        else return false;
    }
}
