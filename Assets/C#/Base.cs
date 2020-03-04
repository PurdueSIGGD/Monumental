using System.Collections;
using System.Collections.Generic;
using System;
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
    private UI_Control uiControl;
    private float lastPurchase;
    private float cooldown = 1;
    const int smallCost = 10;
    const int bigCost = 100;

    const int HealthUpgrade = 35;
    const float MovementUpgrade = 1.02f;
    const float InteractionUpgrade = .98f;
    const float GatherUpgrade = 1.05f;
    const int MeleeUpgrade = 18;
    const int RangedUpgrade = 9;
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
        uiControl = GameObject.FindObjectOfType<UI_Control>();
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
                localPlayer.CmdEndGame(teamIndex);
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
        if (resPool != null)
        {
            resPool.addBagAsInt(res);
            for (int i = 0; i < 6; i++)
            {
                if (res[i] > 0)
                {
                    uiControl.pulseResource((ResourceName)(i + 1));
                }
            }
        }
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
                speGatUpdate(0);
                return;
            case 2: // Health
                baseStats.baseHealth += (HealthUpgrade);
                speGatUpdate(1);
                return;
            case 3: // Ranged
                baseStats.baseRangedDamage += (RangedUpgrade);
                speGatUpdate(0);
                return;
            case 4: // Carry
                baseStats.baseCarryCapacity += (CarryUpgrade);
                speGatUpdate(1);
                return;
            case 5: // Speed
                speGatUpdate(0);
                return;
            case 6: // Gather
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

    public float[] previewUpgrade(int upgrade)
    {
        float[] display = new float[8];
        display[upgrade-1] = getUpgradeLevel(upgrade) + 1;

        if (upgrade % 2 == 1)//speed
        {
            int lev5 = getUpgradeLevel0(5) + Math.Sign(display[4]);
            int lev3 = getUpgradeLevel0(3) + Math.Sign(display[2]);
            int lev1 = getUpgradeLevel0(1) + Math.Sign(display[0]);
            Debug.Log("5,3,1: " + lev5 + ","+lev3+","+lev1);
            display[6] = Mathf.Pow(MovementUpgrade, lev5)
                * (100 + lev1 + lev3 * 2 + lev5 * 4) / 10;
        }
        else//gather
        {
            int lev6 = getUpgradeLevel0(6) + Math.Sign(display[5]);
            int lev4 = getUpgradeLevel0(4) + Math.Sign(display[3]);
            int lev2 = getUpgradeLevel0(2) + Math.Sign(display[1]);

            Debug.Log("6,4,2: " + lev6 + "," + lev4 + "," + lev2);
            display[7] = Mathf.Pow(GatherUpgrade, lev6)
                * (100 + lev2 + lev4 * 2 + lev6 * 4) / 100;
        }

        return display;
    }
    

    public int getUpgradeLevel0(int up)
    {
        return getUpgradeLevel(up) - 1;
    }

    

}
