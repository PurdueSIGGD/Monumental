using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(ResourceBag))]
public class Player : NetworkBehaviour
{
    private Rigidbody2D body;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;
    [SyncVar]
    public int teamIndex = -1;
	public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        body.velocity = new Vector2(dx, dy) * stats.movementSpeed;
    }

    public void SetTeam(int team)
    {
        teamIndex = team;
    }

	//decreases health and destroys gameobject if health reaches 0
	public void takeDamage(int damage)
	{
		stats.health -= damage;
		if (stats.health <= 0)
		{
			Destroy(gameObject);
		}
	}
}
