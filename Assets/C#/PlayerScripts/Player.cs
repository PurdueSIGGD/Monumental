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
    private UI_Control uiControl = null;
    private Rigidbody2D body;
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
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        setHealth(99);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        body.velocity = new Vector2(dx, dy) * stats.movementSpeed;
    }
    
    public void takeDamage(int damage)
    {
        setHealth(health - damage);
    }

    public void setHealth(int val)
    {
        health = val;
        uiControl.setHealth(health, maxHealth);
    }

    public void SetTeam(int team)
    {
        teamIndex = team;
    }
}
