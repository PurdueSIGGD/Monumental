using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public readonly int WOOD = 1;
    public readonly int STONE = 2;
    public readonly int COPPER = 3;
    public readonly int IRON = 4;
    public readonly int GOLD = 5;
    public readonly int DIAMOND = 6;

    protected int type;
    protected int amount;

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

    public int getType()
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
    }
}
