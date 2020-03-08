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
    public int level;
    public int[] cost = new int[6];
    private GameSettings gameSettings;

    public Upgrade()
    {
        type = UpgradeType.Health;
        tier = 0;
        level = 1;
        gameSettings = GameObject.FindObjectOfType<GameSettings>();
    }

    public Upgrade(UpgradeType t, int r)
    {
        type = t;
        tier = r;
        level = 1;

        /*Resource small = new Resource(
            (ResourceName)(2*r - (int)type), //Utilizes lookup function - Ask Michael Beshear if confused
            (int)UpgradeCost.small
        );

        cost.Add(small);

        Resource large = new Resource(
            (ResourceName)(2*r + (int)type - 1),
            (int)UpgradeCost.large
        );

        cost.Add(large);*/
    }

    //Function that base calls to purchase an upgrade
    public void UpdateStats(PlayerStats ps)
    {
       level++;
        Debug.Log("Movement: " + gameSettings.Movement);
        if (type == UpgradeType.Health) { ps.baseHealth += gameSettings.Health; }
        if (type == UpgradeType.Movement) { ps.baseMovementSpeed += gameSettings.Movement; }
        if (type == UpgradeType.Interaction) { ps.baseInteractionSpeed += gameSettings.Interaction; }
        if (type == UpgradeType.Gather) { ps.baseGatherAmount += gameSettings.Gather; }
        if (type == UpgradeType.Melee) { ps.baseMeleeDamage += gameSettings.Melee; }
        if (type == UpgradeType.Range) { ps.baseRangedDamage += gameSettings.Ranged; }
    }
}

public class SyncListUpgrade : SyncList<Upgrade> { }
