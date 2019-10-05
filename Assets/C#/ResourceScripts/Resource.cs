using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RESOURCE
{
    WOOD,
    STONE,
    COPPER,
    IRON,
    GOLD,
    DIAMOND,
    MAX_RESOURCES
}

public class Resource
{

    private RESOURCE type = RESOURCE.WOOD;
    private uint amount = 0;

    public Resource(RESOURCE t, uint a)
    {
        setType(t);
        setAmount(a);
    }

    public RESOURCE getType()
    {
        return type;
    }

    public void setType(RESOURCE t)
    {
        type = t;
    }

    public uint getAmount()
    {
        return amount;
    }

    public void setAmount(uint a)
    {
        amount = a;
    }

    public void addAmount(int a)
    {
        setAmount((uint)Mathf.Round(Mathf.Max(0, amount + a)));
    }

}
