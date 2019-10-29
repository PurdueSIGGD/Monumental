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

        Resource small = new Resource(
            (ResourceName)((int)mainType+ ((int)mainType%2)*2 - 1), // Lookup function to covert large to small - Ask Michael Beshear if confused
            (int)UpgradeCost.small * 10
        );

        Resource large = new Resource(
            mainType,
            (int)UpgradeCost.large * 10
        );
    }

    public void updateStatus(int team)
    {
        purchased = true;
        owner = team;
    }
}

public class SyncListMonument : SyncList<Monument> { }
