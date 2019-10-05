using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceBag
{
    //List of all resources being held
    [SyncVar]
    private Resource[] resources = new Resource[(int)RESOURCE.MAX_RESOURCES];

    //Adds amount of resource of type
    public void addResource(RESOURCE type, int amount)
    {
        resources[(uint)type].addAmount(amount);
    }

    public void addResource(Resource r)
    {
        addResource(r.getType(), (int)r.getAmount());
    }

    public bool removeResource(Resource r)
    {
        uint index = (uint)r.getType();
        if (resources[index].getAmount() < r.getAmount())
        {
            return false;
        }
        resources[index].addAmount(-(int)r.getAmount());
        return true;
    }

    //Adds a bag of resources
    public void addBag(ResourceBag bag)
    {
        for (uint i = 0; i < resources.Length; i++)
        {
            resources[i].addAmount(bag.getAmount((RESOURCE)i));
        }
    }

    public bool removeBag(ResourceBag bag)
    {
        /* Make sure we have enough to subtract first */
        if (!contains(bag))
        {
            return false;
        }
        for (uint i = 0; i < resources.Length; i++)
        {
            resources[i].addAmount(-bag.getAmount((RESOURCE)i));
        }
        return true;
    }

    /* If this bag contains resources present in another */
    public bool contains(ResourceBag bag)
    {
        for (uint i = 0; i < resources.Length; i++)
        {
            if (resources[i].getAmount() < bag.getAmount((RESOURCE)i))
            {
                return false;
            }
        }
        return true;
    }

    /* If this bag contains amount of a certain resource */
    public bool contains(Resource r)
    {
        return resources[(uint)r.getType()].getAmount() >= r.getAmount();
    }

    //Gets the amount of resources from bag of type
    public int getAmount(RESOURCE type)
    {
        return (int)resources[(uint)type].getAmount();
    }

    //Removes all resources from bag.
    public void clear()
    {
        for (uint i = 0; i < resources.Length; i++)
        {
            resources[i].setAmount(0);
        }
    }
}
