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
    float lastAttack = 0f;
    float attackSpeed = 0f;
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
        spawnPos = body.position;
        health = stats.health;
        attackSpeed = stats.interactionSpeed;
        resources = gameObject.AddComponent<ResourceBag>();
        uiControl = GameObject.Find("Canvas").GetComponent<UI_Control>();
        healthbar = GetComponentInChildren<Slider>();

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

    //calculates the difference between the current player and the other player
    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.transform.position);
    }

    //Checks if the player can attack, and if it does, it causes the other player to take damage of this players attack value
    public void attack(Player target, bool ranged)
    {
        int attackDamage;
        float attackRange;
        if (ranged)
        {
            attackDamage = stats.rangedDamage;
            attackRange = rangedRange;
        }
        else
        {
            attackDamage = stats.meleeDamage;
            attackRange = meleeRange;
        }
        if (Time.time >= lastAttack + 1 / attackSpeed && calculateDistance(target) <= attackRange)
        {

            target.takeDamage(attackDamage, this);
            lastAttack = Time.time;
        }
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
        body.position = spawnPos;
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
            Destroy(gameObject);
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
        foreach (Resource r in myBag.getBag())
        {
            otherBag.addResource(r.getType(), myBag.getAmount(r));
            myBag.removeResource(r.getType());
        }
    }
}
