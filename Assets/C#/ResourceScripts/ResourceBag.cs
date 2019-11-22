using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceBag : NetworkBehaviour
{
    //List of all resources being held
    public SyncListResource bag = new SyncListResource();

	//Prints out all resources and their amounts in the bag, used for debugging
	public void testBag()
	{
		foreach (Resource res in bag)
		{
			Debug.Log(res.getType() + ": " + res.getAmount());
		}
	}

	//Adds amount of resource of type
	public void addResource(ResourceName type, int amount)
    {
		foreach (Resource res in bag)
        {
			if (res.getType() == type)
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
            if (res.getType() == r.getType())
            {
                res.addAmount(r.getAmount());
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
            addResource(r);
        }
    }

    public void addBagAsInt(int[] res)
    {
        for(int i = (int)(ResourceName.WOOD); i <= (int)ResourceName.DIAMOND; i++)
        {
            addResource((ResourceName)i, res[i - 1]);
        }
    }

    //Gets the amount of resources from bag of type
    public int getAmount(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == type)
            {
                return res.getAmount();
            }
        }
        return 0;
    }

    //Gets the amount of resource r
    public int getAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == r.getType())
            {
                return res.getAmount();
            }
        }
        return 0;
    }

    //Gets all of resource type
    public int getResource(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == type)
            {
                return res.getAmount();
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
            if(res.getType() == type)
            {
                res.addAmount(- amount);
                return new Resource(type, Mathf.Abs(amount - res.getAmount()));
            }
        }
        return new Resource(type, 0);
    }

    //Removes an amount of resource r
    public Resource removeAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == r.getType())
            {
                return new Resource(r.getType(), res.removeAmount(r.getAmount()));
            }
        }
        return new Resource(r.getType(), 0);
    }

    //Remove all of resource type
    public Resource removeResource(ResourceName type)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == type)
            {
                Resource ret = new Resource(res);
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
        foreach (Resource res in bag)
        {
            b.Add(removeResource(res.getType()));
        }
        return b;
    }

    public int[] dumpResourcesAsInt()
    {
        int[] ret = new int[6];
        for(int i = (int)ResourceName.WOOD; i <= (int)ResourceName.DIAMOND; i++)
        {
            ret[i-1] = removeResource((ResourceName)i).getAmount();
        }
        return ret;
    }

    //Checks the amount of resources from bag of type
    public bool checkAmount(ResourceName type, int amount)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == type)
            {
                return (res.getAmount() >= amount);
            }
        }
        return false;
    }

    //Checks for an amount of resource r
    public bool checkAmount(Resource r)
    {
        foreach (Resource res in bag)
        {
            if (res.getType() == r.getType())
            {
                return (res.getAmount() >= r.getAmount());
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

    public bool isEmpty()
    {
        foreach (Resource res in bag)
        {
            if (res.getAmount() > 0)
            {
                return false;
            }
        }
        return true;
    }

}
