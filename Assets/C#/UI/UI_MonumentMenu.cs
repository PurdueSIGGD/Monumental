using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_MonumentMenu : NetworkBehaviour
{

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        Monuments myMon = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().monuments;

        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].myMon = myMon;
            buttonList[i].isMonument = true;
            buttonList[i].setPrice(i+1, true);
        }
    }
}
