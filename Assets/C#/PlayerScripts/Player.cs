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
    public Sprite[] classSprites;
    private SpriteRenderer spriteRender;
    public MonumentalNetworkManager mnm;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;
    public ResourceNode resNode;
    public bool isInBase = false;
    private Vector2 spawn;

    [SyncVar]
    public int teamIndex = -1;
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
		shootingProjectile = GetComponent<ShootingProjectiles>();
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = stats.getHealth();
        resources = GetComponent<ResourceBag>();
        resources.initEmpty();
        healthbar = (Instantiate(Resources.Load("UI/Healthbar")) as GameObject).GetComponentInChildren<Slider>();
        spawn = new Vector2(transform.position.x, transform.position.y);
        timeOfLastClick = Time.time;

        if (isLocalPlayer)
        {
            hitDetect.isTheLocalPlayer = true;
            uiControl = GameObject.FindObjectOfType<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
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
        if (!isLocalPlayer) return;

		float dx = Input.GetAxis("Horizontal");
		float dy = Input.GetAxis("Vertical");
        float divisor = 1;
        if(dx != 0 && dy != 0) { divisor = Mathf.Sqrt(2); }
		body.velocity = new Vector2(dx, dy) * stats.getMovementSpeed() / divisor;
		if (Input.GetMouseButton(0) && !isInBase && timeOfLastClick + stats.getInteractionSpeed() < Time.time)
		{
            timeOfLastClick = Time.time;
			hitDetect.clicked = true;
			shootingProjectile.clicked = true;
		}
        if(Input.GetAxis("Jump") > 0 && isInBase && timeOfLastClick + stats.getInteractionSpeed() < Time.time)
        {
            timeOfLastClick = Time.time;
            stats.changeClass();
            CmdUpdateSprite(teamIndex, stats.Class);
        }
        if (spriteNum == -1)
        {
            CmdUpdateSprite(teamIndex, stats.Class);
            spriteUpdated = true;
        }
        checkForStatsUpdate();
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
        currentHealth = stats.getHealth();
        spriteRender.sprite = classSprites[myBase.teamIndex * 2 + stats.Class];
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
        if (stats.baseHealth != myBase.baseStats.baseHealth || stats.baseMeleeDamage != myBase.baseStats.baseMeleeDamage)
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
        CmdTransferResources(attacker, takenRes);
    }

    //respawns character by setting character to maxHealth, moving the character back to spawn, and giving resources to other player
    public void respawn()
    {
        currentHealth = stats.getHealth();
        CmdRespawn();
    }

    [Command]
    private void CmdRespawn()
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

    void LateUpdate()
    {
        healthbar.value = currentHealth / (float)stats.getHealth();
        healthbar.transform.parent.position = this.transform.position;
    }

    public void setHealth(int val)
    {
        currentHealth = val;
        if (currentHealth <= 0)
        {
			Debug.Log(gameObject.name + " is dead");
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
}
