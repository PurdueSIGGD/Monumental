using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//What each type of upgrade affects in player stats
public enum UpgradeType
{
    Health = 0,
    Movement = 0,
    Interaction = 0,
    Gather = 1,
    Melee = 1,
    Range = 1,
    Monument = 2
}

//Base cost of an upgrade of the lesser and greater resource costs
//Can be updated for balance
enum UpgradeCost
{
    small = 10,
    large = 100
}

public class Upgrade
{
    public UpgradeType type;
    public int tier;
    public SyncListResource cost = new SyncListResource();

    //Value sliders that can be chaged for balance
    static int Health = 30;
    static float Movement = 0.25f;
    static float Interaction = 0.1f;
    static float Gather = 0.1f;
    static int Melee = 20;
    static int Ranged = 10;
    static float costScale = 1.2f;

    public Upgrade()
    {
        type = UpgradeType.Health;
        tier = 0;
    }

    public Upgrade(UpgradeType t, int r)
    {
        type = t;
        tier = r;

        Resource small = new Resource(
            (ResourceName)(2*r - (int)type), //Utilizes lookup function - Ask Michael Beshear if confused
            (int)UpgradeCost.small
        );

        cost.Add(small);

        Resource large = new Resource(
            (ResourceName)(2*r + (int)type - 1),
            (int)UpgradeCost.large
        );

        cost.Add(large);
    }

    //Function that base calls to purchase an upgrade
    public void UpdateStatsAndCost(PlayerStats ps)
    {
        if(type == UpgradeType.Health)
        {
            ps.health += Health;
            ps.movementSpeed += Movement;
            ps.interactionSpeed += Interaction;
        }
        else if(type == UpgradeType.Gather)
        {
            ps.gatherAmount += Gather;
            ps.meleeDamage += Melee;
            ps.rangedDamage += Ranged;
        }

        foreach(Resource res in cost)
        {
            res.setAmount((int)(res.getAmount() * costScale));
        }
    }
}

public class SyncListUpgrade : SyncList<Upgrade> { }
