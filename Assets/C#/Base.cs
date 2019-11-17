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
    
    [HideInInspector]
    public ResourceBag resPool;
    public SyncListUpgrade upgrades;
    public SyncListInt upgradeLevels;
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
        for (int i = 0; i < 3; i++)
        {
            upgrades.Add(new Upgrade(UpgradeType.Health, i+1));
            upgradeLevels.Add(1);

        }
        for (int i = 0; i < 3; i++)
        {
            upgrades.Add(new Upgrade(UpgradeType.Gather, i+1));
            upgradeLevels.Add(1);
        }
    }

    public bool purchaseUpgrade(Upgrade up)
    {
        if (resPool.checkBag(up.cost) && (Time.time - lastPurchase) > cooldown)
        {
            resPool.removeBag(up.cost);
            up.UpdateStatsAndCost(baseStats);
            updateAllPlayerStats();
            int upInd = upgrades.IndexOf(up);
            upgradeLevels.Insert(upInd, upgradeLevels[upInd] + 1);
            upgradeLevels.RemoveAt(upInd + 1);
            lastPurchase = Time.time;

            return true;
        }
        return false;
    }

    public bool purchaseMonument(Monument mon)
    {
        if  (resPool.checkBag(mon.cost) && mon.owner < 0)
        {
            resPool.removeBag(mon.cost);
            mon.updateStatus(teamIndex);

            if (GameObject.Find("MonumentHolder").GetComponent<MonumentHolder>().getScore(teamIndex) >= 3)
            {
                //Insert code to win the game


            }
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
                PlayerStats pStat = player.GetComponent<PlayerStats>();
                pStat.updateStats(baseStats);
                player.GetComponent<Player>().health = pStat.health;
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
                ResourceBag bag = col.gameObject.GetComponent<ResourceBag>();
                if (!bag.isEmpty())
                {
                    resourceSound.Play();
                    resPool.addBag(bag.dumpResources());
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
