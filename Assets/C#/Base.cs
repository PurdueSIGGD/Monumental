using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private Collider2D myCol;
    public int teamIndex;
    // 0 for team 1; 1 for team 2 because indexing
    // Start is called before the first frame update
    void Start()
    {
        myCol = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Player p = col.gameObject.GetComponent<Player>();
            if(p.teamIndex == teamIndex)
            {
                // deposit all resources
            } else
            {
                // don't let the player into the area
            }
        }
    }
}
