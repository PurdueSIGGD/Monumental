using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Monument : Upgrade
{
    public bool purchased;
    public string name;
    public int owner;

    public Monument()
    {
        purchased = false;
        owner = -1;
    }

    public Monument(ResourceName mainType)
    {
        purchased = false;
        owner = -1;

        type = UpgradeType.Monument;
        tier = ((int)mainType / 2) + ((int)mainType % 2);

        Resource small = new Resource(
            ((int)mainType+ ((int)mainType%2)*2 - 1), // Lookup function to covert large to small - Ask Michael Beshear if confused
            (int)UpgradeCost.small //* 10
        );

        Resource large = new Resource(
            (int)mainType,
            (int)UpgradeCost.large //* 10
        );
        cost[small.getType()-1] = small.getAmount();
        cost[large.getType()-1] = large.getAmount();
    }

    public void updateStatus(int team)
    {
        purchased = true;
        owner = team;
    }
}
