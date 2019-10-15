using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceName type = ResourceName.WOOD;
    public float size;
    private int scaling;

    //Set the scaling varible right to determine how many resources to give gatherers
    void Start()
    {
        if (type == ResourceName.WOOD || type == ResourceName.STONE)
        {
            scaling = 9;
        }

        else if (type == ResourceName.COPPER || type == ResourceName.IRON)
        {
            scaling = 3;
        }

        else
        {
            scaling = 1;
        }
    }

    //Returns the amount of resources gathered based on type scaling and node size
    public Resource gather()
    {
        //We have amount be a float so size can be a non-whole number
        float amount = size * scaling;

        //Add a small randomness element to every time they gather
        float rand = (Random.value * 0.2f) + 1; //Random number between 1 and 1.2
        amount = amount * rand;

        //Returns the amount gathered in an integer
        return new Resource(type, (int)Mathf.Floor(amount));
    }
}
