﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{
    
    private bool skip_first_enable = true;

    private void OnEnable()
    {
        if (skip_first_enable)
        {
            skip_first_enable = false;
            if (NetworkServer.active)
            {
                this.gameObject.SetActive(false);
                print("Server");
            }
            else
            {
                print("Not server");
            }
        }
    }

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();

        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        Debug.Log("Base: " + myBase.teamIndex);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            Upgrade up = myBase.upgrades[i];
            buttonList[i].setPrice(up);
        }
    }

}
