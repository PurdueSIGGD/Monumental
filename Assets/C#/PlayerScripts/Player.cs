using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(ResourceBag))]
public class Player : NetworkBehaviour
{
    public int health;
    private Rigidbody2D body;
    private UI_Control uiControl;
    private Slider healthbar;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;
    private Vector2 spawn;

    [SyncVar]
    public int teamIndex = -1;
	[SyncVar]
    public int health = 100;
    public GameObject projectile;
	private HitDetection hitDetect;
	private ShootingProjectiles shootingProjectile;

    // Start is called before the first frame update
    void Start()
    {
		hitDetect = GetComponentInChildren<HitDetection>();
		shootingProjectile = GetComponent<ShootingProjectiles>();
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        health = stats.health;
        resources = gameObject.AddComponent<ResourceBag>();
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        healthbar = GetComponentInChildren<Slider>();
        spawn = transform.position;

        if (isLocalPlayer)
        {
            UI_Control uiControl = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
            uiCamera.followTarget = this.gameObject;
            health = stats.health;
        }
    }

	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer) return;

		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
		body.velocity = new Vector2(dx, dy) * stats.movementSpeed;
		if (Input.GetMouseButtonDown(0))
		{
			hitDetect.clicked = true;
			shootingProjectile.clicked = true;
		}
	}

    //calculates the difference between the current player and the other player
    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.transform.position);
    }

    //player takes damage of amount damage from player attacker
    public void takeDamage(int damage, Player attacker)
    {
        health -= damage;
        if (health <= 0)
        {
            resourceTransfer(attacker);
            respawn();
        }
    }

    //respawns character by setting character to maxHealth, moving the character back to spawn, and giving resources to other player
    public void respawn()
    {
        health = stats.health;
        transform.position = spawn;
    }

    void LateUpdate()
    {
        healthbar.value = health / (float)stats.health;
    }

    public void setHealth(int val)
    {
        health = val;
        if (health <= 0)
        {
			Debug.Log(gameObject.name + " is dead");
			health = 100;
        }
    }

        //Sets team the team to whatever the input is
    public void SetTeam(int team)
    {
        teamIndex = team;
    }

    //transfers resources from this players bag to the other players bag
    public void resourceTransfer(Player attacker)
    {
        ResourceBag otherBag = attacker.GetComponent<ResourceBag>();
        ResourceBag myBag = this.GetComponent<ResourceBag>();
        otherBag.addBag(myBag.dumpResources());
    }
}
