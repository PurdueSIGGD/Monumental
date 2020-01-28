using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceBag : NetworkBehaviour
{
    //List of all resources being held
    [SyncVar]
    int wood;
    [SyncVar]
    int stone;
    [SyncVar]
    int copper;
    [SyncVar]
    int iron;
    [SyncVar]
    int gold;
    [SyncVar]
    int diamond;


    void start()
    {
        wood = 0;
        stone = 0;
        copper = 0;
        iron = 0;
        gold = 0;
        diamond = 0;
    }

	//Prints out all resources and their amounts in the bag, used for debugging
	/*public void testBag()
	{
		foreach (Resource res in bag)
		{
			Debug.Log(res.getType() + ": " + res.getAmount());
		}
	}*/

	//Adds amount of resource of type
	public void addAmount(int type, int amount)
    {
        switch (type)
        {
            case 1:
                wood += amount;
                return;
            case 2:
                stone += amount;
                return;
            case 3:
                copper += amount;
                return;
            case 4:
                iron += amount;
                return;
            case 5:
                gold += amount;
                return;
            case 6:
                diamond += amount;
                return;
        }
        return;
    }
    
    public void addResource(Resource r)
    {
        addAmount(r.getType(), r.getAmount());
    }

    public void addBagAsInt(int[] res)
    {
        wood += res[0];
        stone += res[1];
        copper += res[2];
        iron += res[3];
        gold += res[4];
        diamond += res[5];
    }

    //Gets the amount of resources from bag of type
    public int getAmount(int type)
    {
        switch (type)
        {
            case 1:
                return wood;
            case 2:
                return stone;
            case 3:
                return copper;
            case 4:
                return iron;
            case 5:
                return gold;
            case 6:
                return diamond;
        }
        return 0;
    }

    //Gets the total bag of resources
    public int[] getBag()
    {
        return new int[] { wood, stone, copper, iron, gold, diamond };
    }

    //Removes amount of resources from bag of type
    public void removeAmount(int type, int amount)
    {
        switch (type)
        {
            case 1:
                wood -= amount;
                if (wood < 0) wood = 0;
                return;
            case 2:
                stone -= amount;
                if (stone < 0) stone = 0;
                return;
            case 3:
                copper -= amount;
                if (copper < 0) copper = 0;
                return;
            case 4:
                iron -= amount;
                if (iron < 0) iron = 0;
                return;
            case 5:
                gold -= amount;
                if (gold < 0) gold = 0;
                return;
            case 6:
                diamond -= amount;
                if (diamond < 0) diamond = 0;
                return;
        }
        return;
    }

    public void removeBagAsInt(int[] res)
    {
        for(int i=0; i<6; i++)
        {
            removeAmount(i + 1, res[i]);
        }
    }

    //Removes all resources from bag
    public int[] dumpResources()
    {
        int[] b = getBag();
        removeBagAsInt(b);
        return b;
    }

    //Checks the amount of resources from bag of type
    public bool checkAmount(int type, int amount)
    {
        int a = getAmount(type);
        return a >= amount;
    }

    //Checks if the bag has at least the resources of b
    public bool checkBag(int[] b)
    {
        for(int i=0; i<6; i++)
        {
            if (!checkAmount(i + 1, b[i])) return false;
        }
        return true;
    }

    public bool isEmpty()
    {
        return wood == 0 && stone == 0 && copper == 0 && iron == 0 && gold == 0 && diamond == 0;
    }

}
