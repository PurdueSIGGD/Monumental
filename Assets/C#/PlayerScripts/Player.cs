using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(ResourceBag))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    public int currentHealth;
    private Rigidbody2D body;
    private Slider healthbar;
    private TMPro.TextMeshProUGUI nameUI;
    private GameObject abovePlayerUI;
    public Sprite[] classSprites;
    private SpriteRenderer spriteRender;
    public MonumentalNetworkManager mnm;
    private MonumentalNetworkMenu mnmenu;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;
    public ResourceNode resNode;
    public bool isInBase = false;
    private Vector2 spawn;
    public Animator attackAnimator;

    [SyncVar]
    public int teamIndex = -1;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public int positionInPlayerList = -1;
    [SyncVar]
    public int spriteNum = -1;
    private bool spriteUpdated = false;
    public Base myBase;

    public GameObject projectile;
	private HitDetection hitDetect;
	private ShootingProjectiles shootingProjectile;
    private float timeOfLastClick;

    private UI_Control uiControl;
    public Sprite[] playerSprites;

    // Start is called before the first frame update
    void Start()
    {
		hitDetect = GetComponentInChildren<HitDetection>();
        attackAnimator = GetComponentInChildren<Animator>();
        shootingProjectile = GetComponent<ShootingProjectiles>();
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = stats.getHealth();
        resources = GetComponent<ResourceBag>();
        resources.initEmpty();

        abovePlayerUI = (Instantiate(Resources.Load("UI/Healthbar")) as GameObject);
        healthbar = abovePlayerUI.GetComponentInChildren<Slider>();
        nameUI = abovePlayerUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        spawn = new Vector2(transform.position.x, transform.position.y);
        timeOfLastClick = Time.time;

        if (isLocalPlayer)
        {
            hitDetect.isTheLocalPlayer = true;
            uiControl = GameObject.FindObjectOfType<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
            mnmenu = GameObject.FindObjectOfType<MonumentalNetworkMenu>();
            uiCamera.followTarget = this.gameObject;
            currentHealth = stats.getHealth();
            spriteRender = this.GetComponent<SpriteRenderer>();
            spriteRender.sprite = classSprites[stats.Class];
        }
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if (spriteNum != -1 && !spriteUpdated)
        {
            this.GetComponent<SpriteRenderer>().sprite = playerSprites[spriteNum];
        }
        checkForStatsUpdate();
        if (!isLocalPlayer) return;

        if (!mnmenu.menuIsShowing)
        {
            float dx = Input.GetAxis("Horizontal");
            float dy = Input.GetAxis("Vertical");
            float divisor = 1;
            if (dx != 0 && dy != 0) { divisor = Mathf.Sqrt(2); }
            body.velocity = new Vector2(dx, dy) * stats.getMovementSpeed() / divisor;
            if (Input.GetMouseButton(0) && !isInBase && timeOfLastClick + stats.getInteractionSpeed() < Time.time)
            {
                attackAnimator.SetBool("isAttacking", true);
                timeOfLastClick = Time.time;
                hitDetect.clicked = true;
                shootingProjectile.clicked = true;
                uiControl.setCooldown(stats.getInteractionSpeed());
            }
            else
            {
                attackAnimator.SetBool("isAttacking", false);
            }
        }
        else
        {
            body.velocity = new Vector2();
        }
        if (spriteNum == -1)
        {
            CmdUpdateSprite(teamIndex, stats.Class);
            spriteUpdated = true;
        }
	}

    private void OnDestroy()
    {
        if (healthbar)
        {
            Destroy(healthbar.transform.parent.gameObject);
        }
    }

    public void changeClass()
    {
        stats.changeClass();
        CmdUpdateSprite(teamIndex, stats.Class);
        currentHealth = stats.getHealth();
    }

    //calculates the difference between the current player and the other player
    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.transform.position);
    }

    //player takes damage of amount damage from player attacker
    public void takeDamage(int damage, int attacker)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            resourceTransfer(attacker);
            respawn();
        }
    }

    private void checkForStatsUpdate()
    {
        if (myBase == null) return;
        if (stats.baseHealth != myBase.baseStats.baseHealth || stats.baseMeleeDamage != myBase.baseStats.baseMeleeDamage ||
            stats.baseCarryCapacity != myBase.baseStats.baseCarryCapacity || stats.baseRangedDamage != myBase.baseStats.baseRangedDamage ||
            stats.baseMovementSpeed != myBase.baseStats.baseMovementSpeed || stats.baseGatherAmount != myBase.baseStats.baseGatherAmount)
        {
            stats.baseHealth = myBase.baseStats.baseHealth;
            stats.baseMovementSpeed = myBase.baseStats.baseMovementSpeed;
            stats.baseInteractionSpeed = myBase.baseStats.baseInteractionSpeed;
            stats.baseGatherAmount = myBase.baseStats.baseGatherAmount;
            stats.baseMeleeDamage = myBase.baseStats.baseMeleeDamage;
            stats.baseRangedDamage = myBase.baseStats.baseRangedDamage;
            stats.baseCarryCapacity = myBase.baseStats.baseCarryCapacity;
            currentHealth = stats.getHealth();
        }
    }

    public void resourceTransfer(int attacker)
    {
        int[] takenRes = resources.dumpResources();
        if (isLocalPlayer)
        {
            CmdTransferResources(attacker, takenRes);
        }
    }

    //respawns character by setting character to maxHealth, moving the character back to spawn, and giving resources to other player
    public void respawn()
    {
        currentHealth = stats.getHealth();
        CmdRespawn();
    }

    [Command]
    public void CmdRespawn()
    {
        RpcRespawn();
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        transform.position = spawn;
    }

    [Command]
    public void CmdTransferResources(int attacker, int[] res)
    {
        mnm.playerList[attacker].GetComponent<Player>().RpcTransferResources(res);
    }

    [Command]
    public void CmdTransferResToBase(int[] res)
    {
        if (myBase == null)
        {
            myBase = mnm.baseList[teamIndex].GetComponent<Base>();
        }
        myBase.RpcTransferResources(res);
    }

    [ClientRpc]
    public void RpcTransferResources(int[] res)
    {
        resources.addBagAsInt(res);
    }

    [ClientRpc]
    public void RpcDumpResources()
    {
        resources.dumpResources();
    }

    [ClientRpc]
    public void RpcClearUI()
    {
        uiControl.clear();
    }

    void LateUpdate()
    {
        healthbar.value = currentHealth / (float)stats.getHealth();
        healthbar.transform.parent.position = this.transform.position;
        nameUI.text = playerName;
    }

    public void setHealth(int val)
    {
        currentHealth = val;
        if (currentHealth <= 0)
        {
			currentHealth = 100;
        }
    }

    public void OnWinGame(bool didWin)
    {
        if (isLocalPlayer)
        {
            if (didWin) {
                uiControl.centerText.text = "VICTORY";
                uiControl.centerText.color = new Color(0, 1, 0);
            }
            else
            {
                uiControl.centerText.text = "DEFEAT";
                uiControl.centerText.color = new Color(1, 0, 0);
            }
        }
    }

    //Sets team the team to whatever the input is
    public void SetTeam(int team)
    {
        teamIndex = team;
    }

    [Command]
    public void CmdUpdateSprite(int team, int Class){
        RpcUpdateSprite(team, Class);
    }

    [ClientRpc]
    void RpcUpdateSprite(int team, int Class)
    {
        int value = team + (2 * Class);
        value = Mathf.Max(value, 0);
        value = Mathf.Min(value, 3);
        spriteNum = value;
        this.GetComponent<SpriteRenderer>().sprite = playerSprites[value];
    }

    public void SetNetManager(MonumentalNetworkManager m)
    {
        mnm = m;
    }

    public void SetPositionInPlayerList(int p)
    {
        positionInPlayerList = p;
    }

    public void gather(int resType, float size)
    {
        resources.addResourceWithLimit(stats.getCarryCapacity(), resNode.gatherPass(stats.getGatherAmount(), resType, size));
    }

    [Command]
    public void CmdDamageThem(int target, int source, int damage)
    {
        mnm.playerList[target].GetComponent<Player>().RpcDamageThem(source, damage);
    }

    [ClientRpc]
    void RpcDamageThem(int source, int damage)
    {
        takeDamage(damage, source);
    }

    [Command]
    public void CmdRemoveBaseResources(int[] res)
    {
        myBase.RpcRemoveResources(res);
    }

    [Command]
    public void CmdBaseUpgrade(int upgrade)
    {
        myBase.RpcBaseUpgrade(upgrade);
    }

    [Command]
    public void CmdPurchaseMonument(int mon, int team)
    {
        mnm.monuments.RpcClaimMonument(mon, team);
    }

    public void chooseName(string name)
    {
        CmdChooseName(name);
    }

    [Command]
    public void CmdChooseName(string name)
    {
        playerName = name;
    }

    [Command]
    public void CmdEndGame(int winningTeam)
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
        //OnWinGame(teamIndex == winningTeam);

    }
}
