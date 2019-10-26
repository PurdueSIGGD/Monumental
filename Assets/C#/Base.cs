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
        TeleportTile[] tels = GetComponentsInChildren<TeleportTile>();
        for (int i = 0; i < tels.Length; i++)
        {
            tels[i].teamIndex = teamIndex;
        }
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
                //Heal player to full (consider adding a max health field so that the heal amount isn't hard coded)
                PlayerStats pStat = col.gameObject.GetComponent<PlayerStats>();
                pStat.health = 100;
                
            } else
            {/*
                Rigidbody2D pRigid = p.GetComponentInParent<Rigidbody2D>();
                //pRigid.position += (myCol.GetComponentInParent<Rigidbody2D>().position - pRigid.position);
                pRigid.position = pRigid.position - 2f * Time.deltaTime * pRigid.velocity;
                pRigid.velocity = Vector2.zero;
                Debug.Log("not welcome");
                */
            }
        }
    }
}
