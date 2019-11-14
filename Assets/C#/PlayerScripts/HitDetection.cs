using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HitDetection : NetworkBehaviour
{
	public Collision2D other;
	public bool clicked;
    public bool isTheLocalPlayer = false;
    private ShootingProjectiles shooting;
    private Player me;
    [SerializeField]
    private BoxCollider2D hitbox;

    void Start()
    {
        me = this.GetComponentInParent<Player>();
        shooting = this.GetComponentInParent<ShootingProjectiles>();
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        if (!isTheLocalPlayer)
        {
            if(hitbox != null) hitbox.enabled = false;
            return;
        }
        else
        {
            if(hitbox != null) hitbox.enabled = true;
        }
		if (clicked)
		{
            Player other;
			if ((other = collision.gameObject.GetComponent<Player>()) != null &&
				other.teamIndex != me.teamIndex)	//if the collision is with someone from a different team
			{
                //deal damage
				me.CmdDamageThem(other.positionInPlayerList, me.positionInPlayerList, me.stats.meleeDamage);

			} else if (collision.gameObject.GetComponent<ResourceNode>() != null)   //if the collision is with a resource
			{
				//gather resource and add it to this player's resource bag
				ResourceNode resource = collision.gameObject.GetComponent<ResourceNode>();
				me.resources.addResource(resource.type, resource.gather().getAmount());
			}
			clicked = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (isTheLocalPlayer)
        {
            clicked = false;
            shooting.cannotShoot();
            shooting.clicked = false;
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (isTheLocalPlayer)
        {
            shooting.canShootNow();
            shooting.clicked = false;
        }
	}
}
