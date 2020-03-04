using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UI_Camera : NetworkBehaviour
{

    public GameObject followTarget = null;
    public Vector2 offset = new Vector2(0, 1);
    public float FOVDistance = -12;
    
    void FixedUpdate()
    {
        if (followTarget)
        {
            Vector3 pos = new Vector3();
            pos.x = followTarget.transform.position.x + offset.x;
            pos.y = followTarget.transform.position.y + offset.y;
            pos.z = FOVDistance;

            this.transform.position = pos;
        }
    }
}
