using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Player : NetworkBehaviour
{
    int attackValue;
    [SerializeField]int health;
    [SerializeField] int maxHealth = 100;
    float lastAttack = 0f;
    float attackSpeed = 0f;
    float range;
    private Rigidbody2D body;
    public float speed;
    Vector2 spawnPos;
    [SyncVar]
    public int resource;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spawnPos = body.position;
        health = maxHealth;
        attackValue = 1;
        attackSpeed = 1;
        range = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        body.velocity = new Vector2(dx, dy) * speed;
    }

    public int GetResource()
    {
        return resource;
    }

    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.transform.position);
    }

    public void attack(Player target)
    {
        if (Time.time >= lastAttack + 1 / attackSpeed && calculateDistance(target) <= range)
        {
            target.takeDamage(attackValue, this);
            lastAttack = Time.time;
        }
    }

    public void takeDamage(int damage, Player attacker)
    {
        health -= damage;
        if (health <= 0)
        {
            resourceTransfer(attacker);
            respawn();
        }
    }
    // increment resource by x, but don't go negative
    // return the difference
    public void IncrementResource(int x)
    {
        resource += x;
    }

    //respawns character by setting character to maxHealth, moving the character back to spawn, and giving resources to other player
    public void respawn ()
    {
        health = maxHealth;
        body.position = spawnPos;
    }

    public void resourceTransfer (Player attacker)
    {
        attacker.IncrementResource(this.resource);
        this.resource = 0;
    }
}
