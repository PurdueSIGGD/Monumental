using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentalGameManager : MonoBehaviour//NetworkBehaviour
{
    public Transform[] startPoints0;
    public Transform[] startPoints1;
    private List<Player> playersHere = new List<Player>();
    private int[] startPointIndex = {0,0};
    public bool gameHasStarted = false;

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
        int i = p.teamIndex;
        if (i == 0) {
            p.GetComponent<Transform>().position = startPoints0[startPointIndex[i]].position;
            startPointIndex[i] = (startPointIndex[i] + 1) % startPoints0.Length;
        }else if (i == 1)
        {
            p.GetComponent<Transform>().position = startPoints1[startPointIndex[i]].position;
            startPointIndex[i] = (startPointIndex[i] + 1) % startPoints1.Length;
        }
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

    public void winGame(int teamIndex) //Called by base when last monument purchased
    {
        Debug.Log("Grats to team " + teamIndex);
        Player[] players = GameObject.FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnWinGame(players[i].teamIndex == teamIndex);
        }


    }
}
