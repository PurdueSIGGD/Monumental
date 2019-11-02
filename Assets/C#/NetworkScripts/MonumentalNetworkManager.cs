using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MonumentalNetworkManager : NetworkManager
{
    public int[] teamSizes;
    public List<GameObject>[] teamPlayers = new List<GameObject>[2];
    public GameObject[] teamSpawns;

    public override void Start()
    {
        teamSizes = new int[2] { 0, 0 };
        
        if (isHeadless && startOnHeadless)
        {
            StartServer();
        }
    }

    public Transform GetStartPosition(int team)
    {
        return teamSpawns[team].transform;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
    {
        
        int team = 0;
        if(teamSizes[0] > teamSizes[1])
        {
            team = 1;
        }
        Transform startPos = GetStartPosition(team);
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        player.GetComponent<Player>().SetTeam(team);
        teamPlayers[team].Add(player);
        teamSizes[team]++;
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkIdentity p = conn.playerController;
        int team = p.gameObject.GetComponent<Player>().teamIndex;
        if (team >= 0)
        {
            teamSizes[team]--;
        }
        NetworkServer.DestroyPlayerForConnection(conn);
        Debug.Log("OnServerDisconnect: Client disconnected.");
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
    {
        if (player.gameObject != null)
        {
            int team = player.gameObject.GetComponent<Player>().teamIndex;
            if(team >= 0)
            {
                teamSizes[team]--;
            }
            NetworkServer.Destroy(player.gameObject);
        }
        Debug.Log("OnServerRemovePlayer: Client disconnected.");
    }
}
