using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentHolder : NetworkBehaviour
{
    public SyncListMonument monuments = new SyncListMonument();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = (int)ResourceName.WOOD; i <= (int)ResourceName.DIAMOND; i++)
        {
            monuments.Add(new Monument((ResourceName)i));
        }
    }

    public int getScore(int team)
    {
        int count = 0;
        for (int i = 0; i < monuments.Count; i++)
        {
            if (monuments[i].owner == team)
            {
                count++;
            }
        }
        return count;
    }
}
