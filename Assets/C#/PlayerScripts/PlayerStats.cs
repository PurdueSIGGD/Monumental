﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

struct ClassMods {
    static public float ghealth = 1f;
    static public float gmovementSpeed = 1f;
    static public float ginteractionSpeed = 1f;
    static public float ggather = 1.5f;
    static public float gmelee = 1f;
    static public float grange = 1f;
    static public float gcarry = 1.5f;

    static public float fhealth = 1.2f;
    static public float fmovementSpeed = 1f;
    static public float finteractionSpeed = 0.9f;
    static public float fgather = 1f;
    static public float fmelee = 1.5f;
    static public float frange = 1.5f;
    static public float fcarry = 1f;
}

enum classes
{
    gather = 0,
    fighter = 1
}

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
    public int Class = 0;

    [SyncVar]
    public int baseHealth = 100;
    [SyncVar]
    public float baseMovementSpeed = 10;
    [SyncVar]
    public float baseInteractionSpeed = 1.0f;
    [SyncVar]
    public float baseGatherAmount = 1.0f;
    [SyncVar]
    public int baseMeleeDamage = 40;
    [SyncVar]
    public int baseRangedDamage = 20;
    [SyncVar]
    public int baseCarryCapacity = 60;

    public int getHealth()
    {
        if(Class == 0)
        {
            return Mathf.CeilToInt(baseHealth * ClassMods.ghealth);
        }
        return Mathf.CeilToInt(baseHealth * ClassMods.fhealth);
    }

    public float getMovementSpeed()
    {
        if(Class == 0)
        {
            return baseMovementSpeed * ClassMods.gmovementSpeed;
        }
        return baseMovementSpeed * ClassMods.fmovementSpeed;
    }

    public float getInteractionSpeed()
    {
        if (Class == 0)
        {
            return baseInteractionSpeed * ClassMods.ginteractionSpeed;
        }
        return baseInteractionSpeed * ClassMods.finteractionSpeed;
    }

    public float getGatherAmount()
    {
        if (Class == 0)
        {
            return baseGatherAmount * ClassMods.ggather;
        }
        return baseGatherAmount * ClassMods.fgather;
    }

    public int getMeleeDamage()
    {
        if (Class == 0)
        {
            return Mathf.CeilToInt(baseMeleeDamage * ClassMods.gmelee);
        }
        return Mathf.CeilToInt(baseMeleeDamage * ClassMods.fmelee);
    }

    public int getRangedDamage()
    {
        if (Class == 0)
        {
            return Mathf.CeilToInt(baseRangedDamage * ClassMods.grange);
        }
        return Mathf.CeilToInt(baseRangedDamage * ClassMods.frange);
    }

    public int getCarryCapacity()
    {
        if (Class == 0)
        {
            return Mathf.CeilToInt(baseCarryCapacity * ClassMods.gcarry);
        }
        return Mathf.CeilToInt(baseCarryCapacity * ClassMods.fcarry);
    }

    public void changeClass()
    {
        Class = 1 - Class;
    }
}
