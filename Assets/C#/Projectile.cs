﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
	public GameObject parentGameobject;
	public int damage;
	public int teamIndex;
    public int sourcePlayer;
	public float timer; //this object will be destroyed after this amount of time

	private void Start()
	{
		StartCoroutine(TimerDestroy());
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.isTrigger) { return; }
		if (collision.transform.root.gameObject != parentGameobject) //don't destroy on the player that shot it
		{ 
			if (collision.gameObject.GetComponent<Player>() != null &&
				collision.gameObject.GetComponent<Player>().teamIndex != teamIndex)   //if the collision is with someone from a different team deal damage
			{
				collision.gameObject.GetComponent<Player>().takeDamage(damage, sourcePlayer);
			}
            DestroyThis();
		}
	}

    [Client]
    void DestroyThis()
    {
        Destroy(this.gameObject);
    }

	IEnumerator TimerDestroy()
	{
		yield return new WaitForSeconds(timer);
        DestroyThis();
	}
}
