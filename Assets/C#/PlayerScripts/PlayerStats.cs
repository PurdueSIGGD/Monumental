using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;
    [SyncVar]
    public int maxHealth = 100;
    [SyncVar]
    public float movementSpeed = 10;
    [SyncVar]
    public float interactionSpeed = 1.0f;
    [SyncVar]
    public float gatherAmount = 1.0f;
    [SyncVar]
    public int meleeDamage = 10;
    [SyncVar]
    public int rangedDamage = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
