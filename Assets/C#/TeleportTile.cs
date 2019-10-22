using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeleportTile : NetworkBehaviour
{
    public TeleportTile destination;
    public float cooldown;
    private float lastUse = -1;
    public int teamIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (teamIndex < 0 || p.teamIndex == teamIndex)
            {
                if (Time.time - lastUse > cooldown)
                {
                    destination.lastUse = Time.time;
                    Rigidbody2D pRigid = other.gameObject.GetComponent<Rigidbody2D>();
                    Collider2D dCol = destination.GetComponent<Collider2D>();
                    Vector2 pDir = ((Vector2)transform.position - pRigid.position).normalized;
                    Vector2 pOffset = pDir * Mathf.Abs((other.bounds.center - other.bounds.ClosestPoint(other.bounds.center + (Vector3)pDir)).magnitude);
                    Vector2 dOffset = pDir * Mathf.Abs((dCol.bounds.center - dCol.bounds.ClosestPoint(dCol.bounds.center + (Vector3)pDir)).magnitude);
                    //pRigid.position = (Vector2)destination.transform.position + pRigid.velocity.normalized * (other.bounds.extents + destination.GetComponent<Collider2D>().bounds.extents).magnitude;
                    pRigid.position = (Vector2)destination.transform.position + 1.1f * (pOffset + dOffset);
                    Debug.Log("go on");
                }
            }
            else
            {

                Debug.Log("no passage");
            }
        }
    }

}
