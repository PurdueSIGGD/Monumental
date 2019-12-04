using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootingProjectiles : NetworkBehaviour
{
	private bool canShoot = true; //is true if player's hitbox isn't hitting a resource or enemy player
	private Rigidbody2D projectilePrefab;
    private Rigidbody2D playerBody;
	private PlayerStats stats;
	public float yValue; // Used to make it look like it's shot from the gun itself (offset)
	public float xValue; // Same as above
	public bool clicked;
    public float projectileSpeed = 1000.0f;

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
        playerBody = gameObject.GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
    {
		if (!isLocalPlayer) return;
		if (clicked && canShoot)
		{
            CmdSpawnProjectile(playerBody.velocity);
            clicked = false;
		}
    }

    [Command]
    void CmdSpawnProjectile(Vector2 vel)
    {
        RpcSpawnProjectile(vel);
    }

    [ClientRpc]
    void RpcSpawnProjectile(Vector2 vel)
    {
        Rigidbody2D newProjectile = Instantiate(projectilePrefab, new Vector3(transform.position.x + xValue, transform.position.y + yValue, transform.position.z), Quaternion.identity) as Rigidbody2D;
        Projectile newProjectileProperties = newProjectile.gameObject.GetComponent<Projectile>();
        newProjectile.AddForce(vel, ForceMode2D.Impulse);
        newProjectile.AddForce(transform.up * projectileSpeed);
        newProjectileProperties.parentGameobject = gameObject;
        newProjectileProperties.damage = stats.getRangedDamage();
        newProjectileProperties.teamIndex = gameObject.GetComponent<Player>().teamIndex;
        newProjectileProperties.sourcePlayer = gameObject.GetComponent<Player>().positionInPlayerList;
        //coolDown = Time.time + attackSpeed;
    }
}
