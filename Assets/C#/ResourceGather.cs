using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResourceGather : NetworkBehaviour
{
    private Collider2D myCol;
    // Start is called before the first frame update
    void Start()
    {
        myCol = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay(Collider col)
    {
        Debug.Log("Stay");
        GameObject playerObj = col.gameObject;
        Player p = playerObj.GetComponent<Player>();
        p.IncrementResource(1);
    }
}
