using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Monuments : NetworkBehaviour
{
    public MonumentalNetworkManager mnm;

    public static string[] MonumentNames =
    {
        "Bonfire", "Sundial",
        "Statue", "Throne",
        "Crown", "Mirror", "Shrine"
    };

    [SyncVar]
    int monument1;
    [SyncVar]
    int monument2;
    [SyncVar]
    int monument3;
    [SyncVar]
    int monument4;
    [SyncVar]
    int monument5;
    [SyncVar]
    int monument6;
    [SyncVar]
    int monument7;

    public readonly int[] cost1 = { 1000, 100, 0, 0, 0, 0 };
    public readonly int[] cost2 = { 100, 1000, 0, 0, 0, 0 };
    public readonly int[] cost3 = { 0, 0, 1000, 100, 0, 0 };
    public readonly int[] cost4 = { 0, 0, 100, 1000, 0, 0 };
    public readonly int[] cost5 = { 0, 0, 0, 0, 1000, 100 };
    public readonly int[] cost6 = { 0, 0, 0, 0, 100, 1000 };
    public readonly int[] cost7 = { 1000, 1000, 1000, 1000, 1000, 1000 };

    // Start is called before the first frame update
    void Start()
    {
        mnm = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>();
        if (isServer)
        {
            monument1 = -1;
            monument2 = -1;
            monument3 = -1;
            monument4 = -1;
            monument5 = -1;
            monument6 = -1;
            monument7 = -1;
        }
    }

    public int[] GetCost(int mon)
    {
        switch (mon)
        {
            case 1:
                return cost1;
            case 2:
                return cost2;
            case 3:
                return cost3;
            case 4:
                return cost4;
            case 5:
                return cost5;
            case 6:
                return cost6;
            case 7:
                return cost7;
        }
        return null;
    }

    // Returns the team that owns a given monument, -1 represents unowned monuments.
    public int GetOwner(int mon)
    {
        switch (mon)
        {
            case 1:
                return monument1;
            case 2:
                return monument2;
            case 3:
                return monument3;
            case 4:
                return monument4;
            case 5:
                return monument5;
            case 6:
                return monument6;
            case 7:
                return monument6;
        }
        return -2;
    }

    // Returns the number of monuments owned by the given team.
    public int GetScore(int team)
    {
        int score = 0;
        for(int i=0; i<7; i++)
        {
            if (GetOwner(i + 1) == team) score++;
        }
        return score;
    }

    [ClientRpc]
    public void RpcClaimMonument(int mon, int team)
    {
        switch (mon)
        {
            case 1:
                monument1 = team;
                return;
            case 2:
                monument2 = team;
                return;
            case 3:
                monument3 = team;
                return;
            case 4:
                monument4 = team;
                return;
            case 5:
                monument5 = team;
                return;
            case 6:
                monument6 = team;
                return;
            case 7:
                monument7 = team;
                return;
        }
    }
}
