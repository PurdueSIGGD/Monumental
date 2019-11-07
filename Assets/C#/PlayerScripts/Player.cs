﻿using System.Collections;
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
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        resources = gameObject.AddComponent<ResourceBag>();
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        healthbar = (Instantiate(Resources.Load("UI/HealthBar")) as GameObject).GetComponentInChildren<Slider>();

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
    }

    void LateUpdate()
    {
        healthbar.value = stats.health / (float)stats.health;
        healthbar.transform.parent.position = transform.position;
    }

    public void setHealth(int val)
    {
        health = val;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetTeam(int team)
    {
        teamIndex = team;
    }

	//decreases health and destroys gameobject if health reaches 0
	public void takeDamage(int damage)
	{
        setHealth(health - damage);
	}
}
