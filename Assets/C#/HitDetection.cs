using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
	public Collider2D other;

	void Update()
    {
        
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (collision.gameObject.GetComponent<Player>() != null &&
				collision.gameObject.GetComponent<Player>().teamIndex != transform.root.GetComponent<Player>().teamIndex)	//if the collision is with someone from a different team
			{
				Player other = collision.gameObject.GetComponent<Player>();
				Player me = transform.root.GetComponent<Player>();

				//deal damage
				other.takeDamage(me.stats.meleeDamage);

			} else if (collision.gameObject.GetComponent<ResourceNode>() != null)											//if the collision is with a resource
			{
				ResourceBag bag = transform.root.GetComponent<ResourceBag>();
				ResourceNode resource = collision.gameObject.GetComponent<ResourceNode>();
				bag.addResource(resource.type, resource.gather().getAmount());
			} else																											//if none of those shoot projectile
			{

			}



		}
	}
}
