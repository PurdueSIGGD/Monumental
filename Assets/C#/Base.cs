﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Base : NetworkBehaviour
{
    private Collider2D myCol;
    public int teamIndex;
    private Player localPlayer;
    public PlayerStats baseStats;
    public MonumentalNetworkManager mnm;
    public Monuments monuments;
    public AudioSource enterSound;
    public AudioSource resourceSound;
    public UI_UpgradeMenu upgradeMenu;
    private float lastPurchase;
    private float cooldown = 1;
    const int smallCost = 10;
    const int bigCost = 100;

    const int HealthUpgrade = 30;
    const float MovementUpgrade = 1.02f;
    const float InteractionUpgrade = .98f;
    const float GatherUpgrade = 1.05f;
    const int MeleeUpgrade = 20;
    const int RangedUpgrade = 10;
    const int CarryUpgrade = 20;

    [HideInInspector]
    public ResourceBag resPool;
    [SyncVar]
    int upgrade1level;
    [SyncVar]
    int upgrade2level;
    [SyncVar]
    int upgrade3level;
    [SyncVar]
    int upgrade4level;
    [SyncVar]
    int upgrade5level;
    [SyncVar]
    int upgrade6level;

    // 0 for team 1; 1 for team 2 because indexing
    // Start is called before the first frame update
    void Start()
    {
        myCol = this.GetComponent<Collider2D>();
        resPool = GetComponent<ResourceBag>();
        baseStats = GetComponent<PlayerStats>();
        TeleportTile[] tels = GetComponentsInChildren<TeleportTile>();
        mnm = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>();
        lastPurchase = Time.time;
        for (int i = 0; i < tels.Length; i++)
        {
            tels[i].teamIndex = teamIndex;
        }
        if (isServer)
        {
            upgrade1level = 1;
            upgrade2level = 1;
            upgrade3level = 1;
            upgrade4level = 1;
            upgrade5level = 1;
            upgrade6level = 1;
        }
        monuments = GameObject.FindObjectOfType<Monuments>();
    }

    public int getUpgradeLevel(int up)
    {
        if (up == 1)
        {
            return upgrade1level;
        }
        else if (up == 2)
        {
            return upgrade2level;
        }
        else if (up == 3)
        {
            return upgrade3level;
        }
        else if (up == 4)
        {
            return upgrade4level;
        }
        else if (up == 5)
        {
            return upgrade5level;
        }
        else if (up == 6)
        {
            return upgrade6level;
        }
        else return 0;
    }

    private void incrementUpgradeLevel(int up)
    {
        if (up == 1)
        {
            upgrade1level++;
        }
        else if (up == 2)
        {
            upgrade2level++;
        }
        else if (up == 3)
        {
            upgrade3level++;
        }
        else if (up == 4)
        {
            upgrade4level++;
        }
        else if (up == 5)
        {
            upgrade5level++;
        }
        else if (up == 6)
        {
            upgrade6level++;
        }
        else return;
    }

    float costMath(int i)
    {
        return Mathf.Pow(1.2f, i-1);
    }

    public int[] resourceCostForUpgrade(int upgrade, int level)
    {
        if (upgrade == 1)
        {
            return new int[] { (int)(smallCost * costMath(level)), (int)(bigCost * costMath(level)), 0, 0, 0, 0 };
        }
        else if (upgrade == 2)
        {
            return new int[] { (int)(bigCost * costMath(level)), (int)(smallCost * costMath(level)), 0, 0, 0, 0 };
        }
        else if (upgrade == 3)
        {
            return new int[] {0,0, (int)(smallCost * costMath(level)), (int)(bigCost * costMath(level)), 0, 0};
        }
        else if (upgrade == 4)
        {
            return new int[] {0,0, (int)(bigCost * costMath(level)), (int)(smallCost * costMath(level)), 0, 0 };
        }
        else if (upgrade == 5)
        {
            return new int[] {0,0,0,0, (int)(smallCost * costMath(level)), (int)(bigCost * costMath(level)) };
        }
        else if (upgrade == 6)
        {
            return new int[] {0,0,0,0, (int)(bigCost * costMath(level)), (int)(smallCost * costMath(level)) };
        }
        else
        {
            return null;
        }
    }

    public bool purchaseUpgrade(int up)
    {
        int upLevel = getUpgradeLevel(up);
        if(upLevel == 0)
        {
            return false;
        }
        int[] cost = resourceCostForUpgrade(up, upLevel);
        if (resPool.checkBag(cost) && (Time.time - lastPurchase) > cooldown)
        {
            lastPurchase = Time.time;
            if (localPlayer == null) return false;
            localPlayer.CmdBaseUpgrade(up);
            localPlayer.CmdRemoveBaseResources(cost);
            return true;
        }
        return false;
    }

    public bool purchaseMonument(int mon)
    {
        if  (resPool.checkBag(mnm.monuments.GetCost(mon)) && mnm.monuments.GetOwner(mon) == -1 && (Time.time - lastPurchase) > cooldown)
        {
            lastPurchase = Time.time;
            localPlayer.CmdRemoveBaseResources(mnm.monuments.GetCost(mon));
            int score = monuments.GetScore(teamIndex) + 1; //Dont wait for command
            localPlayer.CmdPurchaseMonument(mon, teamIndex);
            if (score >= 3)
            {
                //WIN GAME
                winGame(teamIndex);
            }

            GameObject.FindObjectOfType<UI_Control>().updateMonument(mon, teamIndex);
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Player p = col.gameObject.GetComponent<Player>();
            if (localPlayer == null && p.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = p;
            }
            if (p.teamIndex == teamIndex)
            {
                if(p.myBase == null)
                {
                    p.myBase = this;
                }
                p.isInBase = true;
                //Heal player to full
                p.currentHealth = p.stats.getHealth();
                /* CHEAT */
                for (int i = 1; i <= 6; i++)
                {
                    resPool.addAmount(i, 10000);
                }

                //dump player resources into pool
                if (!p.resources.isEmpty())
                {
                    resourceSound.Play();
                    p.CmdTransferResToBase(p.resources.dumpResources());
                }

                /* Play entry sound effect */
                enterSound.Play();
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        if (player)
        {
            player.isInBase = false;
        }
    }

    [ClientRpc]
    public void RpcTransferResources(int[] res)
    {
        if(resPool != null) resPool.addBagAsInt(res);
    }

    [ClientRpc]
    public void RpcRemoveResources(int[] res)
    {
        resPool.removeBagAsInt(res);
    }

    [ClientRpc]
    public void RpcBaseUpgrade(int upgrade)
    {
        incrementUpgradeLevel(upgrade);
        lastPurchase = Time.time;
        switch (upgrade)
        {
            case 1: // Melee
                baseStats.baseMeleeDamage += (MeleeUpgrade);
                //baseStats.baseMovementSpeed *= (Mathf.Pow(MovementUpgrade, 1/2));
                //baseStats.baseInteractionSpeed *= (Mathf.Pow(InteractionUpgrade, 1/2));
                speGatUpdate(0);
                return;
            case 2: // Health
                baseStats.baseHealth += (HealthUpgrade);
                //baseStats.baseGatherAmount *= (Mathf.Pow(GatherUpgrade, 1/2));
                speGatUpdate(1);
                return;
            case 3: // Ranged
                baseStats.baseRangedDamage += (RangedUpgrade);
                //baseStats.baseMovementSpeed *= (Mathf.Pow(MovementUpgrade, 1));
                //baseStats.baseInteractionSpeed *= (Mathf.Pow(InteractionUpgrade, 1));
                speGatUpdate(0);
                return;
            case 4: // Carry
                baseStats.baseCarryCapacity += (CarryUpgrade);
                //baseStats.baseGatherAmount *= (Mathf.Pow(GatherUpgrade, 1));
                speGatUpdate(1);
                return;
            case 5: // Speed
                //baseStats.baseMovementSpeed *= (Mathf.Pow(MovementUpgrade, 2));
                //baseStats.baseInteractionSpeed *= (Mathf.Pow(InteractionUpgrade, 2));
                speGatUpdate(0);
                return;
            case 6: // Gather
                //baseStats.baseGatherAmount *= (Mathf.Pow(GatherUpgrade, 2));
                speGatUpdate(1);
                return;
        }
        // Speed = SpeMul (1000 + MeleeLevel + 2 * RangeLevel + 4 * SpeMulLevel)
        // For interaction speed, use - instead of +
        return;
    }
    
    //[ClientRpc]
    public void speGatUpdate(int type) //Speed = 0; Gather = 1;
    {
        if (type == 0)
        {
            baseStats.baseMovementSpeed = Mathf.Pow(MovementUpgrade,getUpgradeLevel0(5))
                * (100 + getUpgradeLevel0(1) + getUpgradeLevel0(3) * 2 + getUpgradeLevel0(5) * 4)/10;
            baseStats.baseInteractionSpeed = Mathf.Pow(InteractionUpgrade, getUpgradeLevel0(5))
                * (100 - getUpgradeLevel0(1) - getUpgradeLevel0(3) * 2 - getUpgradeLevel0(5) * 4)/100;
        }
        else
        {
            baseStats.baseGatherAmount =  Mathf.Pow(GatherUpgrade, getUpgradeLevel0(6))
                * (100 + getUpgradeLevel0(2) + getUpgradeLevel0(4) * 2 + getUpgradeLevel0(6) * 4)/100;
        }
        if (upgradeMenu)
        {
            upgradeMenu.reset(teamIndex);
        }
    }
    public int getUpgradeLevel0(int up)
    {
        return getUpgradeLevel(up) - 1;
    }

    public void winGame(int teamIndex) //Called by base when last monument purchased
    {
        CmdEndGame(teamIndex);
    }

    [Command]
    private void CmdEndGame(int winningTeam)
    {
        RpcEndGame(winningTeam);
    }

    [ClientRpc]
    private void RpcEndGame(int winningTeam)
    {
        Player[] players = GameObject.FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnWinGame(players[i].teamIndex == winningTeam);
        }

    }

}
