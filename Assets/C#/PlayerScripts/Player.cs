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
    private Rigidbody2D body;
    private UI_Control uiControl;
    private Slider healthbar;
    public MonumentalNetworkManager mnm;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;

    [SyncVar]
    public int teamIndex = -1;
    [SyncVar]
    public int positionInPlayerList = -1;

    [SyncVar]
    public int health = 100;
    public GameObject projectile;
	private HitDetection hitDetect;
	private ShootingProjectiles shootingProjectile;
    private float timeOfLastClick;

    // Start is called before the first frame update
    void Start()
    {
		hitDetect = GetComponentInChildren<HitDetection>();
		shootingProjectile = GetComponent<ShootingProjectiles>();
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        resources = gameObject.AddComponent<ResourceBag>();
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        healthbar = GetComponentInChildren<Slider>();
        timeOfLastClick = Time.time;

        if (isLocalPlayer)
        {
            hitDetect.isTheLocalPlayer = true;
            UI_Control uiControl = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
            uiCamera.followTarget = this.gameObject;
            health = stats.health;
        }

    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isLocalPlayer) return;

		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
		body.velocity = new Vector2(dx, dy) * stats.movementSpeed;
		if (Input.GetMouseButton(0) && timeOfLastClick + stats.interactionSpeed < Time.time)
		{
            timeOfLastClick = Time.time;
			hitDetect.clicked = true;
			shootingProjectile.clicked = true;
		}
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

    public void SetTeam(int team)
    {
        teamIndex = team;
    }

    public void SetNetManager(MonumentalNetworkManager m)
    {
        mnm = m;
    }

    public void SetPositionInPlayerList(int p)
    {
        positionInPlayerList = p;
    }

	//decreases health and destroys gameobject if health reaches 0
	public void takeDamage(int damage)
	{
        setHealth(health - damage);
    }

    [Command]
    public void CmdDamageThem(int target, int damage)
    {
        RpcDamageThem(target, damage);
    }

    [ClientRpc]
    void RpcDamageThem(int target, int damage)
    {
        mnm.playerList[target].GetComponent<Player>().takeDamage(damage);
    }
}
