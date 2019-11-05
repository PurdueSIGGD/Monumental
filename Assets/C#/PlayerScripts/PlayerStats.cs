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

    public void copyStats(PlayerStats cp)
    {
        health = cp.health;
        movementSpeed = cp.movementSpeed;
        interactionSpeed = cp.interactionSpeed;
        gatherAmount = cp.gatherAmount;
        meleeDamage = cp.meleeDamage;
        rangedDamage = cp.rangedDamage;
    }

}
