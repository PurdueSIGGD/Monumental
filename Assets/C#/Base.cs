﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Base : NetworkBehaviour
{
    private Collider2D myCol;
    public int teamIndex;
    public PlayerStats baseStats;
    public MonumentalNetworkManager mnm;
    
    [HideInInspector]
    public ResourceBag resPool;
    public SyncListUpgrade upgrades;
    // 0 for team 1; 1 for team 2 because indexing
    // Start is called before the first frame update
    void Start()
    {
        myCol = this.GetComponent<Collider2D>();
        resPool = GetComponent<ResourceBag>();
        baseStats = GetComponent<PlayerStats>();
        TeleportTile[] tels = GetComponentsInChildren<TeleportTile>();
        mnm = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>();
        for (int i = 0; i < tels.Length; i++)
        {
            tels[i].teamIndex = teamIndex;
        }
        for (int i = 1; i < 3; i++)
        {
            upgrades.Add(new Upgrade(UpgradeType.Health, i));
        }
        for (int i = 1; i < 3; i++)
        {
            upgrades.Add(new Upgrade(UpgradeType.Gather, i));
        }
    }

    public bool purchaseUpgrade(Upgrade up)
    {
        if (resPool.checkBag(up.cost))
        {
            resPool.removeBag(up.cost);
            up.UpdateStatsAndCost(baseStats);
            updateAllPlayerStats();


            return true;
        }
        return false;
    }

    public void updateAllPlayerStats()
    {
        foreach (GameObject player in mnm.playerList)
        {
            if(player.GetComponent<Player>().teamIndex == teamIndex)
            {
                player.GetComponent<PlayerStats>().updateStats(baseStats);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Player p = col.gameObject.GetComponent<Player>();
            if(p.teamIndex == teamIndex)
            {
                p.isInBase = true;
                //Heal player to full
                col.gameObject.GetComponent<Player>().health = col.gameObject.GetComponent<PlayerStats>().health;

                //dump player resources into pool
                resPool.addBag(col.gameObject.GetComponent<ResourceBag>().dumpResources());
                
            } else
            {/*
                Rigidbody2D pRigid = p.GetComponentInParent<Rigidbody2D>();
                //pRigid.position += (myCol.GetComponentInParent<Rigidbody2D>().position - pRigid.position);
                pRigid.position = pRigid.position - 2f * Time.deltaTime * pRigid.velocity;
                pRigid.velocity = Vector2.zero;
                Debug.Log("not welcome");
                */
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

}
