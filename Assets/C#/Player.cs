using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int attack;
    int health;
    float lastAttack = 0f;
    float attackSpeed = 0f;
    float range;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        attack = 1;
        attackSpeed = 1;
        range = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.position);
    }

    public void attack(Player target)
    {
        if (Time.time >= lastAttack + 1/attackSpeed && calculateDistance(target) <= range) { 
            target.takeDamage(attack);
            lastAttack = Time.time;
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;  
    }
}
