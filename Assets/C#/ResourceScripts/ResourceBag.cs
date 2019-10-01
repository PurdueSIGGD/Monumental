using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBag : MonoBehaviour
{
    //List of all resources being held
    private List<Resource> bag;

    //Adds amount of resource of type
    public void addResource(int type, int amount)
    {
        foreach(Resource res in bag)
        {
            if(res.type == type)
            {
                res.amount += amount;
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
                res.amount += r.amount;
                return;
            }
        }
        bag.Add(r);
    }

    //Adds a bag of resources
    public void addBag(List<Resource> b)
    {
        foreach(Resource r in b)
        {
            foreach (Resource res in bag)
            {
                if (res.type == r.type)
                {
                    res.amount += r.amount;
                    break;
                }
            }
            bag.Add(r);
        }
    }

    //Removes amount of resources from bag of type
    public Resource removeAmount(int type, int amount)
    {
        foreach(Resource res in bag)
        {
            if(res.type == type)
            {
                res.amount -= amount;
                res.amount = Mathf.Max(res.amount, 0);
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
                res.amount -= r.amount;
                res.amount = Mathf.Max(res.amount, 0);
                return new Resource(r.type, Mathf.Abs(r.amount - res.amount));
            }
        }
        return new Resource(r.type, 0);
    }

    //Remove all of resource type
    public Resource removeResource(int type)
    {
        foreach (Resource res in bag)
        {
            if (res.type == type)
            {
                Resource ret = res;
                res.amount = 0;
                return ret;
            }
        }
        return new Resource(type, 0);
    }

    //Removes all resources from bag.
    public List<Resource> dumpResources()
    {
        List<Resource> b = bag;
        bag.Clear();
        return b;
    }
}
