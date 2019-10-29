using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectiles : MonoBehaviour
{
	private bool canShoot = true; //is true if player's hitbox isn't hitting a resource or enemy player
	private Rigidbody2D projectilePrefab;
	private PlayerStats stats;
	public float yValue = 1f; // Used to make it look like it's shot from the gun itself (offset)
	public float xValue = 0.2f; // Same as above

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
		if (Input.GetMouseButtonDown(0) && canShoot)
		{
			Rigidbody2D newProjectile = Instantiate(projectilePrefab, new Vector3(transform.position.x + xValue, transform.position.y + yValue, transform.position.z), Quaternion.identity) as Rigidbody2D;
			Projectile newProjectileProperties = newProjectile.gameObject.GetComponent<Projectile>();
			newProjectile.AddForce(transform.right * stats.projectileSpeed);
			newProjectileProperties.parentGameobject = gameObject;
			newProjectileProperties.damage = stats.rangedDamage;
			newProjectileProperties.teamIndex = gameObject.GetComponent<Player>().teamIndex;
			//coolDown = Time.time + attackSpeed;
		}
    }
}
