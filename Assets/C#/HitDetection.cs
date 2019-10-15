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
			Debug.Log(collision.gameObject.name);
		}	
	}
}
