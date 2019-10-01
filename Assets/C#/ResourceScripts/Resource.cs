using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ResourceNames
{
    WOOD = 1,
    STONE = 2,
    COPPER = 3,
    IRON = 4,
    GOLD = 5,
    DIAMOND = 6
}

public class Resource : MonoBehaviour
{
    //Defintions of resource types as readable words
    public readonly int WOOD = 1;
    public readonly int STONE = 2;
    public readonly int COPPER = 3;
    public readonly int IRON = 4;
    public readonly int GOLD = 5;
    public readonly int DIAMOND = 6;

    public int type;
    public int amount;

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
        Mathf.Max(amount, 0);   //Resource amount should never be negative
    }
}
