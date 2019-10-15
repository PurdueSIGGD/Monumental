using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

enum UpgradeAmount
{
    small = 10,
    large = 100
}

public class Upgrade
{
    public UpgradeType type;
    public int tier;
    public SyncListResource cost = new SyncListResource();

    public Upgrade(UpgradeType t, int r)
    {
        type = t;
        tier = r;

        Resource small = new Resource();
        small.type = (ResourceName)(2*r - (int)type); //Utilizes lookup function - Ask Michael Beshear if confused
        small.amount = (int)UpgradeAmount.small;

        cost.Add(small);

        Resource large = new Resource();
        large.type = (ResourceName)(2*r + (int)type - 1);
        large.amount = (int)UpgradeAmount.large;

        cost.Add(large);
    }
}
