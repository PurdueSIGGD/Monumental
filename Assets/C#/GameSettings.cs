using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSettings : NetworkBehaviour
{
    /* Constant Default Game Values */
    private const int DefaultHealth = 35;
    private const float DefaultMovement = 1.02f;
    private const float DefaultInteraction = 0.98f;
    private const float DefaultGather = 1.05f;
    private const int DefaultMelee = 18;
    private const int DefaultRanged = 9;
    private const int DefaultCarry = 20;

    /* Networked values */
    [SyncVar]
    public int Health;
    [SyncVar]
    public float Movement;
    [SyncVar]
    public float Interaction;
    [SyncVar]
    public float Gather;
    [SyncVar]
    public int Melee;
    [SyncVar]
    public int Ranged;
    [SyncVar]
    public int Carry;

    private void Start()
    {
        resetSettings();
    }

    public void resetSettings()
    {
        Health = DefaultHealth;
        Movement = DefaultMovement;
        Interaction = DefaultInteraction;
        Gather = DefaultGather;
        Melee = DefaultMelee;
        Ranged = DefaultRanged;
        Carry = DefaultCarry;
    }

}
