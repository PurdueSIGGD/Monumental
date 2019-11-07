using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;
    [SyncVar]
    public float movementSpeed = 10;
    [SyncVar]
    public float interactionSpeed = 1.0f;
    [SyncVar]
    public float gatherAmount = 1.0f;
    [SyncVar]
    public int meleeDamage = 40;
    [SyncVar]
    public int rangedDamage = 20;

    public void updateStats(PlayerStats ps)
    {
        health = ps.health;
        movementSpeed = ps.movementSpeed;
        interactionSpeed = ps.interactionSpeed;
        gatherAmount = ps.gatherAmount;
        meleeDamage = ps.meleeDamage;
        rangedDamage = ps.rangedDamage;
    }
}
