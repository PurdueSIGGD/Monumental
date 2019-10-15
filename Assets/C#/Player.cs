using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed;
    [SyncVar]
    public int resource;
    public int health = 100;
    public int maxHealth = 100;
    private UI_Control uiControl = null;
    [SyncVar]
    public int teamIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
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
        body.velocity = new Vector2(dx, dy) * speed;

    }

    public int GetResource()
    {
        return resource;
    }

    // increment resource by x, but don't go negative
    // return the difference
    public void IncrementResource(int x)
    {
        resource += x;
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
