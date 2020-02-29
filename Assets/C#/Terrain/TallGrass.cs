using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TallGrass : MonoBehaviour
{

    // This should be a unique value from all other tall grasses, and also should never be 0.
    public int layer;
    private Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player p;
        if((p = collision.GetComponent<Player>()) != null)
        {
            if(p.isLocalPlayer == true)
            {
                cam.cullingMask = cam.cullingMask ^ (1 << layer);
            }
        }
        SpriteRenderer sr;
        if((sr = collision.GetComponent<SpriteRenderer>()))
        {
            sr.renderingLayerMask = (uint)layer;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Player p;
        if ((p = collision.GetComponent<Player>()) != null)
        {
            if (p.isLocalPlayer == true)
            {
                cam.cullingMask = cam.cullingMask ^ (1 << layer);
            }
        }
        SpriteRenderer sr;
        if ((sr = collision.GetComponent<SpriteRenderer>()))
        {
            sr.renderingLayerMask = 0;
        }
    }
}
