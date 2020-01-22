using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Base : NetworkBehaviour
{
    private Collider2D myCol;
    public int teamIndex;
    public PlayerStats baseStats;
    public MonumentalNetworkManager mnm;
    public AudioSource enterSound;
    public AudioSource resourceSound;
    private float lastPurchase;
    private float cooldown = 1;
    const int smallCost = 10;
    const int bigCost = 100;

    const int HealthUpgrade = 30;
    const float MovementUpgrade = 1;
    const float InteractionUpgrade = 0.1f;
    const float GatherUpgrade = 1;
    const int MeleeUpgrade = 20;
    const int RangedUpgrade = 10;

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
        return (float)(4+i)/5;
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
            resPool.removeBagAsInt(cost);
            //CmdRemoveResources(cost);
            updateBasePlayerStats(up);
            //up.UpdateStatsAndCost(baseStats);
            UpdateAllPlayerStats(teamIndex, baseStats.baseHealth, baseStats.baseMovementSpeed, baseStats.baseInteractionSpeed,
                        baseStats.baseGatherAmount, baseStats.baseMeleeDamage, baseStats.baseRangedDamage);
            //int upInd = upgrades.IndexOf(up);
            incrementUpgradeLevel(up);
            //CmdUpdateUpgradeList(upInd);
            lastPurchase = Time.time;
            return true;
        }
        return false;
    }

    public bool purchaseMonument(Monument mon)
    {
        if  (resPool.checkBag(mon.cost) && mon.owner < 0)
        {
            resPool.removeBagAsInt(mon.cost);
            mon.updateStatus(teamIndex);

            if (GameObject.Find("MonumentHolder").GetComponent<MonumentHolder>().getScore(teamIndex) >= 3)
            {
                //Insert code to win the game
                GameObject.Find("NetworkManager").GetComponent<MonumentalGameManager>().winGame(teamIndex);

            }
            return true;
        }
        return false;
    }

    private void updateBasePlayerStats(int upgrade)
    {
        if(upgrade % 2 == 0)
        {
            baseStats.baseHealth += HealthUpgrade;
            baseStats.baseMovementSpeed += MovementUpgrade;
            baseStats.baseInteractionSpeed += InteractionUpgrade;
        }
        else
        {
            baseStats.baseGatherAmount += GatherUpgrade;
            baseStats.baseMeleeDamage += MeleeUpgrade;
            baseStats.baseRangedDamage += RangedUpgrade;
        }
    }
    
    public void UpdateAllPlayerStats(int team, int bh, float ms, float isp, float ga, int md, int rd)
    {
        foreach (GameObject player in mnm.playerList)
        {
            if(player.GetComponent<Player>().teamIndex == team)
            {
                PlayerStats pStat = player.GetComponent<PlayerStats>();
                pStat.UpdateStats(bh, ms, isp, ga, md, rd);
                player.GetComponent<Player>().health = pStat.getHealth();
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
                p.health = p.stats.getHealth();

                //dump player resources into pool
                if (!p.resources.isEmpty())
                {
                    resourceSound.Play();
                    resPool.addBagAsInt(p.resources.dumpResources());
                }

                /* Play entry sound effect */
                enterSound.Play();
                
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
