using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceBag : NetworkBehaviour
{
    //List of all resources being held
    private SyncListResource bag = new SyncListResource();

    //Adds amount of resource of type
    public void addResource(ResourceName type, int amount)
    {
        foreach(Resource res in bag)
        {
            if(res.type == type)
            {
                res.addAmount(amount);
                return;
            }
        }
        bag.Add(new Resource(type, amount));
    }

    //Adds a resource, combining any identical resources
    public void addResource(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.type == r.type)
            {
                res.addAmount(r.amount);
                return;
            }
        }
        bag.Add(r);
    }

    //Adds a bag of resources
    public void addBag(SyncListResource b)
    {
        foreach(Resource r in b)
        {
            foreach (Resource res in bag)
            {
                if (res.type == r.type)
                {
                    res.addAmount(r.amount);
                    break;
                }
            }
            bag.Add(r);
        }
    }

    //Gets the amount of resources from bag of type
    public int getAmount(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.type == type)
            {
                return res.amount;
            }
        }
        return 0;
    }

    //Gets the amount of resource r
    public int getAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.type == r.type)
            {
                return res.amount;
            }
        }
        return 0;
    }

    //Gets all of resource type
    public int getResource(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.type == type)
            {
                return res.amount;
            }
        }
        return 0;
    }

    //Gets the total bag of resources
    public SyncListResource getBag()
    {
        return bag;
    }


    //Removes amount of resources from bag of type
    public Resource removeAmount(ResourceName type, int amount)
    {
        foreach(Resource res in bag)
        {
            if(res.type == type)
            {
                res.addAmount(- amount);
                return new Resource(type, Mathf.Abs(amount - res.amount));
            }
        }
        return new Resource(type, 0);
    }

    //Removes an amount of resource r
    public Resource removeAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.type == r.type)
            {
                return new Resource(r.type, res.removeAmount(r.amount));
            }
        }
        return new Resource(r.type, 0);
    }

    //Remove all of resource type
    public Resource removeResource(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.type == type)
            {
                Resource ret = res;
                res.setAmount(0);
                return ret;
            }
        }
        return new Resource(type, 0);
    }

    //Removes b's worth of resources from bag
    public SyncListResource removeBag(SyncListResource b)
    {
        ResourceBag ret = new ResourceBag();
        foreach (Resource res in b)
        {
            ret.addResource(removeAmount(res));
        }
        return ret.bag;
    }

    //Removes all resources from bag
    public SyncListResource dumpResources()
    {
        SyncListResource b = new SyncListResource();
        b = bag;
        bag.Clear();
        return b;
    }

    //Checks the amount of resources from bag of type
    public bool checkAmount(ResourceName type, int amount)
    {
        foreach (Resource res in bag)
        {
            if (res.type == type)
            {
                return (res.amount >= amount);
            }
        }
        return false;
    }

    //Checks for an amount of resource r
    public bool checkAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.type == r.type)
            {
                return (res.amount >= r.amount);
            }
        }
        return false;
    }

    //Checks if the bag has at least the resources of b
    public bool checkBag(SyncListResource b)
    {
        bool ret = true;
        foreach(Resource res in b)
        {
            ret = ret && checkAmount(res);
        }
        return ret;
    }
}
