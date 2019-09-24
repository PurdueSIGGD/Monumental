using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceGather : NetworkBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        p.IncrementResource(1);
    }
}
