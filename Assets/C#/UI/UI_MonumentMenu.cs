using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_MonumentMenu : NetworkBehaviour
{

    public UI_Purchase_Button[] buttonList;

    public void Start()
    {
        buttonList = GetComponentsInChildren<UI_Purchase_Button>();
    }

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        Monuments monuments = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().monuments;

        buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].myMon = monuments;
            buttonList[i].isMonument = true;
            buttonList[i].setPrice(i+1, true);
        }
    }

}
