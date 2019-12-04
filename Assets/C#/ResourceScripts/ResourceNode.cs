using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceNode : MonoBehaviour
{
    public ResourceName type = ResourceName.WOOD;
    public float size;
    private const float onethird = 1f / 3f;
    private const float oneninth = 1f / 9f;
    float[] scaling = new float[] { 0f, 1f, 1f, onethird, onethird, oneninth, oneninth };

    //Returns the amount of resources gathered based on type scaling and node size
    public Resource gather(float gatherMultiplier)
    {
        //We have amount be a float so size can be a non-whole number
        float amount = size * scaling[(int)type];

        //Add a small randomness element to every time they gather
        float rand = (Random.value * 0.2f) + 1; //Random number between 1 and 1.2
        amount = amount * rand * gatherMultiplier;

        //Returns the amount gathered in an integers
        return new Resource(type, (int)Mathf.Ceil(amount));
    }

    public Resource gatherPass(float gatherMultiplier, int typePass, float sizePass)
    {
        //We have amount be a float so size can be a non-whole number
        float amount = sizePass * scaling[(int)typePass];

        //Add a small randomness element to every time they gather
        float rand = (Random.value * 0.2f) + 1; //Random number between 1 and 1.2
        amount = amount * rand * gatherMultiplier;

        //Returns the amount gathered in an integers
        return new Resource((ResourceName)typePass, (int)Mathf.Ceil(amount));
    }
}
