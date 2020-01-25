using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Monument : Upgrade
{
    public bool purchased;
    public int owner;
    public ResourceName mainType;
    private static string[] names = { "Straw Man", "Rock Henge", "Sun Dial", "Steel Throne", "Midas", "Vanity Sculpture" };
    private UI_Control uiControl;

    public Monument()
    {
        purchased = false;
        owner = -1;
        uiControl = GameObject.FindObjectOfType<UI_Control>();
    }

    public string getName()
    {
        return names[(int)mainType - 1];
    }

    public Monument(ResourceName mainType)
    {
        this.mainType = mainType;
        purchased = false;
        owner = -1;

        type = UpgradeType.Monument;
        tier = ((int)mainType / 2) + ((int)mainType % 2);

        Resource small = new Resource(
            (ResourceName)((int)mainType+ ((int)mainType%2)*2 - 1), // Lookup function to convert large to small - Ask Michael Beshear if confused
            (int)UpgradeCost.small //* 10
        );

        Resource large = new Resource(
            mainType,
            (int)UpgradeCost.large //* 10
        );
        cost.Add(small);
        cost.Add(large);
    }

    public void updateStatus(int team)
    {
        purchased = true;
        owner = team;
        uiControl.updateMonument((int)mainType, owner);
    }
}

public class SyncListMonument : SyncList<Monument> { }
