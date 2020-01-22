using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentHolder : NetworkBehaviour
{
    string[] names = { "Straw Man", "Rock Henge", "Sun Dial", "Steel Throne", "Midas", "Vanity Sculpture" };
    List<Monument> monuments;

    // Start is called before the first frame update
    void Start()
    {
        monuments = new List<Monument>();
        for(int i = (int)ResourceName.WOOD; i <= (int)ResourceName.DIAMOND; i++)
        {
            monuments.Add(new Monument((ResourceName)i));
            monuments[i-1].name = names[i-1];
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
