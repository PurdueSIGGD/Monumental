using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Player : NetworkBehaviour
{
    private Rigidbody2D body;
    public float speed;

	public float hitboxWidth; //the width of the hitbox so the player reaches things farther to the side
	public float hitboxLength; //the length of the hitbox so the player reaches things farther forward

	[SyncVar]
    public int resource;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");
        body.velocity = new Vector2(dx, dy) * speed;
    }

    public int GetResource()
    {
        return resource;
    }

    // increment resource by x, but don't go negative
    // return the difference
    public void IncrementResource(int x)
    {
        resource += x;
    }
}
