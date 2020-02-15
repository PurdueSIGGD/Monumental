using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

public class UI_UpgradeMenu : NetworkBehaviour
{
    
    private bool skip_first_enable = true;
    
    public Text[] UpgradeDisplay;
    public Text SpeedText;
    public Text GatherText;

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
        Debug.Log("Updating Menu");
        Base myBase = GameObject.Find("NetworkManager").GetComponent<MonumentalNetworkManager>().baseList[team].GetComponent<Base>();
        myBase.upgradeMenu = this;
        UI_Purchase_Button[] buttonList = GetComponentsInChildren<UI_Purchase_Button>();
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].myBase = myBase;
            buttonList[i].setPrice(i+1, false);
            UpgradeDisplay[i].text = ""+myBase.getUpgradeLevel(i+1);
        }
        SpeedText.text = "" + myBase.baseStats.baseMovementSpeed;
        GatherText.text = "" + myBase.baseStats.baseGatherAmount;
    }
}
