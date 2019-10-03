using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Camera : MonoBehaviour
{

    public GameObject followTarget = null;
    public Vector2 offset = new Vector2(0, 0);

    void Start()
    {
        
    }
    
    void LateUpdate()
    {
        if (followTarget)
        {
            Vector3 pos = new Vector3();
            pos.x = followTarget.transform.position.x + pos.x;
            pos.y = followTarget.transform.position.y + pos.y;
            pos.z = -1;

            this.transform.position = pos;
        }
    }
}
