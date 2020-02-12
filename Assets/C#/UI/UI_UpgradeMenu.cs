using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{
    
    private bool skip_first_enable = true;
    
    public GameObject[] UpgradeDisplay;
    public GameObject SpeedText;
    public GameObject GatherText;

    private void OnEnable()
    {
        if (skip_first_enable)
        {
            skip_first_enable = false;
            if (NetworkServer.active)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void reset(int team)
    {
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();

        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].setPrice(i+1, false);
            //UpgradeDisplay[i].getComponent<Text>().text = myBase.baseStats;
        }
    }
}
