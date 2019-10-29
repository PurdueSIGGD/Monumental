using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum ResourceName
{
    WOOD = 1,
    STONE = 2,
    COPPER = 3,
    IRON = 4,
    GOLD = 5,
    DIAMOND = 6
}

public struct Resource
{
    public ResourceName type;
    public int amount;

    public static Sprite getSprite(ResourceName r)
    {
        string path = "Sprites/Resources/" + getName(r);
        return Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    public static string getName(ResourceName r)
    {
        switch (r)
        {
            case ResourceName.WOOD:
                return "Wood";
            case ResourceName.STONE:
                return "Stone";
            case ResourceName.COPPER:
                return "Copper";
            case ResourceName.IRON:
                return "Iron";
            case ResourceName.GOLD:
                return "Gold";
            case ResourceName.DIAMOND:
                return "Diamond";
        }

        return "";

    }

    public Resource(ResourceName t, int a)
    {
        type = t;
        amount = a;
    }

    public ResourceName getType()
    {
        return type;
    }

    public int getAmount()
    {
        return amount;
    }

    public void setAmount(int a)
    {
        amount = a;
        Mathf.Max(amount, 0);   //Resource amount should never be negative
    }

    public void addAmount(int a)
    {
        amount += a;
        Mathf.Max(amount, 0);   //Resource amount should never be negative
    }

    public int removeAmount(int a)
    {
        int start = amount;
        amount -= a;
        Mathf.Max(amount, 0);   //Resource amount should never be negative
        return Mathf.Abs(amount - start);
    }
}

public class SyncListResource : SyncList<Resource> { }
