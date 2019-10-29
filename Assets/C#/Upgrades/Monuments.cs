using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Monument : Upgrade
{
    public bool purchased = false;
    public int owner = -1;

    public Monument(ResourceName mainType)
    {
        type = UpgradeType.Monument;
        tier = ((int)mainType / 2) + ((int)mainType % 2);

        Resource small = new Resource();
        small.type = (ResourceName)((int)mainType+ ((int)mainType%2)*2 - 1); // Lookup function to covert large to small - Ask Michael Beshear if confused
        small.amount = (int)UpgradeCost.small * 10;

        Resource large = new Resource();
        large.type = mainType;
        large.amount = (int)UpgradeCost.large * 10;
    }

    public void updateStatus(int team)
    {
        purchased = true;
        owner = team;
    }
}

public class SyncListMonument : SyncList<Monument> { }
