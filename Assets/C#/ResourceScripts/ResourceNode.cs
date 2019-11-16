using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceName type = ResourceName.WOOD;
    public float size;
    private float scaling;

    //Set the scaling varible right to determine how many resources to give gatherers
    void Start()
    {
        if (type == ResourceName.WOOD || type == ResourceName.STONE)
        {
            scaling = 1;
        }

        else if (type == ResourceName.COPPER || type == ResourceName.IRON)
        {
            scaling = 1f/3f;
        }

        else
        {
            scaling = 1f/9f;
        }
    }

    //Returns the amount of resources gathered based on type scaling and node size
    public Resource gather(float gatherMultiplier)
    {
        //We have amount be a float so size can be a non-whole number
        float amount = size * scaling;

        //Add a small randomness element to every time they gather
        float rand = (Random.value * 0.2f) + 1; //Random number between 1 and 1.2
        amount = amount * rand * gatherMultiplier;

        //Returns the amount gathered in an integers
        return new Resource(type, (int)Mathf.Ceil(amount));
    }
}
