using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TallGrass : MonoBehaviour
{
    public GameObject[] grass;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player p;
        if((p = collision.GetComponent<Player>()) != null)
        {
            if(p.isLocalPlayer == true)
            {
                foreach (GameObject g in grass)
                {
                    g.SetActive(false);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Player p;
        if ((p = collision.GetComponent<Player>()) != null)
        {
            if (p.isLocalPlayer == true)
            {
                foreach (GameObject g in grass)
                {
                    g.SetActive(true);
                }
            }
        }
    }
}
