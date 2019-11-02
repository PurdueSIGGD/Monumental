using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentalGameManager : NetworkBehaviour
{
    public Transform [] startPointParents;
    private Transform[][] startPoints = new Transform[2][];
    private List<Player> playersHere = new List<Player>();
    private int[] startPointIndex = {0,0};
    public bool gameHasStarted = false;

    // Start is called before the first frame update
    private void Start()
    {
        for(int i=0; i<startPointParents.Length; i++)
        {
            startPoints[i] = startPointParents[i].GetComponentsInChildren<Transform>();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Player p;
        if((p = col.GetComponent<Player>()) != null)
        {
            if (!playersHere.Contains(p))
            {
                if (gameHasStarted) { SendPlayer(p); }
                else { playersHere.Add(p); }
            }
        }
    }

    private void SendPlayer(Player p)
    {
        p.GetComponent<Transform>().position = startPoints[p.teamIndex][startPointIndex[p.teamIndex]].position;
        startPointIndex[p.teamIndex] = (startPointIndex[p.teamIndex] + 1) % startPoints[p.teamIndex].Length;
    }

    public void StartGame()
    {
        gameHasStarted = true;
        foreach(Player p in playersHere)
        {
            SendPlayer(p);
            playersHere.Remove(p);
        }
    }
}
