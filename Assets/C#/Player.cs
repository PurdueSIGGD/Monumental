using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Player : NetworkBehaviour
{
    int attackValue;
    int health;
    float lastAttack = 0f;
    float attackSpeed = 0f;
    float range;
    private Rigidbody2D body;
    public float speed;
    private int resource;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        health = 100;
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
            target.takeDamage(attackValue);
            lastAttack = Time.time;
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }
    // increment resource by x, but don't go negative
    // return the difference
    public int IncrementResource(int x)
    {
        int newres = Math.Max(0, resource + x);
        int dif = newres - resource;
        resource = newres;
        return dif;
    }
}
