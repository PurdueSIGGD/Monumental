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
    public int health;
    private Rigidbody2D body;
    private Slider healthbar;
    public MonumentalNetworkManager mnm;
    [HideInInspector]
    public PlayerStats stats;
    [HideInInspector]
    public ResourceBag resources;
    public bool isInBase = false;
    private Vector2 spawn;

    [SyncVar]
    public int teamIndex = -1;
    [SyncVar]
    public int positionInPlayerList = -1;

    public GameObject projectile;
	private HitDetection hitDetect;
	private ShootingProjectiles shootingProjectile;
    private float timeOfLastClick;

    // Start is called before the first frame update
    void Start()
    {
		hitDetect = GetComponentInChildren<HitDetection>();
		shootingProjectile = GetComponent<ShootingProjectiles>();
        stats = GetComponent<PlayerStats>();
        body = GetComponent<Rigidbody2D>();
        health = stats.getHealth();
        resources = GetComponent<ResourceBag>();
        healthbar = (Instantiate(Resources.Load("UI/Healthbar")) as GameObject).GetComponentInChildren<Slider>();
        spawn = new Vector2(transform.position.x, transform.position.y);
        timeOfLastClick = Time.time;

        if (isLocalPlayer)
        {
            hitDetect.isTheLocalPlayer = true;
            UI_Control uiControl = GameObject.FindObjectOfType<UI_Control>();
            uiControl.player = this;
            UI_Camera uiCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UI_Camera>();
            uiCamera.followTarget = this.gameObject;
            health = stats.getHealth();
        }
    }

	// Update is called once per frame
	void FixedUpdate()
	{
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
	}

    //calculates the difference between the current player and the other player
    public float calculateDistance(Player there)
    {
        return Vector3.Distance(transform.position, there.transform.position);
    }

    //player takes damage of amount damage from player attacker
    public void takeDamage(int damage, int attacker)
    {
        health -= damage;
        if (health <= 0)
        {
            resourceTransfer(attacker);
            respawn();
        }
    }

    public void resourceTransfer(int attacker)
    {
        int[] takenRes = resources.dumpResourcesAsInt();
        CmdTransferResources(attacker, takenRes);
    }

    //respawns character by setting character to maxHealth, moving the character back to spawn, and giving resources to other player
    public void respawn()
    {
        health = stats.getHealth();
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

    [ClientRpc]
    public void RpcTransferResources(int[] res)
    {
        resources.addBagAsInt(res);
    }

    void LateUpdate()
    {
        healthbar.value = health / (float)stats.getHealth();
        healthbar.transform.parent.position = this.transform.position;
    }

    public void setHealth(int val)
    {
        health = val;
        if (health <= 0)
        {
			Debug.Log(gameObject.name + " is dead");
			health = 100;
        }
    }

    //Sets team the team to whatever the input is
    public void SetTeam(int team)
    {
        teamIndex = team;
    }

    public void SetNetManager(MonumentalNetworkManager m)
    {
        mnm = m;
    }

    public void SetPositionInPlayerList(int p)
    {
        positionInPlayerList = p;
    }
    
    public void giveResToBase(int target)
    {
        int[] res = resources.dumpResourcesAsInt();
        CmdDepositResources(target, res);
    }

    [Command]
    public void CmdDepositResources(int target, int[] res)
    {
        mnm.baseList[target].GetComponent<Base>().CmdReceiveResources(res);
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
}
