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

public class Resource
{
    private int type;
    [SyncVar]
    private int amount;

    public static Sprite getSprite(int r)
    {
        string path = "Sprites/Resources/" + getName(r);
        return Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    public static string getName(int r)
    {
        switch (r)
        {
            case 1:
                return "Wood";
            case 2:
                return "Stone";
            case 3:
                return "Copper";
            case 4:
                return "Iron";
            case 5:
                return "Gold";
            case 6:
                return "Diamond";
        }

        return "";

    }

    public static Sprite getSpriteRefined(int r)
    {
        string path = "Sprites/Resources/" + getNameRefined(r);
        //Debug.Log(path);
        return Resources.Load(path, typeof(Sprite)) as Sprite;
    }

    public static string getNameRefined(int r)
    {
        switch (r)
        {
            case 1:
                return "Wood";
            case 2:
                return "Stone";
            case 3:
                return "CopperBar";
            case 4:
                return "IronBar";
            case 5:
                return "Gold";
            case 6:
                return "Diamond";
        }

        return "";

    }

    public Resource()
  	{
  		type = 0;
  		amount = 0;
  	}

    public Resource(int t, int a)
    {
        type = t;
        amount = a;
    }

    public Resource(Resource r)
    {
        type = r.getType();
        amount = r.getAmount();
    }

    public int getType()
    {
        return type;
    }

    public void setType(int t)
    {
        type = t;
    }

    public int getAmount()
    {
        return amount;
    }

    public void setAmount(int a)
    {
        amount = a;
		amount = Mathf.Max(amount, 0);   //Resource amount should never be negative
    }

    public void addAmount(int a)
    {
        amount += a;
        amount = Mathf.Max(amount, 0);   //Resource amount should never be negative
    }

    public int removeAmount(int a)
    {
        int start = amount;
        amount -= a;
		amount = Mathf.Max(amount, 0);   //Resource amount should never be negative
        return Mathf.Abs(amount - start);
    }
}
