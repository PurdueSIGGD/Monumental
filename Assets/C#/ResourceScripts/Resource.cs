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
    public ResourceName type;
    public int amount;

	public Resource()
	{
		type = 0;
		amount = 0;
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

    public void setType(ResourceName t)
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

public class SyncListResource : SyncList<Resource> { }
