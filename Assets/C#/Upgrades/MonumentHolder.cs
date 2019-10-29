using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentHolder : MonoBehaviour
{
    public SyncListMonument monuments = new SyncListMonument();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = (int)ResourceName.WOOD; i < (int)ResourceName.DIAMOND; i++)
        {
            monuments.Add(new Monument((ResourceName)i));
        }
    }
}
