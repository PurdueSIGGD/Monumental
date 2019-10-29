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
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;

    [SyncVar]
    public int teamIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        resources = gameObject.AddComponent<ResourceBag>();
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        healthbar = GetComponentInChildren<Slider>();

        if (isLocalPlayer)
        {
            UI_Control uiControl = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
            uiCamera.followTarget = this.gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        body.velocity = new Vector2(dx, dy) * stats.movementSpeed;
    }

    void LateUpdate()
    {
        healthbar.value = stats.health / (float)stats.maxHealth;
    }

    public void takeDamage(int damage)
    {
        setHealth(stats.health - damage);
    }

    public void setHealth(int val)
    {
        stats.health = val;
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
