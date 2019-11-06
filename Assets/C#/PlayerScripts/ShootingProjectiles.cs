﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootingProjectiles : NetworkBehaviour
{
	private bool canShoot = true; //is true if player's hitbox isn't hitting a resource or enemy player
	private Rigidbody2D projectilePrefab;
	private PlayerStats stats;
	public float yValue; // Used to make it look like it's shot from the gun itself (offset)
	public float xValue; // Same as above
	public bool clicked;

	public void cannotShoot()
	{
		canShoot = false;
	}

	public void canShootNow()
	{
		canShoot = true;
	}

	void Start()
	{
		projectilePrefab = gameObject.GetComponent<Player>().projectile.GetComponent<Rigidbody2D>();
		stats = gameObject.GetComponent<PlayerStats>();
	}

	void Update()
    {
		if (!isLocalPlayer) return;
		if (Input.GetMouseButtonDown(0) && canShoot)
		{
			Transform hitbox = transform.Find("Hitbox Pivot Point");
			Rigidbody2D newProjectile = Instantiate(projectilePrefab, new Vector3(transform.position.x + xValue, transform.position.y + yValue, transform.position.z), Quaternion.identity) as Rigidbody2D;
			Projectile newProjectileProperties = newProjectile.gameObject.GetComponent<Projectile>();
			newProjectile.AddForce(hitbox.right * stats.projectileSpeed);
			newProjectileProperties.parentGameobject = gameObject;
			newProjectileProperties.damage = stats.rangedDamage;
			newProjectileProperties.teamIndex = gameObject.GetComponent<Player>().teamIndex;
			//coolDown = Time.time + attackSpeed;
		}
    }
}
