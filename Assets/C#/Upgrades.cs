using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;


public class Upgrades : NetworkBehaviour
{

    int attackIncrease;
    int miningIncrease;
    int healthIncrease;
    float speedIncrease;

    int attackWeakLevel;
    int defenseWeakLevel;
    int attackMedLevel;
    int defenseMedLevel;
    int attackStrongLevel;
    int defenseStrongLevel;

    int attackWeakCost;
    int attackMedCost;
    int attackStrongCost;
    int defenseWeakCost;
    int defenseMedCost;
    int defenseStrongCost;
    // Start is called before the first frame update
    void Start()
    {
        attackIncrease = 0;
        miningIncrease = 0;
        healthIncrease = 0;
        speedIncrease = 0;
        attackWeakLevel = 0;
        defenseWeakLevel = 0;
        attackMedLevel = 0;
        defenseMedLevel = 0;
        attackStrongLevel = 0;
        defenseStrongLevel= 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackWeakCost = attackWeakLevel * 100;
        attackMedCost = attackMedLevel * 100;
        attackStrongCost = attackStrongLevel * 100;
        defenseWeakCost = defenseWeakLevel * 100;
        defenseMedCost = defenseMedLevel * 100;
        defenseStrongCost = defenseStrongLevel * 100;
    }

    void upgradeAttack(String type, Player p)
    {
        if (type.Equals("Strong")){
            if (canBuyUpgrade(type + "Attack", p)){
                p.IncrementResource(attackStrongCost);
                attackStrongLevel++;
                attackIncrease += 4;
                miningIncrease += 4;
            }
        }
        else if (type.Equals("Medium"))
        {
            if (canBuyUpgrade(type + "Attack", p))
            {
                p.IncrementResource(attackMedCost);
                attackMedLevel++;
                attackIncrease += 2;
                miningIncrease += 2;
            }
        }
        else if (type.Equals("Weak")){
            if (canBuyUpgrade(type + "Attack", p)) {
                p.IncrementResource(attackWeakCost);
                attackWeakLevel++;
                attackIncrease++;
                miningIncrease++;
            }
        }
    }
    void upgradeHealth(String type, Player p)
    {
        if (type.Equals("Strong")) {
            if (canBuyUpgrade(type + "Defense", p)){
                p.IncrementResource(defenseStrongCost);
                defenseStrongLevel++;
                healthIncrease += 4;
                speedIncrease += 4;
            }
        } else if (type.Equals("Medium")){
            if (canBuyUpgrade(type + "Defense", p)) {
                p.IncrementResource(defenseMedCost);
                defenseMedLevel++;
                healthIncrease += 2;
                speedIncrease += 2;
            }
        }
        else if (type.Equals("Weak")) {
            if (canBuyUpgrade(type + "Defense", p)){
                p.IncrementResource(defenseWeakCost);
                defenseWeakLevel++;
                healthIncrease++;
                speedIncrease++;
            }
        }
    }

    bool canBuyUpgrade(String upgrade, Player p)
    {
        bool canBuy = false;
        if (upgrade == "WeakAttack")
        {
            if (p.GetResource() >= attackWeakCost)
            {
                canBuy = true;
            }
        }
        else if (upgrade == "MediumAttack")
        {
            if (p.GetResource() >= attackMedCost)
            {
                canBuy = true;
            }
        }
        else if (upgrade == "StrongAttack")
        {
            if (p.GetResource() >= attackStrongCost)
            {
                canBuy = true;
            }
        }
        else if (upgrade == "WeakDefense")
        {
            if (p.GetResource() >= defenseWeakCost)
            {
                canBuy = true;
            }
        }
        else if (upgrade == "MediumDefense"){
            if (p.GetResource() >= defenseMedCost){
                canBuy = true;
            }
        }else if (upgrade == "StrongDefense"){
            if (p.GetResource() >= defenseStrongCost)
            {
                canBuy = true;
            }
        }
        return canBuy;
    }   

}
