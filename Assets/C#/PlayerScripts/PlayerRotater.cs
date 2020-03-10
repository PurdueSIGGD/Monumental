using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerRotater : NetworkBehaviour
{

    MonumentalNetworkMenu mnmenu;

    private void Start()
    {
        mnmenu = GameObject.FindObjectOfType<MonumentalNetworkMenu>();
    }

    void FixedUpdate()
    {
		if (!isLocalPlayer || mnmenu.menuIsShowing) return;
		//rotate the hitbox to point in the direction of the mouse
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0f;

		Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, -90);
	}
}
